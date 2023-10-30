using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CicrleHP : MonoBehaviour
{
    public Image circleHP;
    public Text hpText; 

    private int currentHP = 100; 
    private int maxHP = 100; 

    void Start()
    {
        UpdateHPText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            DecreaseHP(10);
        }
    }

    public void DecreaseHP(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // �ּҰ� 0, �ִ밪 maxHP�� ����
        circleHP.fillAmount = (float)currentHP / (float)maxHP;
        UpdateHPText();
    }

    private void UpdateHPText()
    {
        hpText.text = currentHP.ToString(); // ���� HP ���� �ؽ�Ʈ�� ǥ��
    }
}
