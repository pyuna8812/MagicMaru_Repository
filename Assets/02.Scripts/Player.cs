using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CommonStatus
{
    public int maxHp;
    public int currentHp;
    public int minAttack;
    public int maxAttack;
}
public class Player : MonoBehaviour
{
    public CommonStatus commonStatus;
    private void Awake()
    {
        commonStatus.currentHp = commonStatus.maxHp;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
