using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [SerializeField] private double[] goldUnit; //∞ÒµÂ «•±‚ ¥‹¿ß (A~Z±Ó¡ˆ √— 26∞≥∑Œ ≥™¥∏)
    private double gold; //«√∑π¿ÃæÓ∞° ∫∏¿Ø«— ∞ÒµÂ
    public int goldUnitMaximum; //¥‹¿ß∫∞∑Œ «•±‚ ∞°¥…«— √÷¥Î ∞ÒµÂ
    public int index; //«ˆ¿Á ∞ÒµÂ «•±‚ ¥‹¿ß∞° ¿ßƒ°«— ¿Œµ¶Ω∫ (A ~ Z ªÁ¿Ã)
    public int tapGold; //»≠∏È ≈«¿∏∑Œ »πµÊ«œ¥¬ ∞ÒµÂ
    public double goldPerSec; //√ ¥Á »πµÊ ∞ÒµÂ
    public Text goldUnitText; //∞ÒµÂ A~Z ¥‹¿ß «•±‚ ≈ÿΩ∫∆Æ
    public Text goldText; //Ω«¡¶ ∞ÒµÂ∑Æ ≈ÿΩ∫∆Æ
    public Text goldPerSecondText; //√ ¥Á »πµÊ ∞ÒµÂ ≈ÿΩ∫∆Æ
    public Dictionary<string, ObjectInfo> interiorObjs = new Dictionary<string, ObjectInfo>();
    public Text level;
    private double UpdateDictionaryInfo()
    {
        double goldPerSec = 0;
        foreach (var item in interiorObjs)
        {
            if (item.Value.Unlock)
            {
                goldPerSec += item.Value.GoldPerSec;
            }
        }
        return goldPerSec;
    }
    public static GameManager Instance { get => instance; }
    public void BtnEvt_UpdateGoldPerSec()
    {
        goldPerSec = UpdateDictionaryInfo();
    }
    public void BtnEvt_CheckDictionary()
    {
        foreach (var item in interiorObjs)
        {
            Debug.Log($"{item.Key} : {item.Value.Level}");
        }
    }
    public void Reinforce(string objectName)
    {
        interiorObjs[objectName].Level++;
        level.text = interiorObjs[objectName].Level.ToString();
    }
    public void BtnEvt_TapGold()
    {
        UpdateGold(tapGold, true);
    }
    public void UpdateGoldPerSec(float value, bool isIncrease)
    {
        goldPerSec += isIncrease ? value : -(value);
        goldPerSecondText.text = "√ ¥Á »πµÊ ∞ÒµÂ∑Æ : " + goldPerSec.ToString();
        UpdateMyGoldUnit();
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
            if(goldUnit[i] >= goldUnitMaximum)
            {
                goldUnit[i] -= goldUnitMaximum;
                goldUnit[i + 1] += 1;
            }
            if(goldUnit[i] < 0)
            {
                if (index > i)
                {
                    goldUnit[i + 1] -= 1;
                    goldUnit[i] += goldUnitMaximum;
                }
            }
        }
        UpdateMyGold();
    }
    private void UpdateMyGold()
    {
        double a = goldUnit[index];
        if(index > 0)
        {
            double b = goldUnit[index - 1];
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
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(instance);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_GoldPerSecond());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMyGoldUnit();
        //UpdateMyGold();
    }
}
