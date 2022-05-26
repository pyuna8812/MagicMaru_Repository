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
        while (true)
        {
            yield return null;
            float rand = Random.Range(0f, 100f);
            print(rand);
            if(rand < 95)
            {
                var monster = monsterList.Find(x => x.name.Contains("Monster"));
                monsterList.Remove(monster);
                GameManager.Instance.monsterList.Add(monster);
                monster.ResetStatus();
                summonMonster(monster.gameObject);
            }
            else
            {
                var monster = monsterList.Find(x => x.name.Contains("Boss"));
                monsterList.Remove(monster);
                GameManager.Instance.monsterList.Add(monster);
                monster.ResetStatus();
                summonMonster(monster.gameObject);
            }
            summonDelay = Random.Range(5f, 12f);
            yield return new WaitForSeconds(summonDelay);
        }
    }
    private void summonMonster(GameObject obj)
    {
        float randPosX = Random.Range(Player.LEFT_MAX, Player.RIGHT_MAX);
        obj.transform.position = new Vector3(randPosX, obj.transform.position.y);
        obj.SetActive(true);
    }
}
