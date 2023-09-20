using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHUD : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] private WeaponAssaultRifle wepon; // 현재 정보가 출력되는 무기

    [Header("Weapon Base")]
    [SerializeField] private TextMeshProUGUI textWeaponName; // 무기 이름
    [SerializeField] private Image imageWeaponIcon; // 무기 아이콘
    [SerializeField] private Sprite[] spriteWeaponIcons; // 무기 아이콘에 사용되는 sprite 배열

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI textAmmo; // 현재/최대 탄 수 출력 text

    private void Awake()
    {
        SetupWeapon();

        wepon.onAmmoEvent.AddListener(UpdateammoHUD);
    }

    private void SetupWeapon()
    {
        textWeaponName.text = wepon.WeaponName.ToString();
        imageWeaponIcon.sprite = spriteWeaponIcons[(int)wepon.WeaponName];
    }

    private void UpdateammoHUD(int curretAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{curretAmmo}/</size>{maxAmmo}";
    }
}
