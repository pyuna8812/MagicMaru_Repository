using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //long타입 길이 : 9,223,372,036,854,775,807 = 922경 3372조 368억 54775807원 -> 모든 가구 오브젝트를 얻은 경우 long타입 최대치 도달까지 106일 걸림
    //모든 가구를 획득한 경우 초당 획득 골드 : 1,095,350,000,000 = 1조 953억 5천만원
    public double gold;
    public double goldPerSec;
    public double tapGold;
    /// <summary>
    /// 0 = tapGoldLevel, 1 ~ = SkillLevel
    /// </summary>
    public int[] skillLevelArray = { 1, 1, 1, 1, 1 }; // 모든 스킬들의 레벨을 담을 배열 (0 = 탭골드, 1부터는 스킬 UI 왼쪽 상단 -> 순으로)

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
    private void Awake() //데이터 초기화와 동시에 모든 Manager 클래스의 초기화 함수를 Delegate를 통해 호출하도록 변경해야 한다. 5/4
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
            print($"저장 시간 : {DateTime.Now}");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            print($"저장된 시간 : {exitTime}, 현재 시간 : {startTime}, 지난 시간 : {timeIntervalSecond}");
        }
    }
}
