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
            // y축 기준으로 회전
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

            yield return null; 
        }
    }

    public override void Use(GameObject entity)
    {
        // 소지중인 모든 무기의 탄창 수를 증가 
        //entity.GetComponent<WeapnSwitchSystem>().IncreaseMagazine(increaseMagazine);
        // Main 무기(assultRifle)의 탄창 수를 증가
        entity.GetComponent<WeapnSwitchSystem>().IncreaseMagazine(WeaponType.Main, increaseMagazine);

        Instantiate(magazineEffactPrefab,transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
