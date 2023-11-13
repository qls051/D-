using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class Login : LoginBase
{
    [SerializeField] private Image imageID; // ID �ʵ� ���� ����
    [SerializeField] private Text inputFieldID; // ID �ʵ� �ؽ�Ʈ ���� ����
    [SerializeField] private Image imagePW; // PW �ʵ� ���� ����
    [SerializeField] private Text inputFieldPW; // PW �ʵ� �ؽ�Ʈ ���� ����

    [SerializeField] private Button btnLogin; // �α��� ��ư (��ȣ�ۿ� ����/�Ұ���)

    /// <summary>
    /// "�α���" ��ư�� ������ �� ȣ��
    /// </summary>
    public void OnClickLogin()
    {
        // �Ű������� �Է��� InputField UI�� ����� Message ���� �ʱ�ȭ
        ResetUI(imageID, imagePW);

        // �ʵ� ���� ����ִ��� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;

        // �α��� ��ư ��Ÿ���� ���ϵ��� ��ȣ�ۿ� ��Ȱ��ȭ
    }
}
