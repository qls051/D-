using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class LoginBase : MonoBehaviour
{
    [SerializeField] private Text textMassage;   

    /// <summary>
    ///  �޽��� ����, InputField ���� �ʱ�ȭ
    /// </summary>
    /// <param name="images"></param>
    protected void ResetUI(params Image[] images)
    {
        textMassage.text = string.Empty;

        for(int  i = 0; i < images.Length; ++i)
        {
            images[i].color = Color.white;
        }
    }
    /// <summary>
    /// �Ű������� �ִ� ������ ���
    /// </summary>
    /// <param name="msg"></param>
    protected void SetMessage(string msg)
    {
        textMassage.text = msg;
    }

    /// <summary>
    /// �Է� ������ �ִ� InputField�� ���� ����
    /// ������ ���� �޽��� ���
    /// </summary>
    /// <param name="image"></param>
    /// <param name="msg"></param>
    protected void GuideForIncorrectlyEnteredData(Image image, string msg)
    {
        textMassage.text = msg;
        image.color = Color.red;
    }

    /// <summary>
    /// �ʵ� ���� ����ִ��� Ȯ�� (image: �ʵ�, field: ����, result: ��µ� ����)
    /// </summary>
    protected bool IsFieldDataEmpty(Image image, string field, string result)
    {
        if (field.Trim().Equals(""))
        {
            GuideForIncorrectlyEnteredData(image, $"\"{result}\"�ʵ带 ä���ּ���.");
            return true;
        }
        return false;
    }
}
