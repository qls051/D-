using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenade : WeaponBase
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipFire; // ���� ����

    [Header("Grenade")]
    [SerializeField] private GameObject grenadePrefab; // ����ź ������
    [SerializeField] private Transform grenadeSpawnPoint; // ����ź ���� ��ġ

    private void OnEnable()
    {
        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ źâ ������ ����
        onMagazineEvent.Invoke(weaponSetting.curretMagazine);
        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ ź�� ���� ����
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    private void Awake()
    {
        base.Setup();

        // ó�� źâ ���� �ִ�� ����
        weaponSetting.curretMagazine = weaponSetting.maxMagazine;
        // ó�� ź���� �ִ�� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;

    }
    public override void StartReload()
    {
    }

    public override void StartWeaponAction(int type = 0)
    {
        if(type == 0 && isAttack == false && weaponSetting.currentAmmo > 0)
        {
            StartCoroutine("OnAttack");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
    }

    private IEnumerator OnAttack()
    {
        isAttack = true;

        // ���� �ִϸ��̼� ���
        animator.Play("Fire", -1, 0);
        // ���� ���� ���
        PlaySound(audioClipFire);

        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (animator.CurretAnimationIs("Movement"))
            {
                isAttack = false;

                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// arms_assault_rifle_01.fbx��
    /// grenade_throw@assault_rifle_01 �ִϸ��̼� �̺�Ʈ �Լ�
    /// </summary>
    public void SpawnGrenadeProjectile()
    {
        GameObject grenadeClone = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Random.rotation);
        grenadeClone.GetComponent<WeaponGrenadeProjectile>().Setup(weaponSetting.damage, transform.parent.forward);

        weaponSetting.currentAmmo--;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
}
