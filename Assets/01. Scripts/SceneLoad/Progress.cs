using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Progress : MonoBehaviour
{
    [SerializeField] private Slider sliderProgress;
    [SerializeField] private float progressTime; // �ε��� ��� �ð�

    public void Play(UnityAction action = null)
    {
        StartCoroutine(OnProgress(action));
    }

    private IEnumerator OnProgress(UnityAction action)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / progressTime;

            // slider �� ����
            sliderProgress.value = Mathf.Lerp(0, 1, percent);

            yield return null;
        }

        // action�� null�� �ƴϸ� action �޼ҵ� ����
        action?.Invoke();
    }
}
