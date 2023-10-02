using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectObject;
    [SerializeField] private GameObject stageSelectObject;
    [SerializeField] private GameObject characterSelectObject;
    [SerializeField] private GameObject textAliveObject;

    [SerializeField] private Button EasyButton;
    [SerializeField] private Button NormalButton;
    [SerializeField] private Button HardButton;

    [SerializeField] private Button StageAButton;
    [SerializeField] private Button StageBButton;
    [SerializeField] private Button StageCButton;
    [SerializeField] private Button StageDButton;
    [SerializeField] private Button StageEButton;
    [SerializeField] private Button StageFButton;
    [SerializeField] private Button StageGButton;

    [SerializeField] private Button MikuButton;
    [SerializeField] private Button LenButton;
    [SerializeField] private Button RinButton;
    [SerializeField] private Button LukaButton;
    [SerializeField] private Button MeikoButton;
    [SerializeField] private Button KaitoButton;

    private Level level;
    private Stage stage;
    private Character character;

    // Start is called before the first frame update
    void Start()
    {
        levelSelectObject.SetActive(false);
        stageSelectObject.SetActive(false);
        characterSelectObject.SetActive(false);
        textAliveObject.SetActive(false);
        if (Skyway.Instance.Host)
        {
            levelSelectObject.SetActive(true);
        } else
        {
            textAliveObject.SetActive(true);
            SetSong();
        }

        Skyway.Instance.DataEventHandler.AddListener(SkywayData);
        Skyway.Instance.ReadyHandler.AddListener(NextScene);

        TextAlive.Instance.TimerReadyHandler.AddListener(TimerReady);

        EasyButton.onClick.AddListener(() =>
        {
            level = Level.Easy;
            LevelSet();
        });
        NormalButton.onClick.AddListener(() =>
        {
            level = Level.Normal;
            LevelSet();
        });
        HardButton.onClick.AddListener(() =>
        {
            level = Level.Hard;
            LevelSet();
        });

        StageAButton.onClick.AddListener(() => {
            stage = Stage.A;
            StageSet();
        });
        StageBButton.onClick.AddListener(() =>
        {
            stage = Stage.B;
            StageSet();
        });
        StageCButton.onClick.AddListener(() =>
        {
            stage = Stage.C;
            StageSet();
        });
        StageDButton.onClick.AddListener(() =>
        {
            stage = Stage.D;
            StageSet();
        });
        StageEButton.onClick.AddListener(() =>
        {
            stage = Stage.E;
            StageSet();
        });
        StageFButton.onClick.AddListener(() =>
        {
            stage = Stage.F;
            StageSet();
        });
        StageGButton.onClick.AddListener(() =>
        {
            stage = Stage.G;
            StageSet();
        });

        MikuButton.interactable = true;
        LenButton.interactable = true;
        RinButton.interactable = true;
        LukaButton.interactable = true;
        MeikoButton.interactable = true;
        KaitoButton.interactable = true;

        MikuButton.onClick.AddListener(() =>
        {
            character = Character.Miku;
            CharacterData();
        });
        LenButton.onClick.AddListener(() =>
        {
            character = Character.Len;
            CharacterData();
        });
        RinButton.onClick.AddListener(() =>
        {
            character = Character.Rin;
            CharacterData();
        });
        LukaButton.onClick.AddListener(() =>
        {
            character = Character.Luka;
            CharacterData();
        });
        MeikoButton.onClick.AddListener(() =>
        {
            character = Character.Meiko;
            CharacterData();
        });
        KaitoButton.onClick.AddListener(() =>
        {
            character = Character.Kaito;
            CharacterData();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SkywayData(string header, string body, string formid)
    {
        if (!Skyway.Instance.Mult)
        {
            return;
        }
        switch (header)
        {
            case "room":
                if (body == "start" && !Skyway.Instance.Host)
                {
                    NextScene();
                }
                break;
            case "char":
                Character character = (Character)int.Parse(body);
                switch (character)
                {
                    case Character.Miku:
                        MikuButton.interactable = false;
                        break;
                    case Character.Len:
                        LenButton.interactable = false;
                        break;
                    case Character.Rin:
                        RinButton.interactable = false;
                        break;
                    case Character.Luka:
                        LukaButton.interactable = false;
                        break;
                    case Character.Meiko:
                        MeikoButton.interactable = false;
                        break;
                    case Character.Kaito:
                        KaitoButton.interactable = false;
                        break;
                }
                break;
        }
    }

    void LevelSet()
    {
        levelSelectObject.SetActive(false);
        stageSelectObject.SetActive(true);
    }

    void StageSet()
    {
        stageSelectObject.SetActive(false);
        if (Skyway.Instance.Mult)
        {
            Skyway.Instance.SetStageLevel(stage, level);
            Skyway.Instance.writeStream("level", ((int)level).ToString());
            Skyway.Instance.writeStream("stage", ((int)stage).ToString());
            Skyway.Instance.writeStream("room", "rend");    
        }
        SetSong(stage);
    }

    private void SetSong()
    {
        Stage stage = Skyway.Instance.Stage;
        SetSong(stage);
    }

    private void SetSong(Stage stage)
    {
        textAliveObject.SetActive(true);
        switch (stage)
        {
            case Stage.A:
                TextAlive.Instance.CreateSong(TextAlive.A);
                break;
            case Stage.B:
                TextAlive.Instance.CreateSong(TextAlive.B);
                break;
            case Stage.C:
                TextAlive.Instance.CreateSong(TextAlive.C);
                break;
            case Stage.D:
                TextAlive.Instance.CreateSong(TextAlive.D);
                break;
            case Stage.E:
                TextAlive.Instance.CreateSong(TextAlive.E);
                break;
            case Stage.F:
                TextAlive.Instance.CreateSong(TextAlive.F);
                break;
        }
    }

    private void CharacterData()
    {
        if (Skyway.Instance.Mult)
        {
            MikuButton.interactable = false;
            LenButton.interactable = false;
            RinButton.interactable = false;
            LukaButton.interactable = false;
            MeikoButton.interactable = false;
            KaitoButton.interactable = false;
            Skyway.Instance.pickCharacter(character);
        }
        else
        {
            Skyway.Instance.joinSolo(level, stage, character);
            NextScene();
        }
    }

    private void TimerReady()
    {
        textAliveObject.SetActive(false);
        characterSelectObject.SetActive(true);
    }

    private void NextScene()
    {
        Debug.Log("stage" + Skyway.Instance.Stage);
        switch (Skyway.Instance.Stage)
        {
            case Stage.A:
                break;
            case Stage.B:
                break;
            case Stage.C:
                break;
            case Stage.D:
                break;
            case Stage.E:
                break;
            case Stage.F:
                break;
            case Stage.G:
                break;
        }
        TitleMusic.Instance.FadeOut();
        FadeSence.Instance.LoadSence("StageScene");
    }
}
