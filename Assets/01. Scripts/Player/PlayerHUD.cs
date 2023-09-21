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

    [Header("Magazine")]
    [SerializeField] private GameObject magazineUIPrefab; // źâ UI ������
    [SerializeField] private Transform magazineParent; // źâ Ui�� ��ġ�Ǵ� �г�

    private List<GameObject> magazineList; // źâ UI ����Ʈ

    private void Awake()
    {
        SetupWeapon();
        SetUPMagazine();

        wepon.onAmmoEvent.AddListener(UpdateammoHUD);
        wepon.onMagazineEvent.AddListener(UpdateMagazineHUD);
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

    private void SetUPMagazine()
    {
        // weapon�� ��ϵǾ� �ִ� �ִ� źâ ������ŭ Image Icon�� ����
        // magazineParent ������Ʈ�� �ڽ����� ��� �� ��� ��Ȱ��ȭ/����Ʈ�� ����
        magazineList = new List<GameObject>();
        for (int i = 0; i < wepon.MaxMagazine; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }

        // wepon�� ��ϵǾ� �ִ� ���� źâ ������ŭ ������Ʈ Ȱ��ȭ
        for (int i = 0; i < wepon.CurrentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }

    private void UpdateMagazineHUD(int currentMagazine)
    {
        // ���� ��Ȱ��ȭ�ϰ� curretMagazine ������ŭ Ȱ��ȭ
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
