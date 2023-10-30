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
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // 최소값 0, 최대값 maxHP로 제한
        circleHP.fillAmount = (float)currentHP / (float)maxHP;
        UpdateHPText();
    }

    private void UpdateHPText()
    {
        hpText.text = currentHP.ToString(); // 현재 HP 값을 텍스트로 표시
    }
}
