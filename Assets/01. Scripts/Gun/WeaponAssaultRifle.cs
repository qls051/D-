using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }
public class WeaponAssaultRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent onMagazineEvent = new MagazineEvent();

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon;  // ���� ���� ����
    [SerializeField] private AudioClip audioClipFire; // ���� ����
    [SerializeField] private AudioClip audioClipReload; // ������ ����

    [Header("WeaponSetting")]
    [SerializeField] private WeaponSetting weaponSetting; // ���� ����

    [Header("Aim UI")]
    [SerializeField] private Image imageAim; // ����Ʈ/���� ��忡 ���� ���� �̹��� ��Ȱ��/Ȱ��

    private float lastAttackTime = 0; // ������ �߻� üũ��
    private bool isReload = false; // ������ ������ üũ
    private bool isAttack = false; // ���� ���� üũ��
    private bool isModeChange = false; // ��� ��ȯ ���� üũ��
    private float defaultModeFOV = 60; // �⺻��忡���� ī�޶� FOV
    private float aimMoveFOV = 40; // Aim��忡���� ī�޶� FOV

    [Header("Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect; // �ѱ� ����Ʈ on/off

    [Header("Spawn Points")]
    [SerializeField] private Transform casingSpawnPoint; // ź�� ���� ��ġ
    [SerializeField] private Transform bulletSpawnPoint; // �Ѿ� ���� ��ġ

    private AudioSource audioSource;
    private PlayerAnimatorController animator;
    private CasingMemoryPool casingMemoryPool; // ź�� ���� �� Ȱ��/��Ȱ�� ����
    private ImpactMemoryPool impactMemoryPool; // ���� ���� �� Ȱ��/��Ȱ�� ����
    private Camera mainCamara; // ���� �߻�

    // �ܺο��� �ʿ��� ������ �����ϱ� ���� ������ get Property's
    public WeaponName WeaponName => weaponSetting.weaponName;
    public int CurrentMagazine => weaponSetting.curretMagazine;
    public int MaxMagazine => weaponSetting.maxMagazine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        animator = GetComponentInParent<PlayerAnimatorController>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamara = Camera.main;

        weaponSetting.currentAmmo = weaponSetting.maxAmmo; // ó�� ź ���� �ִ�� ����
        weaponSetting.curretMagazine = weaponSetting.maxMagazine; // ó�� źâ ���� �ִ�� ����
    }
    private void OnEnable()
    {
        PlayerSound(audioClipTakeOutWeapon); // ���� ���� ���� ���
        muzzleFlashEffect.SetActive(false); // �ѱ� ����Ʈ ��Ȱ��ȭ
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo); // ���Ⱑ Ȱ��ȭ �� �� �ش� ������ ź�� ���� ����
        onMagazineEvent.Invoke(weaponSetting.curretMagazine); // ���Ⱑ Ȱ��ȭ �� �� �ش� ������ źâ ���� ���� 
        ResetVariables();
    }

    public void StartWeaponAction(int type = 0)
    {
        if (isReload == true) return; // ������ ���� ���� ���� �׼� �Ұ��� 

        if (isModeChange == true) return; // ��� ��ȯ ���̸� ���� �׼��� �� �� ����

        if (type == 0) // ���콺 ��Ŭ�� ���� ����
        {
            if (weaponSetting.isAutomaticAttack == true) // ���� ����
            {
                isAttack = true;
                StartCoroutine("OnAttackLoop");
            }
            else // �ܹ� ����
            {
                OnAttack();
            }
        }
        // ���콺 ������ Ŭ�� (��� ��ȯ)
        else
        {
            if (isAttack == true) return; // ���� ���� ���� ��� ��ȯ �Ұ���

            StartCoroutine("OnModeChange");
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        if (type == 0) // ���콺 ��Ŭ�� ���� ����
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        Debug.Log(isReload);
        if (isReload == true || weaponSetting.curretMagazine <= 0) return; // ������ ���̰ų� źâ ���� 0�̸� ������ �Ұ���

        StopWeaponAction(); // ���� �׼� ���߿� RŰ ������ ������ �õ��ϸ� ���� �׼� ���� �� ����

        StartCoroutine("OnReload");
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();

            //yield return new WaitForSeconds(.02f); //������Ÿ��
            yield return null;  
        }
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            if (animator.MoveSpped > 0.5) // �ٰ� ���� ���� ������ �� ����
            {
                return;
            }

            lastAttackTime = Time.time;

            if (weaponSetting.currentAmmo <= 0) // ź ���� ������ ���� �Ұ���
            {
                return;
            }
            weaponSetting.currentAmmo--; // ���ݽ� 1�� ����, ź�� UI ������Ʈ
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // ���� �ִϸ��̼� ��� (��忡 ���� AimFire or Fire �ִϸ��̼� ���)
            //animator.Play("Fire", -1, 0); // ���� �ִϸ��̼�
            string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            animator.Play(animation, -1, 0);            
            if(animator.AimModeIs == false) StartCoroutine("OnmuzzelFlashEffect"); // �ѱ� ����Ʈ ���
            PlayerSound(audioClipFire); // ���� ����
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right); // ź�� ����
            TwoStepRaycast(); // ������ �߻��� ���ϴ� ��ġ ����(+Impact Effact)
        }
    }

    private IEnumerator OnmuzzelFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // ȭ�� �߾� ǥ�� (Aim �������� Raycast����)
        ray = mainCamara.ViewportPointToRay(Vector2.one * 0.5f);
        // ���� ��Ÿ�(attackDistance) �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� ������ �ε��� ��ġ
        if(Physics.Raycast(ray,out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }
        // ���� ��Ÿ� �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� �ִ� ��Ÿ� ��ġ
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        // ù���� Raycast�������� ����� tartgetPoint�� ��ǥ�������� �����ϰ� �ѱ��� ������������ �Ͽ� Raycast����
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if(Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }

    private IEnumerator OnModeChange()
    {
        float current = 0;
        float percent = 0;
        //float time = 0.35f;
        float time = 0.2f;

        animator.AimModeIs = !animator.AimModeIs;
        imageAim.enabled = !imageAim.enabled;

        float start = mainCamara.fieldOfView;
        float end = animator.AimModeIs == true ? aimMoveFOV : defaultModeFOV;

        isModeChange = true;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            //mode�� ���� ī�޶��� �þ߰��� ����
            mainCamara.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        isModeChange = false;
    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
        isModeChange = false;
    }
    private IEnumerator OnReload()
    {
        isReload = true;

        //������ �ִϸ��̼�, ���� ����
        animator.OnReload();
        PlayerSound(audioClipReload);

        while (true)
        {
            // ���尡 ������� �ƴϰ� ���� �ִϸ��̼��� Movement�� ������ �ִϸ��̼�, ���尡 ���� �Ǿ��ٴ� ��
            if (audioSource.isPlaying == false && animator.CurretAnimationIs("Movement"))
            {
                isReload = false;

                // ���� źâ ���� 1���� ��Ű�� �ٲ� źâ ���� Text UI�� ������Ʈ
                weaponSetting.curretMagazine--;
                onMagazineEvent.Invoke(weaponSetting.curretMagazine);   

                // ���� ź���� �ִ�� �����ϰ�, �ٲ� ź �� ������ text UI�� ������Ʈ
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }

            yield return null;
        }
    }

    private void PlayerSound(AudioClip clip)
    {
        audioSource.Stop();       // ������ ������� ���带 �����ϰ�, 
        audioSource.clip = clip;  // ���ο� ���� clip�� ��ä ��
        audioSource.Play();       // ���� ��� 
    }
}
