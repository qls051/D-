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
        // Ȱ������ ���� ���¿����� ������ ��� ����
        Application.runInBackground  = true;    

        // �ػ� ���� (16:9, 1920x1080)
        int width = Screen.width;
        int height = (int)(Screen.width * 9/16 );
        Screen.SetResolution(width, height, true);

        // ȭ���� ������ �ʵ��� ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // �ε� �ִϸ��̼� ����, ��� �Ϸ� �� OnAfterProgress() �޼ҵ� ����
        progress.Play(OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        //Utils.LoadScene(nextScene);
        SceneManager.LoadScene("Lobby");
    }
}
