using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private MovementTransform moveMent;
    private float projectileDistance = 30; // 발사체 최대 발사 거리
    private int damage = 5; // 발사체의 공격력

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
