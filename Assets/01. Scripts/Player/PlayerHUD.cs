using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.NetworkInformation;

public class PlayerHUD : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] private WeaponAssaultRifle wepon; // ���� ������ ��µǴ� ����
    [SerializeField] private Status status; // �÷��̾��� ���� (�̵��ӵ�, ü��)

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

    [Header("HP & BloodScreen UI")]
    //[SerializeField] private TextMeshProUGUI textHp; // �÷��̾� ü�� ��� text 
    [SerializeField] private Image circleHP; // �̹��� hp ���� ���� ��� ����� ���� textHp ���� ��
    [SerializeField] private Text hpText; // �̹��� hp 
    [SerializeField] private Image imageBloodScreen; // �÷��̾ ���ݹ޾��� �� ȭ�鿡 ǥ�õǴ� Image
    [SerializeField] private AnimationCurve curveBloodScreen;

    private void Awake()
    {
        SetupWeapon();
        SetUPMagazine();

        wepon.onAmmoEvent.AddListener(UpdateammoHUD);
        wepon.onMagazineEvent.AddListener(UpdateMagazineHUD);
        status.onHPEvent.AddListener(UpdateammoHUD);
    }

    private void Update() // �̹��� hp�� ���� �� 
    {
        //UpdateHPText();
    }

    /*private void UpdateHPText() // �̹��� hp�� ���� �� 
    {
        hpText.text = currentHP.ToString(); // ���� HP ���� �ؽ�Ʈ�� ǥ��
    }*/

    private void SetupWeapon()
    {
        textWeaponName.text = wepon.WeaponName.ToString();
        imageWeaponIcon.sprite = spriteWeaponIcons[(int)wepon.WeaponName];
    }

    private void UpdateammoHUD(int curretAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{curretAmmo}/</size>{maxAmmo}";
    }

    private void UpdateHUD(int pervious, int current)
    {
        //textHp.text = "HP " + current;

        /*currentHp -= current;
        currentHp = Mathf.Clamp(currentHp, 0, maxHP); // �ּҰ� 0, �ִ밪 maxHP�� ����
        circleHP.fillAmount = (float)currentHp / (float)maxHP;
        UpdateHPText();*/ // �̹��� hp

        if (pervious - current > 0)
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
            //StopCoroutine(OnBloodScreen());
            //StartCoroutine(OnBloodScreen());
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
