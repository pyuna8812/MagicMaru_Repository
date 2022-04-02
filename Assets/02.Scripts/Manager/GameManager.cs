using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float[] goldUnit; //��� ǥ�� ���� (A~Z���� �� 26���� ����)
    private float gold; //�÷��̾ ������ ���
    public int goldUnitMaximum; //�������� ǥ�� ������ �ִ� ���
    public int index; //���� ��� ǥ�� ������ ��ġ�� �ε��� (A ~ Z ����)
    public int tapGold; //ȭ�� ������ ȹ���ϴ� ���
    public float goldPerSec; //�ʴ� ȹ�� ���
    public Text goldUnitText; //��� A~Z ���� ǥ�� �ؽ�Ʈ
    public Text goldText; //���� ��差 �ؽ�Ʈ
    public Text goldPerSecondText; //�ʴ� ȹ�� ��� �ؽ�Ʈ
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
        goldText.text = "���� ���" + gold.ToString("F1");
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
        goldPerSecondText.text = "�ʴ� ȹ�� ��差 : " + goldPerSec.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMyGoldUnit();
        UpdateMyGold();
    }
}
