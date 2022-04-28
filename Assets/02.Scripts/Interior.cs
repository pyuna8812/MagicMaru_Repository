using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interior : MonoBehaviour
{
    [SerializeField] private ObjectInfo objectInfo;
   // [SerializeField] private 
    void Start()
    {
        GameManager.Instance.interiorObjs.Add($"{objectInfo.Name}", objectInfo);
    }
}
