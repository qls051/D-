using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.NetworkInformation;

public class PlayerHUD : MonoBehaviour
{
    private WeaponBase weapon; // 현재 정보가 출력되는 무기
    [Header("Componets")]
    [SerializeField] private Status status; // 플레이어의 상태 (이동속도, 체력)

    [Header("Weapon Base")]
    [SerializeField] private TextMeshProUGUI textWeaponName; // 무기 이름
    [SerializeField] private Image imageWeaponIcon; // 무기 아이콘
    [SerializeField] private Sprite[] spriteWeaponIcons; // 무기 아이콘에 사용되는 sprite 배열
    [SerializeField] private Vector2[] sizeWeaponIcons; // 무기 아이콘의 UI 배열

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI textAmmo; // 현재/최대 탄 수 출력 text

    [Header("Magazine")]
    [SerializeField] private GameObject magazineUIPrefab; // 탄창 UI 프리펩
    [SerializeField] private Transform magazineParent; // 탄창 UI가 배치되는 패널
    [SerializeField] private int maxMagazineCount; // 처음 생성하는 최대 탄창 수

    private List<GameObject> magazineList; // 탄창 UI 리스트

    [Header("HP & BloodScreen UI")]
    [SerializeField] private TextMeshProUGUI textHp; // 플레이어 체력 출력 text 
    [SerializeField] private Image imageBloodScreen; // 플레이어가 공격받았을 때 화면에 표시되는 Image
    [SerializeField] private AnimationCurve curveBloodScreen;

    private void Awake()
    {
        status.onHPEvent.AddListener(UpdateHPHUD);
    } 

    public void SetupAllWeapons(WeaponBase[] weapons)
    {
        SetUPMagazine();

        // 사용 가능한 모든 무기의 이벤트 등록
        for (int i = 0; i < weapons.Length; ++i)
        {
            weapons[i].onAmmoEvent.AddListener(UpdateammoHUD);
            weapons[i].onMagazineEvent.AddListener(UpdateMagazineHUD);
        }
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;

        SetupWeapon();

    }

    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        imageWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
        imageWeaponIcon.rectTransform.sizeDelta = sizeWeaponIcons[(int)weapon.WeaponName];
    }

    private void UpdateammoHUD(int curretAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{curretAmmo}/</size>{maxAmmo}";
    }

    private void UpdateHPHUD(int pervious, int current)
    {
        textHp.text = "HP " + current;

        // 체력이 증가했을 땐 화면에 빨간색 이미지를 출력하지 않도록  return
        if (pervious <= current) return;

        if (pervious - current > 0)
        {
            //StopCoroutine("OnBloodScreen");
            //StartCoroutine("OnBloodScreen");
        }
    }

    private IEnumerator OnBloodScreen()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime;

            Color color = imageBloodScreen.color;
            color.a = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
            imageBloodScreen.color = color;

            yield return null;  
        }
    }

    private void SetUPMagazine()
    {
        // weapon에 등록되어 있는 최대 탄창 개수만큼 Image Icon을 생성
        // magazineParent 오브젝트의 자식으로 등록 후 모두 비활성화/리스트에 저장
        magazineList = new List<GameObject>();
        for (int i = 0; i < maxMagazineCount; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }
    }

    private void UpdateMagazineHUD(int currentMagazine)
    {
        // 전부 비활성화하고 curretMagazine 개수만큼 활성화
        for (int i = 0; i < magazineList.Count; ++i)
        {
            magazineList[i].SetActive(false);
        }

        for(int i = 0; i < currentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }

}
