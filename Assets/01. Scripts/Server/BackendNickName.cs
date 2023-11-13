using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;
public class BackendNickName : MonoBehaviour
{
    [SerializeField] private InputField nickInput;

    // �ѱ�, ����, ���ڸ� �Է� �����ϰ�
    private bool CheekNickName()
    {
        return Regex.IsMatch(nickInput.text, "^[0-0a-zA-Z-�R]*$");
    }

    // �г��� ����
    public void OnClickCreatName()
    {
        // �ѱ�, ����, ���ڷθ� ��������� üũ
        if (CheekNickName() == false)
        {
            Debug.Log("�г����� �ѱ�, ����, ���ڷθ� ���� �� �ֽ��ϴ�.");
            return;
        }
        
        BackendReturnObject BRO = Backend.BMember.CreateNickname(nickInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("�г��� ���� �Ϸ�");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("�̹� �ߺ��� �г����� �ִ� ���");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20�� �г��� �̻��� ���");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("�г��� ��/�� ������ �ִ� ���");
                    break;

                default:
                    Debug.Log("���� ���� ����: " + BRO.GetErrorCode());
                    break;
            }
        }
    }

    // �г��� ����
    public void OnClickUpdateName()
    {
        // �ѱ�, ����, ���ڷθ� ��������� üũ
        if (CheekNickName() == false)
        {
            Debug.Log("�г����� �ѱ�, ����, ���ڷθ� ���� �� �ֽ��ϴ�.");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.UpdateNickname(nickInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("�г��� ���� �Ϸ�");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("�̹� �ߺ��� �г����� �ִ� ���");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20�� �г��� �̻��� ���");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("�г��� ��/�� ������ �ִ� ���");
                    break;

                default:
                    Debug.Log("���� ���� ����: " + BRO.GetErrorCode());
                    break;
            }
        }
    }
}
