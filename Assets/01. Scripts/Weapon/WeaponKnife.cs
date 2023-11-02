using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : WeaponBase
{
    [SerializeField] private WeaponKnifeCollider weaponKnifeCollider;

    private void OnEnable()
    {
        isAttack = false;

        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ źâ ������ ����
        onMagazineEvent.Invoke(weaponSetting.curretMagazine);
        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ ź�� ������ ����
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
        if (isAttack == true) return;

        // ���� ����
        if(weaponSetting.isAutomaticAttack == true)
        {
            StartCoroutine("OnAttackLoop", type);
        }
        else
        {
            StartCoroutine("OnAttack", type);
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
        StartCoroutine("OnAttackLoop");
    }

    private IEnumerator OnAttackLoop(int type)
    {
        while (true)
        {
            yield return StartCoroutine("OnAttack", type);
        }
    }

    private IEnumerator OnAttack(int type)
    {
        isAttack = true;

        // ���� ��� ���� (0,1)
        animator.SetFloat("attackType", type);
        // ���� �ִϸ��̼� ���
        animator.Play("Fire", -1, 0);

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

    ///<summary>
    ///arms_assult)rifle_01.fbs�� 
    ///knife_attack_1@assult_rifle_01,knife_attack_1@assult_rifle_01 �ִϸ��̼� �Լ�
    ///</summary>
    public void StartWeaponKnifeCollider()
    {
        weaponKnifeCollider.startCollider(weaponSetting.damage);
    }
}
