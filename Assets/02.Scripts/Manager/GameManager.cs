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

    public List<Interior> furnitureList = new List<Interior>();
    public List<Interior> decoList = new List<Interior>();
    public List<Interior> propList = new List<Interior>();
    public List<Interior> balconyList = new List<Interior>();

    public List<Monster> monsterList = new List<Monster>();
    public List<Monster> bossList = new List<Monster>();

    private DateTime exitTime;
    private DateTime startTime;
    private TimeSpan timeInterval;
    private int timeIntervalSecond;

    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    private void Awake()
    {
        instance = this;
        startTime = DateTime.Now;
        exitTime = DateTime.Parse(PlayerPrefs.GetString("Time", DateTime.Now.ToString()));
        timeInterval = (startTime - exitTime);
        timeIntervalSecond = (int)timeInterval.TotalSeconds;
    }
    private IEnumerator Start()
    {
        StartCoroutine(DataManager.Co_LoadData());
        yield return new WaitUntil(() => DataManager.LoadingComplete);
        UpdateGold(goldPerSec * timeIntervalSecond);
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
        DataManager.SaveData(DataManager.DATA_PATH_GOLDPERSEC, goldPerSec, 1);
    }
    public void Tapping()
    {
        if (Player.instance.isDie)
        {
            return;
        }
        gold += (long)tapGold;
    }
    public Quaternion ReversalObjectY(bool left)
    {
        Quaternion q = left ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        return q;
    }
    private void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetString("Time", DateTime.Now.ToString());
            print($"���� �ð� : {DateTime.Now}");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            print($"����� �ð� : {exitTime}, ���� �ð� : {startTime}, ���� �ð� : {timeIntervalSecond}");
        }*/
    }
    private void OnApplicationQuit()
    {
        /*DataManager.SaveData(DataManager.DATA_PATH_TIME, DateTime.Now.ToString(), 1);
        DataManager.SaveData(DataManager.DATA_PATH_GOLD, gold, 1);
        DataManager.SaveData(DataManager.DATA_PATH_POS, Player.instance.transform.position.x, 2);
        DataManager.SaveData(DataManager.DATA_PATH_HP, Player.instance.commonStatus.currentHp, 2);
        DataManager.SaveData(DataManager.DATA_PATH_ISDIE, Player.instance.isDie, 1);
        if (Player.instance.isDie)
        {
            DataManager.SaveData(DataManager.DATA_PATH_RESURRECTION, Player.instance.resurrectionCount, 2);
        }*/
    }
    public void BtnEvt_DeleteData()
    {
        DataManager.ResetData();
    }
}
