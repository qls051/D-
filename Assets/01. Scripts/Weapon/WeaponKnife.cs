using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : WeaponBase
{
    [SerializeField] private WeaponKnifeCollider weaponKnifeCollider;

    private void OnEnable()
    {
        isAttack = false;

        // 무기가 활성화될 때 해당 무기의 탄창 정보를 갱신
        onMagazineEvent.Invoke(weaponSetting.curretMagazine);
        // 무기가 활성화될 때 해당 무기의 탄수 정보를 갱신
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    private void Awake()
    {
        base.Setup();

        // 처음 탄창 수는 최대로 설정
        weaponSetting.curretMagazine = weaponSetting.maxMagazine;
        // 처음 탄수는 최대로 설정
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    public override void StartReload()
    {
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (isAttack == true) return;

        // 연속 공격
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

        // 공격 모드 선택 (0,1)
        animator.SetFloat("attackType", type);
        // 공격 애니메이션 재생
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
    ///arms_assult)rifle_01.fbs의 
    ///knife_attack_1@assult_rifle_01,knife_attack_1@assult_rifle_01 애니메이션 함수
    ///</summary>
    public void StartWeaponKnifeCollider()
    {
        weaponKnifeCollider.startCollider(weaponSetting.damage);
    }
}
