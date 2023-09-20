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
    private AudioClip audioClipTakeOutWeapon;  // 무기 장착 사운드
    [SerializeField] private AudioClip audioClipFire; // 공격 사운드
    [SerializeField] private AudioClip audioClipReload; // 재장전 사운드

    [Header("WeaponSetting")]
    [SerializeField] private WeaponSetting weaponSetting; // 무기 설정

    private float lastAttackTime = 0; // 마지막 발사 체크용
    private bool isReload = false; // 재장전 중인지 체크

    [Header("Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect; // 총구 이펙트 on/off

    [Header("Spawn Points")]
    [SerializeField] private Transform casingSpawnPoint; // 탄피 생성 위치

    private CasingMemoryPool casingMemoryPool; // 탄피 생성 후 활성/비활성 관리
    private AudioSource audioSource;
    private PlayerAnimatorController animator;

    public WeaponName WeaponName => weaponSetting.weaponName;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        animator = GetComponentInParent<PlayerAnimatorController>();

        weaponSetting.currentAmmo = weaponSetting.maxAmmo; // 처음 탄 수는 최대로 설정
    }
    private void OnEnable()
    {
        PlayerSound(audioClipTakeOutWeapon); // 무기 장착 사운드 재생
        muzzleFlashEffect.SetActive(false); // 총구 이펙트 비활성화
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo); // 무기가 활성화 될 때 해당 무기의 탄수 정보 갱신
    }

    public void StartWeaponAction(int type = 0)
    {
        if (isReload == true) return; // 재장전 중일 때는 무기 액션 불가능 

        if (type == 0) // 마우스 좌클릭 공격 시작
        {
            if (weaponSetting.isAutomaticAttack == true) // 연속 공격
            {
                StartCoroutine("OnAttackLoop");
            }
            else // 단발 공격
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        if (type == 0) // 마우스 우클릭 공격 종료
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        Debug.Log(isReload);
        if (isReload == true) return; // 재장전 중이면 재장전 불가능

        StopWeaponAction(); // 무기 액션 도중에 R키 눌러서 재장전 시도하면 무기 액션 종료 후 장전

        StartCoroutine("OnReload");
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();

            //yield return new WaitForSeconds(.02f); //딜레이타임
            yield return null;  
        }
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            if (animator.MoveSpped > 0.5) // 뛰고 있을 때는 공격할 수 없음
            {
                return;
            }

            lastAttackTime = Time.time;

            if (weaponSetting.currentAmmo <= 0) // 탄 수가 없으면 공격 불가능
            {
                return;
            }
            weaponSetting.currentAmmo--; // 공격시 1씩 감소, 탄수 UI 업데이트
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            animator.Play("Fire", -1, 0); // 무기 애니메이션
            StartCoroutine("OnmuzzelFlashEffect"); // 총구 이펙트 재생
            PlayerSound(audioClipFire); // 공격 사운드
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right); // 탄피 생성
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

        //재장전 애니메이션, 사운드 실행
        animator.OnReload();
        PlayerSound(audioClipReload);

        while (true)
        {
            //사운드가 재생중이 아니고 현재 애니메이션이 Movement면 재장전 애니메이션, 사운드가 종료 되었다는 뜻
            if (audioSource.isPlaying == false && animator.CurretAnimationIs("Movement"))
            {
                isReload = false;

                //현재 탄수를 최대로 설정하고, 바뀐 탄 수 정보를 text UI에 업데이트
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }

            yield return null;
        }
    }

    private void PlayerSound(AudioClip clip)
    {
        audioSource.Stop();       // 기존에 재생중인 사운드를 정지하고, 
        audioSource.clip = clip;  // 새로운 사운드 clip로 교채 후
        audioSource.Play();       // 사운드 재생 
    }
}
