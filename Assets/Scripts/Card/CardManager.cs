using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public int size;

    private readonly Stack<GameObject> _instances = new Stack<GameObject>();

    private void Awake()
    {
        Assert.IsNotNull(cardPrefab);
    }

    public void Initialize()
    {
        for (int i = 0; i < size; i++)
        {
            var obj = CreateInstance();
            obj.SetActive(false);
            _instances.Push(obj);
        }
    }

    private GameObject CreateInstance()
    {
        var cardObject = Instantiate(cardPrefab, transform, true);
        return cardObject;
    }

    public GameObject GetObject()
    {
        var obj = _instances.Count > 0 ? _instances.Pop() : CreateInstance();
        obj.SetActive(true);
        return obj;
    }
    
    public void ReturnObject(GameObject gameObj)
    {
        var pooledObject = gameObj.GetComponent<ManagedPoolObject>();
        Assert.IsNotNull(pooledObject);
        Assert.IsTrue(pooledObject.cardManager == this);
        
        gameObj.SetActive(false);
        if (!_instances.Contains(gameObj))
        {
            _instances.Push(gameObj);
        }
    }
    
    public class ManagedPoolObject : MonoBehaviour
    {
        public CardManager cardManager;
    }
}
