using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //싱글톤 변수 : 한 개의 클래스 인스턴스를 갖도록 보장하고, 이에 대한 전역적인 접근점 제공
    public static GameManager gameManager;

    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;

    // UI text 컴포넌트 변수
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

    //현재 게임 상태 변수
    public GameState gState;
    void Start()
    {
        // 초기 게임 상태를 준비 상태로 설정
        gState = GameState.Ready;
        gametext = gameLabel.GetComponent<Text>();
        gametext.text = "Ready";
        gametext.color = Color.green;

        // 2초 후에 Ready 텍스트를 숨기는 코루틴을 시작
        StartCoroutine(HideReadyTextAfterDelay(2f));
    }
    IEnumerator HideReadyTextAfterDelay(float delay)
    {
        // 지연 시간을 기다린다
        yield return new WaitForSeconds(delay);

        // 2초 후에 텍스트를 숨깁니다
        gametext.enabled = false;
    }

    void Update()
    {
        
    }
}
