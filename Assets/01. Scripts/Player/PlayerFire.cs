using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition;
    public GameObject bombFactory;
    public float throwPower = 15f;

    public GameObject bulletEffect;
    ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = bulletEffect.GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            Rigidbody rigd = bomb.GetComponent<Rigidbody>();
            rigd.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }       
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // 발사 위치와 방향 설정
            RaycastHit hitinfo = new RaycastHit(); // 레이가 부딪힌 대상의 정보 저장

            if(Physics.Raycast(ray, out hitinfo))
            {
                bulletEffect.transform.position = hitinfo.point;
                bulletEffect.transform.forward = hitinfo.normal; 
                particleSystem.Play();
            }
        }
    }
}
