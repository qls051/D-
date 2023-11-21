using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEditor.ShaderKeywordFilter;

public class MemoryPool
{
    // 메모리 풀로 관리되는 오브젝트 정보
    private class PoolItem
    {
        public bool isActive;
        public GameObject gameObject;
    }

    private int increaseCount = 5; // 오브젝트가 부족할 떄 추가 생성되는 오브젝트 개수
    private int maxCount;          // 리스트에 등록되어 있는 오브젝트 개수
    private int activeCount;       // 게임에 사용되고 있는 오브젝트 개수

    private GameObject poolObject;
    private List<PoolItem> poolItemList;

    public int MaxCount => maxCount;
    public int ActiveCount => activeCount;

    // 오브젝트가 임시로 보관되는 위치
    //private Vector3 tempPosition = new Vector3(48, 1, 48);
    private Vector3 tempPosition = new Vector3(4, 1, 4);

    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiaeObjects();
    }

    public void InstantiaeObjects()
    {
        maxCount = increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.transform.position = tempPosition;
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);
        }
    }

    public void DestroyObjects()
    {
        if (poolItemList != null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        if (maxCount == activeCount)
        {
            InstantiaeObjects();
        }

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
            else InstantiaeObjects(); //여기 고쳐야됨
        }
        return null;
    }

    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeObject)
            {
                activeCount--;

                poolItem.gameObject.transform.position = tempPosition;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    public void DeactivateAllPoolItem()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.gameObject.transform.position = tempPosition;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;
    }
}
