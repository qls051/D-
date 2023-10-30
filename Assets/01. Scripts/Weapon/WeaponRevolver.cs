using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponRevolver : WeaponBase
{
    [Header("Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffact; // �ѱ� ����Ʈ (on/off)

    [Header("Spawn Points")]
    [SerializeField] private Transform bulletSpawnPoint; // �Ѿ� ���� ��ġ

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipFire; // ���� ���� 
    [SerializeField] private AudioClip audioClipReload; // ���� ����

    private ImpactMemoryPool impactMemoryPool; // ���� ȿ�� ���� �� Ȱ��/��Ȱ�� ����
    private Camera mainCamera; // ���� �߻�

    private void OnEnable()
    {
        // �ѱ� ����Ʈ ������Ʈ ��Ȱ��ȭ
        muzzleFlashEffact.SetActive(false);

        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ źâ ������ ����
        onMagazineEvent.Invoke(weaponSetting.curretMagazine);

        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ ź �� ������ ����
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }

    private void Awake()
    {
        base.Setup();

        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera = Camera.main;

        // ó�� źâ ���� �ִ�� ����
        weaponSetting.curretMagazine = weaponSetting.maxMagazine;

        // ó�� ź ���� �ִ�� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
        
    }
    public override void StartWeaponAction(int type = 0)
    {
        if(type == 0 && isAttack == false && isReload == false)
        {
            OnAttack();
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
    }

    public override void StartReload()
    {
        // ���� ������ ���̰ų� źâ ���� 0�̸� ������ �Ұ���
        if (isReload == true || weaponSetting.curretMagazine <= 0) return;

        // ���� �׼� ���߿� RŰ�� ���� ������ �õ��ϸ� ���� �׼� ���� �� ������
        StopWeaponAction();

        StartCoroutine("OnReload");
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            if (animator.MoveSpped > 0.5) // �ٰ� ���� ���� ������ �� ����
            {
                return;
            }

            // �����ֱⰡ �Ǿ�� ������ �� �ֱ� ���� ���� �ð� ����
            lastAttackTime = Time.time;

            if (weaponSetting.currentAmmo <= 0) // ź ���� ������ ���� �Ұ���
            {
                return;
            }
            weaponSetting.currentAmmo--; // ���ݽ� 1�� ����, ź�� UI ������Ʈ
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // ���� �ִϸ��̼� ���
            animator.Play("Fire", -1, 0);
            // �ѱ� ����Ʈ ���
            StartCoroutine("OnMuzzleFlashEffact");
            // ���� ���� ���
            PlaySound(audioClipFire);

            // ������ �߻��� ���ϴ� ��ġ ����
            TwoStepRaycast();
            
        }
    }

    private IEnumerator OnMuzzleFlashEffact()
    {
        muzzleFlashEffact.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);

        muzzleFlashEffact.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        //������ �ִϸ��̼�, ���� ����
        animator.OnReload();
        PlaySound(audioClipReload);


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

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // ȭ�� �߾� ǥ�� (Aim �������� Raycast����)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        // ���� ��Ÿ�(attackDistance) �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� ������ �ε��� ��ġ
        if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
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
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);

            if (hit.transform.CompareTag("ImpactEnemy"))
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(weaponSetting.damage);
            }
            else if (hit.transform.CompareTag("InteractionObject"))
            {
                hit.transform.GetComponent<InteractionObject>().TakeDamage(weaponSetting.damage);
            }
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
    }

}
