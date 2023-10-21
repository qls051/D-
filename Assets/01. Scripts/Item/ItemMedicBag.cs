using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMedicBag : ItemBase
{
    [SerializeField] private GameObject hpEffactPrefab;
    [SerializeField] private int increaseHP = 50;
    [SerializeField] private float moveDistance = 0.2f;
    [SerializeField] private float pingpongSpeed = 0.5f;
    [SerializeField] private float rotateSpeed = 50;

    private IEnumerator Start()
    {
        float y = transform.position.y;

        while (true)
        {
            // y���� �������� ȸ��
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

            // ó�� ��ġ�� ��ġ�� �������� y ��ġ�� ��, �Ʒ��� �̵�
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(y, y + moveDistance, Mathf.PingPong(Time.time * pingpongSpeed, 1));
            transform.position = position;

            yield return null;
        }
    }

    public override void Use(GameObject entity) // ������ ȹ�� �Լ� 
    {
        entity.GetComponent<Status>().IncreashHP(increaseHP);

        Instantiate(hpEffactPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    } 



}
