using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagazine : ItemBase
{
    [SerializeField] private GameObject magazineEffactPrefab;
    [SerializeField] private int increaseMagazine = 2;
    [SerializeField] private float rotateSpeed = 50;

    private IEnumerator Start()
    {
        while (true)
        {
            // y�� �������� ȸ��
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

            yield return null; 
        }
    }

    public override void Use(GameObject entity)
    {
        entity.GetComponentInChildren<WeaponAssaultRifle>().IncreaseMagazine(increaseMagazine);

        Instantiate(magazineEffactPrefab,transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}