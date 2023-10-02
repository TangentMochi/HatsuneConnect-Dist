using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleMusic : SingletonMonoBehaviour<TitleMusic>
{
    [SerializeField] private AudioSource audioSource;

    void Update()
    {
#if UNITY_EDITOR
        return;
#endif

        if (SceneManager.GetActiveScene().name == "ModeMemu" || SceneManager.GetActiveScene().name == "Result" || SceneManager.GetActiveScene().name == "TitleMenu" || SceneManager.GetActiveScene().name == "SettingMenu")
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                audioSource.DOFade(TextAlive.volume(), 1f);
            } else
            {
                audioSource.volume = TextAlive.volume();
            }
        } else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.DOFade(0f, 1f);
            }
        }
    }

    public void FadeIn()
    {
        audioSource.DOFade(TextAlive.volume(), 1f);
    }

    public void FadeOut()
    {
        audioSource.DOFade(0f, 1f);
    }
}
