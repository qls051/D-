using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHUD : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] private WeaponAssaultRifle wepon; // ���� ������ ��µǴ� ����

    [Header("Weapon Base")]
    [SerializeField] private TextMeshProUGUI textWeaponName; // ���� �̸�
    [SerializeField] private Image imageWeaponIcon; // ���� ������
    [SerializeField] private Sprite[] spriteWeaponIcons; // ���� �����ܿ� ���Ǵ� sprite �迭

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI textAmmo; // ����/�ִ� ź �� ��� text

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
