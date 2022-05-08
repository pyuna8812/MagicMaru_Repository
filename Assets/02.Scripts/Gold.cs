using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CAH.GameSystem.BigNumber;
using System.Numerics;

public class Gold : MonoBehaviour
{
    //long타입 길이 : 9,223,372,036,854,775,807 = 922경 3372조 368억 54775807원 -> 모든 가구 오브젝트를 얻은 경우 long타입 최대치 도달까지 106일 걸림
    //모든 가구를 획득한 경우 초당 획득 골드 : 1095350000000
    public long gold;
    public double goldPerSec;
    public Text goldText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetGold());
    }
    private void LateUpdate()
    {
       // BigIntegerManager.GetUnit(gold);
        goldText.text = BigIntegerManager.GetUnit(gold);
    }
    IEnumerator GetGold()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            gold += (long)goldPerSec;      
        }
    }
}
