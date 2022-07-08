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
    /// 0 = 타이틀, 1 = 인게임, 2 = 컷씬
    /// </summary>
    /// <param name="index"></param>
    public void PlayBGM(int index)
    {
        audioSource_BGM.Pause();
        audioSource_BGM.clip = array_BGM[index];
        audioSource_BGM.Play();
    }
    /// <summary>
    /// 0 = 버튼, 1 = 캔슬, 2 = 탭, 3= 구매, 4= 오픈
    /// </summary>
    /// <param name="index"></param>
    public void PlaySE_UI(int index)
    {
        audioSource_SE_UI.clip = array_SE_UI[index];
        audioSource_SE_UI.Play();
    }
    /// <summary>
    /// 0 = 플레이어 공격, 1 = 플레이어 피격 
    /// </summary>
    /// <param name="index"></param>
    public void PlaySE_InGame(int index)
    {
        audioSource_SE_InGame.clip = array_SE_InGame[index];
        audioSource_SE_InGame.Play();
    }
}
