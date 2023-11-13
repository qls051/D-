using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class Login : LoginBase
{
    [SerializeField] private Image imageID; // ID 필드 색상 변경
    [SerializeField] private Text inputFieldID; // ID 필드 텍스트 정보 추출
    [SerializeField] private Image imagePW; // PW 필드 색상 변경
    [SerializeField] private Text inputFieldPW; // PW 필드 텍스트 정보 추출

    [SerializeField] private Button btnLogin; // 로그인 버튼 (상호작용 가능/불가능)

    /// <summary>
    /// "로그인" 버튼을 눌렀을 떄 호출
    /// </summary>
    public void OnClickLogin()
    {
        // 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
        ResetUI(imageID, imagePW);

        // 필드 값이 비어있는지 체크
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;

        // 로그인 버튼 연타하지 못하도록 상호작용 비활성화
    }
}
