using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource_BGM;
    public AudioSource audioSource_SE_UI;
    public AudioSource audioSource_SE_InGame;

    public AudioClip[] array_BGM;
    public AudioClip[] array_SE_UI;
    public AudioClip[] array_SE_InGame;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
        DataManager.LoadSoundData();
    }
    /// <summary>
    /// 0 = Ÿ��Ʋ, 1 = �ΰ���, 2 = �ƾ�
    /// </summary>
    /// <param name="index"></param>
    public void PlayBGM(int index)
    {
        audioSource_BGM.Pause();
        audioSource_BGM.clip = array_BGM[index];
        audioSource_BGM.Play();
    }
    /// <summary>
    /// 0 = ��ư, 1 = ĵ��, 2 = ��, 3= ����, 4= ����
    /// </summary>
    /// <param name="index"></param>
    public void PlaySE_UI(int index)
    {
        audioSource_SE_UI.clip = array_SE_UI[index];
        audioSource_SE_UI.Play();
    }
    /// <summary>
    /// 0 = �÷��̾� ����, 1 = �÷��̾� �ǰ� 
    /// </summary>
    /// <param name="index"></param>
    public void PlaySE_InGame(int index)
    {
        audioSource_SE_InGame.clip = array_SE_InGame[index];
        audioSource_SE_InGame.Play();
    }
}
