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
    private AudioClip audioClipTakeOutWeapon;  // 무기 장착 사운드
    [SerializeField] private AudioClip audioClipFire; // 공격 사운드
    [SerializeField] private AudioClip audioClipReload; // 재장전 사운드

    [Header("WeaponSetting")]
    [SerializeField] private WeaponSetting weaponSetting; // 무기 설정

    [Header("Aim UI")]
    [SerializeField] private Image imageAim; // 디폴트/에임 모드에 따라 에임 이미지 비활성/활성

    private float lastAttackTime = 0; // 마지막 발사 체크용
    private bool isReload = false; // 재장전 중인지 체크
    private bool isAttack = false; // 공격 여부 체크용
    private bool isModeChange = false; // 모드 전환 여부 체크용
    private float defaultModeFOV = 60; // 기본모드에서의 카메라 FOV
    private float aimMoveFOV = 40; // Aim모드에서의 카메라 FOV

    [Header("Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect; // 총구 이펙트 on/off

    [Header("Spawn Points")]
    [SerializeField] private Transform casingSpawnPoint; // 탄피 생성 위치
    [SerializeField] private Transform bulletSpawnPoint; // 총알 생성 위치

    private AudioSource audioSource;
    private PlayerAnimatorController animator;
    private CasingMemoryPool casingMemoryPool; // 탄피 생성 후 활성/비활성 관리
    private ImpactMemoryPool impactMemoryPool; // 공격 생성 후 활성/비활성 관리
    private Camera mainCamara; // 광선 발사

    // 외부에서 필요한 정보를 열람하기 위해 정의한 get Property's
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

        weaponSetting.currentAmmo = weaponSetting.maxAmmo; // 처음 탄 수는 최대로 설정
        weaponSetting.curretMagazine = weaponSetting.maxMagazine; // 처음 탄창 수는 최대로 설정
    }
    private void OnEnable()
    {
        PlayerSound(audioClipTakeOutWeapon); // 무기 장착 사운드 재생
        muzzleFlashEffect.SetActive(false); // 총구 이펙트 비활성화
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo); // 무기가 활성화 될 때 해당 무기의 탄수 정보 갱신
        onMagazineEvent.Invoke(weaponSetting.curretMagazine); // 무기가 활성화 될 떄 해당 무기의 탄창 정보 갱신 
        ResetVariables();
    }

    public void StartWeaponAction(int type = 0)
    {
        if (isReload == true) return; // 재장전 중일 때는 무기 액션 불가능 

        if (isModeChange == true) return; // 모드 전환 중이면 무기 액션을 할 수 없다

        if (type == 0) // 마우스 좌클릭 공격 시작
        {
            if (weaponSetting.isAutomaticAttack == true) // 연속 공격
            {
                isAttack = true;
                StartCoroutine("OnAttackLoop");
            }
            else // 단발 공격
            {
                OnAttack();
            }
        }
        // 마우스 오른쪽 클릭 (모드 전환)
        else
        {
            if (isAttack == true) return; // 공격 중일 때는 모드 전환 불가능

            StartCoroutine("OnModeChange");
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        if (type == 0) // 마우스 우클릭 공격 종료
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        Debug.Log(isReload);
        if (isReload == true || weaponSetting.curretMagazine <= 0) return; // 재장전 중이거나 탄창 수가 0이면 재장전 불가능

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

            // 무기 애니메이션 재생 (모드에 따라 AimFire or Fire 애니메이션 재생)
            //animator.Play("Fire", -1, 0); // 무기 애니메이션
            string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            animator.Play(animation, -1, 0);            
            if(animator.AimModeIs == false) StartCoroutine("OnmuzzelFlashEffect"); // 총구 이펙트 재생
            PlayerSound(audioClipFire); // 공격 사운드
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right); // 탄피 생성
            TwoStepRaycast(); // 광선을 발사해 원하는 위치 공격(+Impact Effact)
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

        // 화면 중앙 표시 (Aim 기준으로 Raycast연산)
        ray = mainCamara.ViewportPointToRay(Vector2.one * 0.5f);
        // 공격 사거리(attackDistance) 안에 부딪히는 오브젝트가 있으면 targetPoint는 광선에 부딪힌 위치
        if(Physics.Raycast(ray,out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }
        // 공격 사거리 안에 부딪히는 오브젝트가 없으면 targetPoint는 최대 사거리 위치
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        // 첫번재 Raycast연산으로 얻어진 tartgetPoint를 목표지점으로 연산하고 총구를 시작지점으로 하여 Raycast연산
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

            //mode에 따라 카메라의 시야각을 변경
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

        //재장전 애니메이션, 사운드 실행
        animator.OnReload();
        PlayerSound(audioClipReload);

        while (true)
        {
            // 사운드가 재생중이 아니고 현재 애니메이션이 Movement면 재장전 애니메이션, 사운드가 종료 되었다는 뜻
            if (audioSource.isPlaying == false && animator.CurretAnimationIs("Movement"))
            {
                isReload = false;

                // 현재 탄창 수를 1감소 시키고 바뀐 탄창 수는 Text UI에 업데이트
                weaponSetting.curretMagazine--;
                onMagazineEvent.Invoke(weaponSetting.curretMagazine);   

                // 현재 탄수를 최대로 설정하고, 바뀐 탄 수 정보를 text UI에 업데이트
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
