using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    public static MonsterPool instance;
    public List<Monster> monsterList = new List<Monster>();
    public float summonDelay;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartCoroutine(Co_SummonMonster());
    }
    private IEnumerator Co_SummonMonster()
    {
        float rand;
        while (true)
        {
            rand = Random.Range(0f, 100f);
            print(rand);
            summonDelay = Random.Range(5f, 12f);
            yield return new WaitForSeconds(summonDelay);
            if (rand < 95)
            {
                var monster = monsterList.Find(x => !x.gameObject.activeSelf && x.monsterType == MonsterType.Normal);
                if (monster == null)
                {
                    Debug.Log("몬스터 소환 안됨");
                    continue;
                }
                print(monster.name);
                GameManager.Instance.monsterList.Add(monster);
                summonMonster(monster.gameObject);
                monster.ResetStatus();
            }
            else
            {
                var monster = monsterList.Find(x => !x.gameObject.activeSelf && x.monsterType == MonsterType.Boss);
                if (monster == null)
                {
                    Debug.Log("몬스터 소환 안됨");
                    continue;
                }
                GameManager.Instance.bossList.Add(monster);
                summonMonster(monster.gameObject);
                monster.ResetStatus();
            }
        }
    }
    private void summonMonster(GameObject obj)
    {
        float randPosX = Random.Range(Player.LEFT_MAX, Player.RIGHT_MAX);
        obj.transform.position = new Vector3(randPosX, obj.transform.position.y);
        obj.SetActive(true);
    }
}
