using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CAH.GameSystem.BigNumber;
using System.Numerics;

public class Gold : MonoBehaviour
{
    //longŸ�� ���� : 9,223,372,036,854,775,807 = 922�� 3372�� 368�� 54775807�� -> ��� ���� ������Ʈ�� ���� ��� longŸ�� �ִ�ġ ���ޱ��� 106�� �ɸ�
    //��� ������ ȹ���� ��� �ʴ� ȹ�� ��� : 1095350000000
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
