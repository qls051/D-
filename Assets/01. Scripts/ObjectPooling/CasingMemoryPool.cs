using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CasingMemoryPool : MonoBehaviour
{
    [SerializeField] private GameObject casingPrefab; //ź�� ������Ʈ
    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(casingPrefab);
    }

    public void SpawnCasing(Vector3 position, Vector3 direction)
    {
        GameObject item = memoryPool.ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = Random.rotation; 
        item.GetComponent<Casing>().Setup(memoryPool, direction);
    }
}
