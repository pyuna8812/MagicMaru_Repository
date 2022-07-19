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
    public double gold;
    public BigInteger bigGold;
    public double goldPerSec;
    public Text goldText;
    // Start is called before the first frame update
    void Start()
    {
        double d = 10;
        //print(string.Format("{0,12:C2}", gold));
        StartCoroutine(GetGold());
        for (int i = 0; i < 1000; i++)
        {
            d = d * 1.2;
        }
        print(d);
    }
    private void LateUpdate()
    {
        //BigIntegerManager.GetUnit(gold);
        goldText.text = gold < 1000 ? gold.ToString("F1") : BigIntegerManager.GetUnit(bigGold);
    }
    IEnumerator GetGold()
    {
        while (gold < 1000)
        {
            yield return new WaitForSeconds(1f);
            gold += goldPerSec;      
        }
        bigGold = BigInteger.Parse(gold.ToString("0"));
        while (true)
        {
            bigGold += BigInteger.Parse(System.Math.Ceiling(goldPerSec).ToString());
            gold += goldPerSec;
            yield return new WaitForSeconds(1f);
        }
    }
}
