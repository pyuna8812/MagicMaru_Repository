using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float[] goldUnit;
    private float gold;
    public int index;
    public int tapGold;
    public float goldPerSec;
    public Text goldUnitText;
    public Text goldText;
    public Text goldPerSecondText;
    private void Theorem()
    {
        for (int i = 0; i < 26; i++)
        {
            if (goldUnit[i] > 0)
            {
                index = i;
            }
        }
        for (int i = 0; i <= index; i++)
        {
            if(goldUnit[i] >= 1000)
            {
                goldUnit[i] -= 1000;
                goldUnit[i + 1] += 1;
            }
            if(goldUnit[i] < 0)
            {
                if (index > i)
                {
                    goldUnit[i + 1] -= 1;
                    goldUnit[i] += 1000;
                }
            }
        }
    }
    private void UpdateMyGoods()
    {
        float a = goldUnit[index];
        if(index > 0)
        {
            float b = goldUnit[index - 1];
            a += b / 1000;
        }
        if(index == 0)
        {
            a += 0;
        }
        char unit = (char)(65 + index);
        string p = (float)(System.Math.Ceiling(a * 100) / 100) + unit.ToString();
        goldUnitText.text = p;
        goldText.text = "½ÇÁ¦ °ñµå" + gold.ToString("F1");
    }
    private IEnumerator Co_GoodsPerSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            gold += goldPerSec;
            goldUnit[0] += goldPerSec;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_GoodsPerSecond());
        goldPerSecondText.text = "ÃÊ´ç È¹µæ °ñµå·® : " + goldPerSec.ToString();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            goldUnit[0] += tapGold;
            gold += tapGold;
        }
        if (Input.GetMouseButtonDown(1))
        {
            goldUnit[index] -= tapGold;
        }
#endif
        if(Input.touchCount > 0)
        {
            goldUnit[0] += tapGold;
            gold += tapGold;
        }
        Theorem();
        UpdateMyGoods();
    }
}
