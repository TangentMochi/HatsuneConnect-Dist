using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageMaster : MonoBehaviour
{
    public static float AddBulletSize;
    public static float AddBulletSpeed;
    public static float AddBulletInterval;

    public static float DevBulletSize = 1f;
    public static float DevBulletSpeed = 1f;
    public static float DevBulletInterval = 1f;

    [SerializeField] private float BufferBulletSize = 0.05f;
    [SerializeField] private float BufferBulletSpeed = 0.05f;
    [SerializeField] private float BufferBulletInterval = 0.25f;

    [SerializeField] private float AntiBullerSize = 0.7f;
    [SerializeField] private float AntiBulletSpeed = 0.8f;
    [SerializeField] private float AntiBulletInterval = 0.9f;


    [SerializeField] private TMP_Text score_text;
    [SerializeField] private GameObject end_obj;
    [SerializeField] private SkillUI skillUI;

    //[SerializeField] private GameObject PlayerPrefab;
    //[SerializeField] private GameObject EmemyPrefab;

    private GameObject Player;
    private Dictionary<int, GameObject> EnemyList = new Dictionary<int, GameObject>();
    private Dictionary<string, GameObject> AllyList = new Dictionary<string, GameObject>();
    private Dictionary<string, Character> CharList = new Dictionary<string, Character>();
    private int EnemyID = 0;
    private bool is_host = false;
    public static bool is_end = false;

    public static int Point = 0;

    private int now_enemy = 0;
    private int now_wave = 1;
    private List<EnemyType> enemies = new List<EnemyType>();
    private bool is_boss = false;


    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR
        TextAlive.play();
#endif
        Point = 0;
        EnemyID = 0;
        EnemyList = new Dictionary<int, GameObject>();
        AllyList = new Dictionary<string, GameObject>();
        CharList = new Dictionary<string, Character>();
        is_host = Skyway.Instance.Host;
        is_end = false;
        DevBulletSize = 1f;
        DevBulletSpeed = 1f;
        DevBulletInterval = 1f;
        now_enemy = 0;
        is_boss = false;

        Skyway.Instance.DataEventHandler.AddListener(OnData);
        TextAlive.Instance.FinalChorus.AddListener(() =>
        {
            if (!is_boss)
            {
                is_boss = true;
                EnemyGenerate(new Vector3(9, Random.Range(-5f, 5f), 0), EnemyType.EnemyBoss);
            }
        });

        end_obj.SetActive(false);
        PlayerGenerate(new Vector3(-9, -5, 0), Skyway.Instance.my_ins.character);
        for (int i=0; i < Skyway.Instance.Members.Count;i++)
        {
            CharList[Skyway.Instance.Members[i].id] = Skyway.Instance.Members[i].character;
            AllyList[Skyway.Instance.Members[i].id] = PlayerDummyGenerate(new Vector3(-9, 0, 0), Skyway.Instance.Members[i].character);
        }

        switch (Skyway.Instance.Level)
        {
            case Level.Hard:
                now_wave = 40;
                enemies.Add(EnemyType.EnemyA);
                enemies.Add(EnemyType.EnemyB);
                enemies.Add(EnemyType.EnemyC);
                enemies.Add(EnemyType.EnemyD);
                enemies.Add(EnemyType.EnemyE);
                break;
            case Level.Normal:
                now_wave = 20;
                enemies.Add(EnemyType.EnemyA);
                enemies.Add(EnemyType.EnemyB);
                enemies.Add(EnemyType.EnemyC);
                break;
            case Level.Easy:
                now_wave = 0;
                enemies.Add(EnemyType.EnemyA);
                enemies.Add(EnemyType.EnemyB);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (is_host)
        {
#if !UNITY_EDITOR
            if (TextAlive.Instance.SongleSong.duration < TextAlive.position() + 100)
            {
                Debug.Log("END");
                Skyway.Instance.writeStream("room", "end", true);
            }       
#else
            // TODO DEBUG
            if(Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(EndScene());
            }
#endif      
            // TODO DEBUG
            /*
            if(Input.GetKeyDown(KeyCode.F)){
                now_enemy++;
                EnemyGenerate(new Vector3(9, Random.Range(-5f, 5f), 0), EnemyType.EnemyB);
            }
            */
            // TODO 敵生成プログラム
            /*
            var to_enemy = 3;
            switch (Skyway.Instance.Level)
            {
                case Level.Hard:
                    to_enemy = 5;
                    break;
                case Level.Normal:
                    to_enemy = 4;
                    break;
                case Level.Easy:
                    to_enemy = 3;
                    break;
            }
            */
            
            if (now_enemy == 0)
            {
                now_wave++;
                int to_enemy = Mathf.CeilToInt(now_wave / 20) + 2;
                for (int i = 0; i < to_enemy; i++)
                {
                    if ((now_wave % 20) / 20f < Random.Range(0f,1f))
                    {
                        EnemyGenerate(new Vector3(Random.Range(5f, 9f), Random.Range(-5f, 5f), 0), enemies[Random.Range(0, enemies.Count)]);
                    } else
                    {
                        //中ボス
                        EnemyGenerate(new Vector3(9, Random.Range(-5f, 5f), 0), (EnemyType)Random.Range(6, 11));
                    }
                    
                }
            }

            /*
            if ((!is_boss) && TextAlive.finalChorus())
            {
                is_boss = true;
                EnemyGenerate(new Vector3(9, Random.Range(-5f, 5f), 0), EnemyType.EnemyBoss);
            }
            */
        }
    }

    private void OnData(string header, string body, string id)
    {
        string[] data = body.Split(";");
        switch(header)
        {
            case "room":
                if (!is_end)
                {
                    StartCoroutine(EndScene());
                    is_end = true;
                }
                break;
            case "damage":

                if (EnemyList[int.Parse(data[0])]) {
                    var enemy = EnemyList[int.Parse(data[0])].GetComponent<BaseEnemy>();
                    enemy.Damage(int.Parse(data[1]));
                }
                break;
            case "enemy":
                // EnemyID EnemyType X Y Z
                EnemyGenerate(int.Parse(data[0]), new Vector3(float.Parse(data[2]), float.Parse(data[3]), float.Parse(data[4])), (EnemyType)int.Parse(data[1]));
                break;
            case "destroy":
                if(EnemyList[int.Parse(body)]?.gameObject)
                {
                    now_enemy--;
                    Destroy(EnemyList[int.Parse(body)]);
                }
                break;
            case "point":
                Point += int.Parse(body);
                score_text.text = Point.ToString();
                break;
            case "goto":
                if (Skyway.Instance.Mult) AllyList[id]?.GetComponent<BasePlayer>().GoToDummy(float.Parse(body));
                break;
            case "fire":
                if (Skyway.Instance.Mult) AllyList[id]?.GetComponent<BasePlayer>().FireDummy();
                break;
            case "shield": // ルカスキル
                Player.GetComponent<BasePlayer>().Guard();
                break;
            case "turret":
                Destroy(FindObjectOfType<Turret>()?.gameObject);
                break;
            case "anti": // カイトスキル
                StartCoroutine(Anti());
                break;
            case "effect": // メイコスキル
                StartCoroutine(Effect());
                break;
            case "skill":
                if (Skyway.Instance.Mult && id != "me")
                {
                    EventUI(CharList[id], "スキル発動");
                    AllyList[id]?.GetComponent<BasePlayer>().SkillDummy();
                }
                break;
        }
    }

    private IEnumerator Effect()
    {
        AddBulletInterval = BufferBulletInterval;
        AddBulletSize = BufferBulletSize;
        AddBulletSpeed = BufferBulletSpeed;
        yield return new WaitForSeconds(30);
        AddBulletInterval = 0;
        AddBulletSize = 0;
        AddBulletSpeed = 0f;
    }

    private IEnumerator Anti()
    {
        DevBulletInterval = AntiBulletInterval;
        DevBulletSize = AntiBullerSize;
        DevBulletSpeed = AntiBulletSpeed;
        yield return new WaitForSeconds(30);
        DevBulletInterval = 1.0f;
        DevBulletSize = 1.0f;
        DevBulletSpeed = 1.0f;
    }

    private IEnumerator EndScene()
    {
        yield return new WaitForSeconds(1);
        foreach (KeyValuePair<int, GameObject> kvp in EnemyList)
        {
            Destroy(kvp.Value);
        }
        end_obj.SetActive(true);
#if !UNITY_EDITOR
        TextAlive.stop();
#endif
        yield return new WaitForSeconds(1);
        FadeSence.Instance.LoadSence("Result");
    }

    public void EventUI(Character character, string meg)
    {
        skillUI.Event(character, meg);
    }

    private void PlayerGenerate(Vector3 place, Character character)
    {
        GameObject player_tmp;
        switch (character)
        {
            case Character.Miku:
                player_tmp = (GameObject)Resources.Load("Miku");
                break;
            case Character.Rin:
                player_tmp = (GameObject)Resources.Load("Rin");
                break;
            case Character.Len:
                player_tmp = (GameObject)Resources.Load("Len");
                break;
            case Character.Luka:
                player_tmp = (GameObject)Resources.Load("Luka");
                break;
            case Character.Meiko:
                player_tmp = (GameObject)Resources.Load("Meiko");
                break;
            case Character.Kaito:
                player_tmp = (GameObject)Resources.Load("Kaito");
                break;
            default:
                Debug.LogWarning("Wrong Char ID");
                player_tmp = (GameObject)Resources.Load("Miku");
                break;
        }
        if (!Player)
        {
            Player = Instantiate(player_tmp, place, Quaternion.identity);
            Player.GetComponent<BasePlayer>().master = this;
        }
    }

    private GameObject PlayerDummyGenerate(Vector3 place, Character character)
    {
        GameObject player_tmp;
        switch (character)
        {
            case Character.Miku:
                player_tmp = (GameObject)Resources.Load("Miku");
                break;
            case Character.Rin:
                player_tmp = (GameObject)Resources.Load("Rin");
                break;
            case Character.Len:
                player_tmp = (GameObject)Resources.Load("Len");
                break;
            case Character.Luka:
                player_tmp = (GameObject)Resources.Load("Luka");
                break;
            case Character.Meiko:
                player_tmp = (GameObject)Resources.Load("Meiko");
                break;
            case Character.Kaito:
                player_tmp = (GameObject)Resources.Load("Kaito");
                break;
            default:
                Debug.LogWarning("Wrong Char ID");
                player_tmp = (GameObject)Resources.Load("Miku");
                break;
        }
        player_tmp = Instantiate(player_tmp, place, Quaternion.identity);
        player_tmp.GetComponent<BasePlayer>().is_dummy = true;
        return player_tmp;
    }

    private void EnemyGenerate(Vector3 place, EnemyType enemyType)
    {
        now_enemy++;
        //Debug.Log("enemy - " + EnemyID.ToString() + ";" + (int)enemyType + ";" + place.x.ToString() + ";" + place.y.ToString() + ";" + place.z.ToString());
        Skyway.Instance.writeStream("enemy", EnemyID.ToString() + ";" + (int)enemyType + ";" + place.x.ToString() + ";" + place.y.ToString() + ";" + place.z.ToString(), true);
        EnemyID += 1;
    }

    private void EnemyGenerate(int enemy, Vector3 place, EnemyType enemyType)
    {
        GameObject enemy_tmp;
        // TODO 敵の種類追加
        switch (enemyType)
        {
            case EnemyType.EnemyA:
                enemy_tmp = (GameObject)Resources.Load("EnemyA");
                break;
            case EnemyType.EnemyB:
                enemy_tmp = (GameObject)Resources.Load("EnemyB");
                break;
            case EnemyType.EnemyC:
                enemy_tmp = (GameObject)Resources.Load("EnemyC");
                break;
            case EnemyType.EnemyD:
                enemy_tmp = (GameObject)Resources.Load("EnemyD");
                break;
            case EnemyType.EnemyE:
                enemy_tmp = (GameObject)Resources.Load("EnemyE");
                break;
            case EnemyType.EnemyBoss:
                enemy_tmp = (GameObject)Resources.Load("Boss");
                break;
            case EnemyType.EnemySA:
                enemy_tmp = (GameObject)Resources.Load("EnemySA");
                break;
            case EnemyType.EnemySB:
                enemy_tmp = (GameObject)Resources.Load("EnemySB");
                break;
            case EnemyType.EnemySC:
                enemy_tmp = (GameObject)Resources.Load("EnemySC");
                break;
            case EnemyType.EnemySD:
                enemy_tmp = (GameObject)Resources.Load("EnemySD");
                break;
            case EnemyType.EnemySE:
                enemy_tmp = (GameObject)Resources.Load("EnemySE");
                break;
            default:
                enemy_tmp = (GameObject)Resources.Load("Enemy");
                break;
        }

        var enemy_ins = Instantiate(enemy_tmp, new Vector3(15f,0,0), Quaternion.identity);
        enemy_ins.GetComponent<BaseEnemy>().EnemyInit(enemy, place);
        enemy_ins.GetComponent<BaseEnemy>().master = this;
        EnemyList[enemy] = enemy_ins;
    }
}
