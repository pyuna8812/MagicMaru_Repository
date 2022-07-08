using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;

public class GameManager : MonoBehaviour
{
    public const int VERSION = 1;
    //longŸ�� ���� : 9,223,372,036,854,775,807 = 922�� 3372�� 368�� 54775807�� -> ��� ���� ������Ʈ�� ���� ��� longŸ�� �ִ�ġ ���ޱ��� 106�� �ɸ�
    //��� ������ ȹ���� ��� �ʴ� ȹ�� ��� : 1,095,350,000,000 = 1�� 953�� 5õ����
    public double gold; // ��尡 1000 �̸��� ��쿡�� ���
    public double goldPerSec; // �ʴ� ��尡 1000 �̸��� ��쿡�� ���
    public double tapGold; // �� ��尡 1000 �̸��� ��쿡�� ���

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
        gold += goldPerSec * timeIntervalSecond;
        StartCoroutine(Co_GoldPerSec());
        MainUIManager.instance.UpdateGoldUI();
        MainUIManager.instance.UpdateGoldPerSecUI();
    }
    private IEnumerator Co_GoldPerSec()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            UpdateGold(goldPerSec);
        }    
    }
    public void UpdateGold(double value)
    {
        gold += value;
        MainUIManager.instance.UpdateGoldUI();
    }
    public void UpdateGoldPerSec(double value)
    {
        goldPerSec += value;  
        MainUIManager.instance.UpdateGoldPerSecUI();
        DataManager.SaveData(DataManager.DATA_PATH_GOLDPERSEC, goldPerSec, 1);
    }
    public void Tapping()
    {
        UpdateGold(tapGold);
    }
    public UnityEngine.Quaternion ReversalObjectY(bool left)
    {
        UnityEngine.Quaternion q = left ? UnityEngine.Quaternion.Euler(0, 0, 0) : UnityEngine.Quaternion.Euler(0, 180, 0);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateGold(10000000000000000000);
        }
    }
    private void OnApplicationQuit()
    {
        DataManager.SaveData(DataManager.DATA_PATH_TIME, DateTime.Now.ToString(), 1);
        DataManager.SaveData(DataManager.DATA_PATH_GOLD, gold, 1);
        DataManager.SaveData(DataManager.DATA_PATH_POS, Player.instance.transform.position.x, 2);
        DataManager.SaveData(DataManager.DATA_PATH_HP, Player.instance.commonStatus.currentHp, 2);
        DataManager.SaveData(DataManager.DATA_PATH_ISDIE, Player.instance.isDie, 1);
        if (Player.instance.isDie)
        {
            DataManager.SaveData(DataManager.DATA_PATH_RESURRECTION, Player.instance.resurrectionCount, 2);
        }
    }
    public void BtnEvt_DeleteData()
    {
        DataManager.ResetData();
    }
}
