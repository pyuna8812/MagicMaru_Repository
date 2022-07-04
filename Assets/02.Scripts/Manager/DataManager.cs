using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DataManager
{
    public const string DATA_PATH_ISUNLOCK = "IsUnlock";
    public const string DATA_PATH_LEVEL = "Level";
    public const string DATA_PATH_SKILL = "Skill";
    public const string DATA_PATH_GOLD = "Gold";
    public const string DATA_PATH_GOLDPERSEC = "GoldPerSec";
    public const string DATA_PATH_TAPGOLD = "TapGold";
    public const string DATA_PATH_TYPE = "Type";
    public const string DATA_PATH_TIME = "Time";
    public const string DATA_PATH_POS = "Pos";
    public const string DATA_PATH_HP = "HP";
    public const string DATA_PATH_ISDIE = "IsDie";
    public const string DATA_PATH_RESURRECTION = "Resurrection";
    public static bool LoadingComplete { get; set; }
/// <summary>
/// true = ������ ����, false = ������ �ε�
/// </summary>
/// <param name="isSave"></param>
/// <returns></returns>
    public static IEnumerator Co_LoadData()
    {
        yield return new WaitUntil(() => InteriorData());
        yield return new WaitUntil(() => SkillData());
        yield return new WaitUntil(() => SystemData());
        LoadingComplete = true;
        Debug.Log("������ �ε� ����");
    }
    private static bool InteriorData()
    {
        for (int i = 0; i < 4; i++) 
        {          
            switch (i)
            {
                case 0:
                    InteriorDataInList(GameManager.Instance.furnitureList);
                    break;
                case 1:
                    InteriorDataInList(GameManager.Instance.decoList);
                    break;
                case 2:
                    InteriorDataInList(GameManager.Instance.propList);
                    break;
                case 3:
                    InteriorDataInList(GameManager.Instance.balconyList);
                    break;
                default:
                    Debug.LogError("List not found");
                    return false;
            }         
        }
        return true;
    }
    private static void InteriorDataInList(List<Interior> list)
    {
        foreach (var item in list)
        {
            item.IsUnlock = Convert.ToBoolean(PlayerPrefs.GetString(item.name + DATA_PATH_ISUNLOCK, "false"));
            item.Level = PlayerPrefs.GetInt(item.name + DATA_PATH_LEVEL, 0);
            Debug.Log($"���� : {item.name} ," + "��� ���� : " + PlayerPrefs.GetString(item.name + DATA_PATH_ISUNLOCK, "false") + ", ���� : " + PlayerPrefs.GetInt(item.name + DATA_PATH_LEVEL, 0));
        }
    }
    private static bool SkillData()
    {
        for (int i = 0; i < SkillManager.instance.skillLevelArray.Length; i++)
        {
            SkillManager.instance.skillLevelArray[i] = PlayerPrefs.GetInt(DATA_PATH_SKILL + i.ToString(), 0);
            Debug.Log($"{i}�� ��ų ���� : " + SkillManager.instance.skillLevelArray[i]);
        }
        return true;
    }
    private static bool SystemData()
    {
        GameManager.Instance.gold = Convert.ToDouble(PlayerPrefs.GetString(DATA_PATH_GOLD, "0"));
        GameManager.Instance.goldPerSec = Convert.ToDouble(PlayerPrefs.GetString(DATA_PATH_GOLDPERSEC, "0.5"));
        GameManager.Instance.tapGold = Convert.ToDouble(PlayerPrefs.GetString(DATA_PATH_TAPGOLD, "10"));
        Player.instance.transform.position = new Vector3(PlayerPrefs.GetFloat(DATA_PATH_POS, 0), -1.69f);
        Player.instance.commonStatus.currentHp = PlayerPrefs.GetFloat(DATA_PATH_HP, 500);
        Player.instance.isDie = Convert.ToBoolean(PlayerPrefs.GetString(DATA_PATH_ISDIE, "false"));
        if (Player.instance.isDie)
        {
            Player.instance.resurrectionCount = PlayerPrefs.GetFloat(DATA_PATH_RESURRECTION, 30);
        }
        Debug.Log($"���� ��� : {GameManager.Instance.gold}, �ʴ� ȹ�� ��� :{GameManager.Instance.goldPerSec}, �ǰ�� : {GameManager.Instance.tapGold}");
        return true;
    }
    /// <summary>
    /// index : 0 = ��Ƽ��, 1 = ��Ʈ��, 2 = �÷�Ʈ
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="index"></param>
    public static void SaveData(string key, object value, int index)
    {
        switch (index)
        {
            case 0:
                PlayerPrefs.SetInt(key, (int)value);                
                break;
            case 1:
                PlayerPrefs.SetString(key, value.ToString());
                break;
            case 2:
                PlayerPrefs.SetFloat(key, (float)value);
                break;
            default:
                Debug.LogError("Out of index");
                break;
        }
        Debug.Log($"{key} : {value}");
    }
    public static void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
