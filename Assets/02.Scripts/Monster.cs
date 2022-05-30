using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public CommonStatus commonStatus;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DecreaseHp(float value)
    {
        commonStatus.currentHp -= value;
        if(commonStatus.currentHp <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.monsterList.Remove(this);
        }
    }
    public void ResetStatus()
    {
        commonStatus.currentHp = commonStatus.maxHp;
    }
    // Update is called once per frame
    void Update()
    {
 
    }
}
