using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FadeSence : SingletonMonoBehaviour<FadeSence>
{
    [SerializeField] GameObject Panel;

    private void Start()
    {
        Panel.SetActive(false);
        
    }

    public void LoadSence(string name)
    {
        Skyway.Instance.RemoveAllEvents();
        TextAlive.Instance.RemoveAllEvents();
        var tween = FadeIn();
        tween.OnComplete(() =>
        {
            SceneManager.LoadSceneAsync(name).completed += (load) =>
            {
                FadeOut();
            };
        });
    }

    private Tween FadeIn()
    {
        Panel.SetActive(true);
        var panel = Panel.GetComponent<Image>();
        return panel.DOFade(1f, 1f);
    }

    private Tween FadeOut()
    {
        var panel = Panel.GetComponent<Image>();
        return panel.DOFade(0f, 1f).OnComplete(() =>
        {
            Panel.SetActive(false);
        });
    }
}
