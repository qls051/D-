using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
public class WeaponAssaultRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon;  // ���� ���� ����
    [SerializeField] private AudioClip audioClipFire; // ���� ����
    [SerializeField] private AudioClip audioClipReload; // ������ ����

    [Header("WeaponSetting")]
    [SerializeField] private WeaponSetting weaponSetting; // ���� ����

    private float lastAttackTime = 0; // ������ �߻� üũ��
    private bool isReload = false; // ������ ������ üũ

    [Header("Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect; // �ѱ� ����Ʈ on/off

    [Header("Spawn Points")]
    [SerializeField] private Transform casingSpawnPoint; // ź�� ���� ��ġ

    private CasingMemoryPool casingMemoryPool; // ź�� ���� �� Ȱ��/��Ȱ�� ����
    private AudioSource audioSource;
    private PlayerAnimatorController animator;

    public WeaponName WeaponName => weaponSetting.weaponName;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        animator = GetComponentInParent<PlayerAnimatorController>();

        weaponSetting.currentAmmo = weaponSetting.maxAmmo; // ó�� ź ���� �ִ�� ����
    }
    private void OnEnable()
    {
        PlayerSound(audioClipTakeOutWeapon); // ���� ���� ���� ���
        muzzleFlashEffect.SetActive(false); // �ѱ� ����Ʈ ��Ȱ��ȭ
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo); // ���Ⱑ Ȱ��ȭ �� �� �ش� ������ ź�� ���� ����
    }

    public void StartWeaponAction(int type = 0)
    {
        if (isReload == true) return; // ������ ���� ���� ���� �׼� �Ұ��� 

        if (type == 0) // ���콺 ��Ŭ�� ���� ����
        {
            if (weaponSetting.isAutomaticAttack == true) // ���� ����
            {
                StartCoroutine("OnAttackLoop");
            }
            else // �ܹ� ����
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        if (type == 0) // ���콺 ��Ŭ�� ���� ����
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        Debug.Log(isReload);
        if (isReload == true) return; // ������ ���̸� ������ �Ұ���

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

            animator.Play("Fire", -1, 0); // ���� �ִϸ��̼�
            StartCoroutine("OnmuzzelFlashEffect"); // �ѱ� ����Ʈ ���
            PlayerSound(audioClipFire); // ���� ����
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right); // ź�� ����
        }
    }

    private IEnumerator OnmuzzelFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        //������ �ִϸ��̼�, ���� ����
        animator.OnReload();
        PlayerSound(audioClipReload);

        while (true)
        {
            //���尡 ������� �ƴϰ� ���� �ִϸ��̼��� Movement�� ������ �ִϸ��̼�, ���尡 ���� �Ǿ��ٴ� ��
            if (audioSource.isPlaying == false && animator.CurretAnimationIs("Movement"))
            {
                isReload = false;

                //���� ź���� �ִ�� �����ϰ�, �ٲ� ź �� ������ text UI�� ������Ʈ
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
