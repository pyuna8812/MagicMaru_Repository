using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //longŸ�� ���� : 9,223,372,036,854,775,807 = 922�� 3372�� 368�� 54775807�� -> ��� ���� ������Ʈ�� ���� ��� longŸ�� �ִ�ġ ���ޱ��� 106�� �ɸ�
    //��� ������ ȹ���� ��� �ʴ� ȹ�� ��� : 1,095,350,000,000 = 1�� 953�� 5õ����
    public double gold;
    public double goldPerSec;
    public double tapGold;
    public List<Interior> furnitureList = new List<Interior>();
    public List<Interior> decoList = new List<Interior>();
    public List<Interior> propList = new List<Interior>();
    public List<Interior> balconyList = new List<Interior>();

    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    private void Awake() //������ �ʱ�ȭ�� ���ÿ� ��� Manager Ŭ������ �ʱ�ȭ �Լ��� Delegate�� ���� ȣ���ϵ��� �����ؾ� �Ѵ�. 5/4
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(instance);
    }
    private void Start()
    {
        StartCoroutine(Co_GoldPerSec());
    }
    private IEnumerator Co_GoldPerSec()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            gold += goldPerSec;
        }
    }
    public void UpdateGold(long value)
    {
        gold += value;
    }
    public void UpdateGoldPerSec(double value)
    {
        goldPerSec += value;
    }
    private void LoadInteriorData()
    {
        foreach (var item in furnitureList)
        {
            string goldData = PlayerPrefs.GetString($"{item.gameObject.name}"+"gold", $"{item.defaultGoldPerSec}");
            item.currentGoldPerSec = System.Convert.ToDouble(goldData);
            string activeData = PlayerPrefs.GetString($"{item.gameObject.name}"+"active", $"{item.isActive}");
            item.isActive = System.Convert.ToBoolean(activeData);
        }
    }
    private void SaveInteriorData()
    {
        foreach (var item in furnitureList)
        {
            PlayerPrefs.SetString($"{item.gameObject.name}" + "active", $"{item.isActive}");
            Debug.Log(PlayerPrefs.GetString($"{item.gameObject.name}" + "active", "none"));
        }
    }
    public void BtnEvt_SaveTest()
    {
        SaveInteriorData();
    }
    public void Tapping()
    {
        gold += (long)tapGold;
    }
}
