using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int startingSize = 5;

    private List<IPoolable> allItems = new List<IPoolable>();
    private List<IPoolable> availableItems = new List<IPoolable>();

    public void Initialize()
    {
        for (int i = 0; i < startingSize; i++)
        {
            var item = InstantiateObject();
            availableItems.Add(item);
        }
    }

    public IPoolable Spawn()
    {
        IPoolable item = null;
        if(availableItems.Count > 0)
        {
            item = availableItems[0];
            availableItems.RemoveAt(0);
        }
        else
        {
            item = InstantiateObject();
        }
        return item;
    }

    public void BackToPool(IPoolable item)
    {
        if (allItems.Contains(item))
        {
            availableItems.Add(item);
            item.ReturnToPool();
        }
    }

    private IPoolable InstantiateObject()
    {
        var item = Instantiate(prefab).GetComponent<IPoolable>();
        item.gameObject.name = $"{prefab.name}_({allItems.Count})";
        item.gameObject.transform.SetParent(transform);
        item.Initialize(GameManager.Instance.hidePoolPoint.position);
        allItems.Add(item);
        return item;
    }
}
