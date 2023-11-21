using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private MovementTransform moveMent;
    private float projectileDistance = 30; // �߻�ü �ִ� �߻� �Ÿ�
    private int damage = 5; // �߻�ü�� ���ݷ�

    public void Setup(Vector3 position)
    {
        moveMent = GetComponent<MovementTransform>();

        StartCoroutine("OnMove", position);
    }

    private IEnumerator OnMove(Vector3 targetPositon)
    {
        Vector3 start = transform.position;

        moveMent.MoveTo((targetPositon - transform.position).normalized);

        while (true)
        {
            if(Vector3.Distance(transform.position, start) >= projectileDistance)
            {
                Destroy(gameObject);

                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player Hit");
            other.GetComponent<PlayerController>().TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
