using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //�̱��� ���� : �� ���� Ŭ���� �ν��Ͻ��� ������ �����ϰ�, �̿� ���� �������� ������ ����
    public static GameManager gameManager;

    // ���� ���� UI ������Ʈ ����
    public GameObject gameLabel;

    // UI text ������Ʈ ����
    Text gametext;
    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
    }
    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    //���� ���� ���� ����
    public GameState gState;
    void Start()
    {
        // �ʱ� ���� ���¸� �غ� ���·� ����
        gState = GameState.Ready;
        gametext = gameLabel.GetComponent<Text>();
        gametext.text = "Ready";
        gametext.color = Color.green;

        // 2�� �Ŀ� Ready �ؽ�Ʈ�� ����� �ڷ�ƾ�� ����
        StartCoroutine(HideReadyTextAfterDelay(2f));
    }
    IEnumerator HideReadyTextAfterDelay(float delay)
    {
        // ���� �ð��� ��ٸ���
        yield return new WaitForSeconds(delay);

        // 2�� �Ŀ� �ؽ�Ʈ�� ����ϴ�
        gametext.enabled = false;
    }

    void Update()
    {
        
    }
}
