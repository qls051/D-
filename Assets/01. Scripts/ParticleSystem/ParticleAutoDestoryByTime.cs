using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestoryByTime : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // ��ƼŬ�� ������� �ƴϸ� ����
        if(particle.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
