using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseRotation : MonoBehaviour
{
    //public float rotationSpeed = 5.0f; // 회전 속도를 조절할 변수

    float rotSpeed = 1.0f;
    private void Update()
    {
        /*// 마우스 커서 위치를 가져옵니다.
        Vector3 mousePosition = Input.mousePosition;

        // 마우스 커서 위치를 월드 좌표로 변환합니다.
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

        // 플레이어 캐릭터가 부드럽게 마우스 위치를 바라보게 합니다.
        Vector3 direction = mousePosition - transform.position;

        // 방향 벡터의 y와 z 구성 요소를 0으로 설정
        direction.y = 0;
        direction.z = 0;

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);*/

        float MouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * rotSpeed * MouseX);
    }
}
