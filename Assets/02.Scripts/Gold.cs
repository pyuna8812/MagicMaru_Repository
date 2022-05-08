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
