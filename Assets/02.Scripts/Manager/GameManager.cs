using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //longŸ�� ���� : 9,223,372,036,854,775,807 = 922�� 3372�� 368�� 54775807�� -> ��� ���� ������Ʈ�� ���� ��� longŸ�� �ִ�ġ ���ޱ��� 106�� �ɸ�
    //��� ������ ȹ���� ��� �ʴ� ȹ�� ��� : 1,095,350,000,000 = 1�� 953�� 5õ����
    public double gold;
    public double goldPerSec;
    public double tapGold;
    /// <summary>
    /// 0 = tapGoldLevel, 1 ~ = SkillLevel
    /// </summary>
    public int[] skillLevelArray = { 1, 1, 1, 1, 1 }; // ��� ��ų���� ������ ���� �迭 (0 = �ǰ��, 1���ʹ� ��ų UI ���� ��� -> ������)

    public List<Interior> furnitureList = new List<Interior>();
    public List<Interior> decoList = new List<Interior>();
    public List<Interior> propList = new List<Interior>();
    public List<Interior> balconyList = new List<Interior>();

    public List<Monster> monsterList = new List<Monster>();

    private DateTime exitTime;
    private DateTime startTime;
    private TimeSpan timeInterval;
    private int timeIntervalSecond;

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
        startTime = DateTime.Now;
        exitTime = DateTime.Parse(PlayerPrefs.GetString("Time", DateTime.Now.ToString()));
        timeInterval = (startTime - exitTime);
        timeIntervalSecond = (int)timeInterval.TotalSeconds;
        UpdateGold(goldPerSec * timeIntervalSecond);
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
    public void UpdateGold(double value)
    {
        gold += value;
    }
    public void UpdateGoldPerSec(double value)
    {
        goldPerSec += value;
    }
    public void ReinforceTapGold()
    {
        tapGold = tapGold * 1.2f;
        skillLevelArray[0]++;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetString("Time", DateTime.Now.ToString());
            print($"���� �ð� : {DateTime.Now}");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            print($"����� �ð� : {exitTime}, ���� �ð� : {startTime}, ���� �ð� : {timeIntervalSecond}");
        }
    }
}
