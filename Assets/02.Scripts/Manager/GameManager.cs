using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float[] goldUnit; //∞ÒµÂ «•±‚ ¥‹¿ß (A~Z±Ó¡ˆ √— 26∞≥∑Œ ≥™¥∏)
    private float gold; //«√∑π¿ÃæÓ∞° ∫∏¿Ø«— ∞ÒµÂ
    public int goldUnitMaximum; //¥‹¿ß∫∞∑Œ «•±‚ ∞°¥…«— √÷¥Î ∞ÒµÂ
    public int index; //«ˆ¿Á ∞ÒµÂ «•±‚ ¥‹¿ß∞° ¿ßƒ°«— ¿Œµ¶Ω∫ (A ~ Z ªÁ¿Ã)
    public int tapGold; //»≠∏È ≈«¿∏∑Œ »πµÊ«œ¥¬ ∞ÒµÂ
    public float goldPerSec; //√ ¥Á »πµÊ ∞ÒµÂ
    public Text goldUnitText; //∞ÒµÂ A~Z ¥‹¿ß «•±‚ ≈ÿΩ∫∆Æ
    public Text goldText; //Ω«¡¶ ∞ÒµÂ∑Æ ≈ÿΩ∫∆Æ
    public Text goldPerSecondText; //√ ¥Á »πµÊ ∞ÒµÂ ≈ÿΩ∫∆Æ
    private Touch touch;
    public void BtnEvt_TapGold()
    {
        UpdateGold(tapGold, true);
    }
    private void UpdateGold(float value, bool isIncrease)
    {
        goldUnit[0] += isIncrease ? value : -(value);
        gold += isIncrease ? value : -(value);
    }
    private void UpdateMyGoldUnit()
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
    private void UpdateMyGold()
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
        goldText.text = "Ω«¡¶ ∞ÒµÂ" + gold.ToString("F1");
    }
    private IEnumerator Co_GoldPerSecond()
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
        StartCoroutine(Co_GoldPerSecond());
        goldPerSecondText.text = "√ ¥Á »πµÊ ∞ÒµÂ∑Æ : " + goldPerSec.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMyGoldUnit();
        UpdateMyGold();
    }
}
