using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseRotation : MonoBehaviour
{
    //public float rotationSpeed = 5.0f; // ȸ�� �ӵ��� ������ ����

    float rotSpeed = 1.0f;
    private void Update()
    {
        /*// ���콺 Ŀ�� ��ġ�� �����ɴϴ�.
        Vector3 mousePosition = Input.mousePosition;

        // ���콺 Ŀ�� ��ġ�� ���� ��ǥ�� ��ȯ�մϴ�.
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

        // �÷��̾� ĳ���Ͱ� �ε巴�� ���콺 ��ġ�� �ٶ󺸰� �մϴ�.
        Vector3 direction = mousePosition - transform.position;

        // ���� ������ y�� z ���� ��Ҹ� 0���� ����
        direction.y = 0;
        direction.z = 0;

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);*/

        float MouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * rotSpeed * MouseX);
    }
}
