using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class TextAlive : SingletonMonoBehaviour<TextAlive>
{
    [DllImport("__Internal")]
    public static extern void initPlayer();
    [DllImport("__Internal")]
    public static extern void createSongConfig(string songUrl, int beatId, int chordId, int repetitiveSegmentId, int lyricId, int lyricDiffId);
    [DllImport("__Internal")]
    public static extern void createSong(string songUrl);
    [DllImport("__Internal")]
    public static extern void play();
    [DllImport("__Internal")]
    public static extern void stop();

    [DllImport("__Internal")]
    public static extern float position();
    [DllImport("__Internal")]
    public static extern float volume();

    [DllImport("__Internal")]
    public static extern string findWord();
    [DllImport("__Internal")]
    public static extern string findChar();
    [DllImport("__Internal")]
    public static extern string findPhrase();
    [DllImport("__Internal")]
    public static extern int findBeat();
    [DllImport("__Internal")]
    public static extern int findPhraseCount();
    [DllImport("__Internal")]
    public static extern string findChord();
    [DllImport("__Internal")]
    public static extern int findChorus();
    [DllImport("__Internal")]
    public static extern void urlOpen(string url);
    [DllImport("__Internal")]
    public static extern string getUrl();
    [DllImport("__Internal")]
    public static extern bool finalChorus();


    public UnityEvent AppReadyHandler;
    public UnityEvent TimerReadyHandler;
    public UnityEvent ErrorHandler;
    public UnityEvent FinalChorus;

    public SongleSong SongleSong = new();

    protected override void Awake()
    {
        base.Awake();
#if !UNITY_EDITOR
        initPlayer();
#endif
    }

    private IEnumerator GetSongData(Song song)
    {
        UnityWebRequest request = UnityWebRequest.Get($"https://widget.songle.jp/api/v1/song.json?url={song.songUrl}");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            ErrorHandler?.Invoke();
        } else
        {
            if (request.responseCode == 200)
            {
                string jsonText = request.downloadHandler.text;
                jsonText = System.Text.RegularExpressions.Regex.Unescape(jsonText);
                SongleSong = JsonUtility.FromJson<SongleSong>(jsonText);
            }
            else
            {
                ErrorHandler?.Invoke();
            }
        }
    }

    public void OnReady()
    {
        AppReadyHandler?.Invoke();
    }

    public void OnPlayReady()
    {
        TimerReadyHandler?.Invoke();
    }

    public void OnStop()
    {

    }

    public void OnPause()
    {
        if (SceneManager.GetActiveScene().name == "StageScene" && !StageMaster.is_end)
        {
            StartCoroutine(RequestPlay());
        }
    }

    IEnumerator RequestPlay()
    {
        yield return new WaitForSeconds(1f);
        play();
    }

    public void OnFinalChorus()
    {
        FinalChorus?.Invoke();
    }

    public void CreateSong(Song song)
    {
        StartCoroutine(GetSongData(song));
#if UNITY_EDITOR
        OnPlayReady();
#else
        createSongConfig(song.songUrl, song.beatId, song.chordId, song.repetitiveSegmentId, song.lyricId, song.lyricDiffId);
#endif
    }

    public static Song A = new("https://piapro.jp/t/ucgN/20230110005414", 4267297, 2405019, 2475577, 56092, 9636);
    public static Song B = new("https://piapro.jp/t/fnhJ/20230131212038", 4267300, 2405033, 2475606, 56131, 9638);
    public static Song C = new("https://piapro.jp/t/Vfrl/20230120182855", 4267334, 2405059, 2475645, 56095, 9637);
    public static Song D = new("https://piapro.jp/t/fyxI/20230203003935", 4267373, 2405138, 2475664, 56096, 9639);
    public static Song E = new("https://piapro.jp/t/Wk83/20230203141007", 4267381, 2405285, 2475676, 56812, 10668);
    public static Song F = new("https://piapro.jp/t/Ya0_/20230201235034", 4269734, 2405723, 2475686, 56098, 9643);

    public void RemoveAllEvents()
    {
        AppReadyHandler.RemoveAllListeners();
        TimerReadyHandler.RemoveAllListeners();
        ErrorHandler.RemoveAllListeners();
}
}
