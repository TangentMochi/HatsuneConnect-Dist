using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIMenu : MonoBehaviour
{
    [SerializeField] private GameObject modeSelectObject;
    [SerializeField] private GameObject levelSelectObject;
    [SerializeField] private GameObject roomSelectObject;
    [SerializeField] private GameObject characterSelectObject;
    [SerializeField] private GameObject stageSelectObject;
    [SerializeField] private GameObject waitRoomObject;
    [SerializeField] private GameObject textAliveObject;
    [SerializeField] private GameObject endButton;

    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private Button SoloButton;

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

    [SerializeField] private Button Num0Button;
    [SerializeField] private Button Num1Button;
    [SerializeField] private Button Num2Button;
    [SerializeField] private Button Num3Button;
    [SerializeField] private Button Num4Button;
    [SerializeField] private Button Num5Button;
    [SerializeField] private Button Num6Button;
    [SerializeField] private Button Num7Button;
    [SerializeField] private Button Num8Button;
    [SerializeField] private Button Num9Button;
    [SerializeField] private TMP_Text NumOUT;

    [SerializeField] private Button NumCButton;
    [SerializeField] private Button NumEButton;

    [SerializeField] private Button MikuButton;
    [SerializeField] private Button LenButton;
    [SerializeField] private Button RinButton;
    [SerializeField] private Button LukaButton;
    [SerializeField] private Button MeikoButton;
    [SerializeField] private Button KaitoButton;

    [SerializeField] private GameObject skyway_text;

    private bool is_host = false;
    private bool is_mult = false;
    private Level level;
    private Stage stage;
    private Character character;
    private string room_id = "";
    private AudioClip selectClip;

    void Start()
    {
        modeSelectObject.SetActive(true);
        levelSelectObject.SetActive(false);
        roomSelectObject.SetActive(false);
        characterSelectObject.SetActive(false);
        stageSelectObject.SetActive(false);
        waitRoomObject.SetActive(false);
        endButton.SetActive(false);
        textAliveObject.SetActive(false);
        room_id = "";
        is_host = false;
        is_mult = false;
        selectClip = (AudioClip)Resources.Load("SE/SE4");

        Skyway.Instance.ErrorHandler.AddListener(SkywayError);
        Skyway.Instance.DataEventHandler.AddListener(SkywayData);
        Skyway.Instance.ReadyHandler.AddListener(NextScene);

        TextAlive.Instance.ErrorHandler.AddListener(TextAliveError);
        TextAlive.Instance.TimerReadyHandler.AddListener(TimerReady);

        HostButton.onClick.AddListener(() => {
            is_host = true;
            is_mult = true;
            endButton.SetActive(true);
            HostSoloMode();
            SelectSound();
        });
        ClientButton.onClick.AddListener(() =>
        {
            is_host = false;
            is_mult = true;
            modeSelectObject.SetActive(false);
            roomSelectObject.SetActive(true);
            SelectSound();
        });
        SoloButton.onClick.AddListener(() =>
        {
            is_host = true;
            is_mult = false;
            HostSoloMode();
            SelectSound();
        });

        EasyButton.onClick.AddListener(() =>
        {
            level = Level.Easy;
            LevelSet();
            SelectSound();
        });
        NormalButton.onClick.AddListener(() =>
        {
            level = Level.Normal;
            LevelSet();
            SelectSound();
        });
        HardButton.onClick.AddListener(() =>
        {
            level = Level.Hard;
            LevelSet();
            SelectSound();
        });

        StageAButton.onClick.AddListener(() => {
            stage = Stage.A;
            StageSet();
            SelectSound();
        });
        StageBButton.onClick.AddListener(() =>
        {
            stage = Stage.B;
            StageSet();
            SelectSound();
        });
        StageCButton.onClick.AddListener(() =>
        {
            stage = Stage.C;
            StageSet();
            SelectSound();
        });
        StageDButton.onClick.AddListener(() =>
        {
            stage = Stage.D;
            StageSet();
            SelectSound();
        });
        StageEButton.onClick.AddListener(() =>
        {
            stage = Stage.E;
            StageSet();
            SelectSound();
        });
        StageFButton.onClick.AddListener(() =>
        {
            stage = Stage.F;
            StageSet();
            SelectSound();
        });
        StageGButton.onClick.AddListener(() =>
        {
            stage = Stage.G;
            StageSet();
            SelectSound();
        });

        Num0Button.onClick.AddListener(() =>
        {
            room_id += "0";
            DisplayRoomID();
            SelectSound();
        });
        Num1Button.onClick.AddListener(() =>
        {
            room_id += "1";
            DisplayRoomID();
            SelectSound();
        });
        Num2Button.onClick.AddListener(() =>
        {
            room_id += "2";
            DisplayRoomID();
            SelectSound();
        });
        Num3Button.onClick.AddListener(() =>
        {
            room_id += "3";
            DisplayRoomID();
            SelectSound();
        });
        Num4Button.onClick.AddListener(() =>
        {
            room_id += "4";
            DisplayRoomID();
            SelectSound();
        });
        Num5Button.onClick.AddListener(() =>
        {
            room_id += "5";
            DisplayRoomID();
            SelectSound();
        });
        Num6Button.onClick.AddListener(() =>
        {
            room_id += "6";
            DisplayRoomID();
            SelectSound();
        });
        Num7Button.onClick.AddListener(() =>
        {
            room_id += "7";
            DisplayRoomID();
            SelectSound();
        });
        Num8Button.onClick.AddListener(() =>
        {
            room_id += "8";
            DisplayRoomID();
            SelectSound();
        });
        Num9Button.onClick.AddListener(() =>
        {
            room_id += "9";
            DisplayRoomID();
            SelectSound();
        });
        NumCButton.onClick.AddListener(() =>
        {
            room_id = "";
            DisplayRoomID();
            SelectSound();
        });
        NumEButton.onClick.AddListener(() => {
            roomSelectObject.SetActive(false);
            waitRoomObject.SetActive(true);
            Skyway.Instance.joinClient(room_id);
            SelectSound();
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
            SelectSound();
        });
        LenButton.onClick.AddListener(() =>
        {
            character = Character.Len;
            CharacterData();
            SelectSound();
        });
        RinButton.onClick.AddListener(() =>
        {
            character = Character.Rin;
            CharacterData();
            SelectSound();
        });
        LukaButton.onClick.AddListener(() =>
        {
            character = Character.Luka;
            CharacterData();
            SelectSound();
        });
        MeikoButton.onClick.AddListener(() =>
        {
            character = Character.Meiko;
            CharacterData();
            SelectSound();
        });
        KaitoButton.onClick.AddListener(() =>
        {
            character = Character.Kaito;
            CharacterData();
            SelectSound();
        });

        endButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Skyway.Instance.end_reception();

            waitRoomObject.SetActive(false);
            SetSong();
            SelectSound();
        });

        if(!Skyway.skyway())
        {
            HostButton.interactable = false;
            ClientButton.interactable = false;
            skyway_text.SetActive(true);
        } else
        {
            skyway_text.SetActive(false);
        }
    }

    private void SelectSound()
    {
#if !UNITY_EDITOR
        var source = GetComponent<AudioSource>();
        source.volume = TextAlive.volume();
        source.PlayOneShot(selectClip);
#endif
    }

    private void SkywayError(string res)
    {
        Debug.LogError("SkywayError:" + res);
        ClearEvent();
        Start();
    }

    private void TextAliveError()
    {
        ClearEvent();
        Start();
    }

    private void ClearEvent()
    {
        HostButton.onClick.RemoveAllListeners();
        ClientButton.onClick.RemoveAllListeners();
        SoloButton.onClick.RemoveAllListeners();

        EasyButton.onClick.RemoveAllListeners();
        NormalButton.onClick.RemoveAllListeners();
        HardButton.onClick.RemoveAllListeners();

        StageAButton.onClick.RemoveAllListeners();
        StageBButton.onClick.RemoveAllListeners();
        StageCButton.onClick.RemoveAllListeners();
        StageDButton.onClick.RemoveAllListeners();
        StageEButton.onClick.RemoveAllListeners();
        StageFButton.onClick.RemoveAllListeners();
        StageGButton.onClick.RemoveAllListeners();

        Num0Button.onClick.RemoveAllListeners();
        Num1Button.onClick.RemoveAllListeners();
        Num2Button.onClick.RemoveAllListeners();
        Num3Button.onClick.RemoveAllListeners();
        Num4Button.onClick.RemoveAllListeners();
        Num5Button.onClick.RemoveAllListeners();
        Num6Button.onClick.RemoveAllListeners();
        Num7Button.onClick.RemoveAllListeners();
        Num8Button.onClick.RemoveAllListeners();
        Num9Button.onClick.RemoveAllListeners();
        NumCButton.onClick.RemoveAllListeners();
        NumEButton.onClick.RemoveAllListeners();

        MikuButton.onClick.RemoveAllListeners();
        LenButton.onClick.RemoveAllListeners();
        RinButton.onClick.RemoveAllListeners();
        LukaButton.onClick.RemoveAllListeners();
        MeikoButton.onClick.RemoveAllListeners();
        KaitoButton.onClick.RemoveAllListeners();
    }

    private void SkywayData(string header, string body, string fromid)
    {
        if (!is_mult)
        {
            return;
        }
        switch (header)
        {
            case "stage":
                var stage_s = (Stage)int.Parse(body);
                SetSong(stage_s);
                break;
            case "room":
                if (body == "rend" && !is_host)
                {
                    waitRoomObject.SetActive(false);

                } else if (body == "start" && !is_host)
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

    private void TimerReady()
    {
        textAliveObject.SetActive(false);
        characterSelectObject.SetActive(true);
    }

    private void HostSoloMode()
    {
        modeSelectObject.SetActive(false);
        levelSelectObject.SetActive(true);
    }

    private void LevelSet()
    {
        levelSelectObject.SetActive(false);
        stageSelectObject.SetActive(true);
    }

    private void StageSet()
    {
        stageSelectObject.SetActive(false);
        if (is_mult)
        {
            Skyway.Instance.joinHost(level, stage);
            waitRoomObject.SetActive(true);
        } else
        {
            SetSong(stage);
        }
    }

    private void DisplayRoomID()
    {
        NumOUT.text = room_id;
    }

    private void CharacterData()
    {
        if (is_mult)
        {
            MikuButton.interactable = false;
            LenButton.interactable = false;
            RinButton.interactable = false;
            LukaButton.interactable = false;
            MeikoButton.interactable = false;
            KaitoButton.interactable = false;
            Skyway.Instance.pickCharacter(character);
        } else
        {
            Skyway.Instance.joinSolo(level, stage, character);
            NextScene();
        }
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
        //SceneManager.LoadScene("StageScene");
    }
}
