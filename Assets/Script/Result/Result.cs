using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Bar bar;
    [SerializeField] private Score score;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button tweetButton;
    [SerializeField] private GameObject host;
    [SerializeField] private GameObject client;


    void Start()
    {
        var point = StageMaster.Point;
        score.Result(point);

        bar.Result(point / 5000f);

        if (Skyway.Instance.Host)
        {
            host.SetActive(true);
            client.SetActive(false);
        } else
        {
            host.SetActive(false);
            client.SetActive(true);
        }

        startButton.onClick.AddListener(() =>
        {
            Skyway.Instance.writeStream("room", "start", true);
        });

        settingButton.onClick.AddListener(() =>
        {
            Skyway.Instance.ResetRoom();
            FadeSence.Instance.LoadSence("SettingMenu");
        });

        closeButton.onClick.AddListener(() =>
        {
            if (Skyway.Instance.Mult)
            {
                Skyway.Instance.CloseRoom();
                FadeSence.Instance.LoadSence("TitleMenu");
            } else
            {
                Skyway.Instance.CloseRoom();
                FadeSence.Instance.LoadSence("TitleMenu");
            }
        });

        tweetButton.onClick.AddListener(() =>
        {
            var url = TextAlive.getUrl();
            TextAlive.urlOpen($"https://twitter.com/share?text=初音コネクトで%0a{point}点でした。%0a&url={url}&hashtags=mm2023procon");
        });

        Skyway.Instance.DataEventHandler.AddListener(OnData);

        Skyway.Instance.CloseHandler.AddListener(() =>
        {
            Skyway.Instance.CloseRoom();
            FadeSence.Instance.LoadSence("TitleMenu");
        });
    }

    void OnData(string header, string body, string fromid)
    {
        switch(header)
        {
            case "room":
                if (body == "start")
                {
                    TitleMusic.Instance.FadeOut();
                    FadeSence.Instance.LoadSence("StageScene");
                }
                if (body == "rend")
                {
                    Skyway.Instance.ResetRoom();
                    FadeSence.Instance.LoadSence("SettingMenu");
                }
                break;
        }
    }
}
