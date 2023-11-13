using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;
public class BackendNickName : MonoBehaviour
{
    [SerializeField] private InputField nickInput;

    // ÇÑ±Û, ¿µ¾î, ¼ıÀÚ¸¸ ÀÔ·Â °¡´ÉÇÏ°Ô
    private bool CheekNickName()
    {
        return Regex.IsMatch(nickInput.text, "^[0-0a-zA-Z-ÆR]*$");
    }

    // ´Ğ³×ÀÓ »ı¼º
    public void OnClickCreatName()
    {
        // ÇÑ±Û, ¿µ¾î, ¼ıÀÚ·Î¸¸ ¸¸µé¾ú´ÂÁö Ã¼Å©
        if (CheekNickName() == false)
        {
            Debug.Log("´Ğ³×ÀÓÀº ÇÑ±Û, ¿µ¾î, ¼ıÀÚ·Î¸¸ ¸¸µé ¼ö ÀÖ½À´Ï´Ù.");
            return;
        }
        
        BackendReturnObject BRO = Backend.BMember.CreateNickname(nickInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("´Ğ³×ÀÓ »ı¼º ¿Ï·á");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("ÀÌ¹Ì Áßº¹µÈ ´Ğ³×ÀÓÀÌ ÀÖ´Â °æ¿ì");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20ÀÚ ´Ğ³×ÀÓ ÀÌ»óÀÎ °æ¿ì");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("´Ğ³×ÀÓ ¾Õ/µÚ °ø¹éÀÌ ÀÖ´Â °æ¿ì");
                    break;

                default:
                    Debug.Log("¼­¹ö °øÅë ¿¡·¯: " + BRO.GetErrorCode());
                    break;
            }
        }
    }

    // ´Ğ³×ÀÓ º¯°æ
    public void OnClickUpdateName()
    {
        // ÇÑ±Û, ¿µ¾î, ¼ıÀÚ·Î¸¸ ¸¸µé¾ú´ÂÁö Ã¼Å©
        if (CheekNickName() == false)
        {
            Debug.Log("´Ğ³×ÀÓÀº ÇÑ±Û, ¿µ¾î, ¼ıÀÚ·Î¸¸ ¸¸µé ¼ö ÀÖ½À´Ï´Ù.");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.UpdateNickname(nickInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("´Ğ³×ÀÓ º¯°æ ¿Ï·á");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("ÀÌ¹Ì Áßº¹µÈ ´Ğ³×ÀÓÀÌ ÀÖ´Â °æ¿ì");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20ÀÚ ´Ğ³×ÀÓ ÀÌ»óÀÎ °æ¿ì");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("´Ğ³×ÀÓ ¾Õ/µÚ °ø¹éÀÌ ÀÖ´Â °æ¿ì");
                    break;

                default:
                    Debug.Log("¼­¹ö °øÅë ¿¡·¯: " + BRO.GetErrorCode());
                    break;
            }
        }
    }
}
