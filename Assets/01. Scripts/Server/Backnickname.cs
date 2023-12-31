using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;
using TMPro;

public class Backnickname : MonoBehaviour
{
    [SerializeField] private TMP_InputField nickInput;

    // 한글, 영어, 숫자만 입력 가능하게
    private bool CheekNickName()
    {
        return Regex.IsMatch(nickInput.text, "^[0-0a-zA-Z-힣]*$");
    }

    // 닉네임 생성
    public void OnClickCreatName()
    {
        // 한글, 영어, 숫자로만 만들었는지 체크
        if (CheekNickName() == false)
        {
            Debug.Log("닉네임은 한글, 영어, 숫자로만 만들 수 있습니다.");
            return;
        }
        
        BackendReturnObject BRO = Backend.BMember.CreateNickname(nickInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("닉네임 생성 완료");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("이미 중복된 닉네임이 있는 경우");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20자 닉네임 이상인 경우");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("닉네임 앞/뒤 공백이 있는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러: " + BRO.GetErrorCode());
                    break;
            }
        }
    }

    // 닉네임 변경
    public void OnClickUpdateName()
    {
        // 한글, 영어, 숫자로만 만들었는지 체크
        if (CheekNickName() == false)
        {
            Debug.Log("닉네임은 한글, 영어, 숫자로만 만들 수 있습니다.");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.UpdateNickname(nickInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("닉네임 변경 완료");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("이미 중복된 닉네임이 있는 경우");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20자 닉네임 이상인 경우");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("닉네임 앞/뒤 공백이 있는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러: " + BRO.GetErrorCode());
                    break;
            }
        }
    }
}
