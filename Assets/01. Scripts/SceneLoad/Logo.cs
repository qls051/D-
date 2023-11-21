using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Logo : MonoBehaviour
{
    [SerializeField] private Progress progress;
    [SerializeField] private SceneNames nextScene;
    private void Awake()
    {
        SystemSetup();   
    }

    private void SystemSetup()
    {
        // 활성되지 않은 상태에서도 게임이 계속 진행
        Application.runInBackground  = true;    

        // 해상도 설정 (16:9, 1920x1080)
        int width = Screen.width;
        int height = (int)(Screen.width * 9/16 );
        Screen.SetResolution(width, height, true);

        // 화면이 꺼지지 않도록 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // 로딩 애니메이션 시작, 재생 완료 시 OnAfterProgress() 메소드 실행
        progress.Play(OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        //Utils.LoadScene(nextScene);
        SceneManager.LoadScene("Lobby");
    }
}
