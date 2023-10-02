using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float EasyRemainLife;
    [SerializeField] protected float NormalRemainLife;
    [SerializeField] protected float HardRemainLife;
    [SerializeField] protected float FireInterval = 5;
    [SerializeField] protected float BulletScale;
    [SerializeField] protected float BulletSpeed;
    [SerializeField] protected int BulletDamage;

    [SerializeField] protected int Point;
    protected int EnemyID;

    protected int frame_count = 0;
    protected AudioSource audioSource;

    /*
    private int beat_index = -1;
    private string text_char;
    private string text_phrase;
    private float deg;
    private float now_deg;
    */

    private float RemainLife;

    protected bool is_dead = false;
    protected float last_fire = 0;
    protected static GameObject BulletPrefab;
    protected static GameObject LaserPrefab;
    protected Vector3 init_pos;

    public StageMaster master;

    private AudioClip destorySound;
    private AudioClip fireSound;
    

    public void EnemyInit(int EnemyID, Vector3 place)
    {
        this.EnemyID = EnemyID;
        init_pos = place;
        switch (Skyway.Instance.Level)
        {
            case Level.Easy:
                RemainLife = EasyRemainLife;
                break;
            case Level.Normal:
                RemainLife = NormalRemainLife;
                break;
            case Level.Hard:
                RemainLife = HardRemainLife;
                break;
            default:
                RemainLife = NormalRemainLife;
                break;
        }
        transform.DOMove(place, 0.5f).OnComplete(() =>
        {
            EnemyMove();
        });
    }


    protected virtual void EnemyMove()
    { 
        transform.DOMove(new Vector3(transform.position.x, Mathf.Min(init_pos.y + 5f, 4.5f), transform.position.z), 1f).OnComplete(() =>
        {
            transform.DOMove(new Vector3(transform.position.x, Mathf.Max(init_pos.y - 4f, -4f), transform.position.z), 1f).OnComplete(() =>
            {
                EnemyMove();
            });
        });
    }

    /*
    if (FireInterval < last_fire)
    {
        last_fire = 0;
        //Fire();
        Laser();
    }
    last_fire += Time.deltaTime;
    if (TextAlive.findPhrase() != "" && text_phrase != TextAlive.findPhrase())
    {
        text_phrase = TextAlive.findPhrase();
        deg = 90f / TextAlive.findPhraseCount();
        now_deg = 45f;
    }
    if (TextAlive.findChar() != "" && text_char != TextAlive.findChar())
    {
        text_char = TextAlive.findChar();
        Fire(now_deg);
        now_deg -= deg;
    }
    */

    private void Awake()
    {
        BulletPrefab = (GameObject)Resources.Load("Bullet");
        LaserPrefab = (GameObject)Resources.Load("Laser");
        destorySound = (AudioClip)Resources.Load("SE/SE1");
        fireSound = (AudioClip)Resources.Load("SE/SE2");
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(destorySound);
    }
    /*
        protected void FixedUpdate()
        {
            frame_count++;
            last_fire += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Sin(Mathf.PI * (frame_count % 360) / 180) * 5, transform.position.z);


            if (FireInterval < last_fire)
            {
                last_fire = 0;
                //Fire();
                Laser();
            }

    #if !UNITY_EDITOR

            if (-1 < TextAlive.findBeat() && beat_index != TextAlive.findBeat())
            {
                beat_index = TextAlive.findBeat();
                Fire();
            }
            */

    /*
    if (TextAlive.findChar() != "" && text_char != TextAlive.findChar())
    {
        text_char = TextAlive.findChar();
        Fire();
    }
    */
    /*
    if (TextAlive.findPhrase() != "" && text_phrase != TextAlive.findPhrase())
    {
        text_phrase = TextAlive.findPhrase();
        deg = 90f / TextAlive.findPhraseCount();
        now_deg = 45f;
    }
    if (TextAlive.findChar() != "" && text_char != TextAlive.findChar())
    {
        text_char = TextAlive.findChar();
        Fire(now_deg);
        now_deg -= deg;
    }

#endif
}
*/
    protected void Fire()
    {
        Fire(0);
    }

    protected void Fire(float rotate)
    {
        audioSource.volume = 0.25f * TextAlive.volume();
        audioSource.PlayOneShot(fireSound);
        int damage = 0;
        switch (Skyway.Instance.Level)
        {
            case Level.Easy:
                damage = BulletDamage;
                break;
            case Level.Normal:
                damage = Mathf.FloorToInt(BulletDamage * 1.5f);
                break;
            case Level.Hard:
                damage = BulletDamage * 2;
                break;
        }
        var bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        var bullet_bullet = bullet.GetComponent<Bullet>();
        bullet_bullet.Init(BulletScale * StageMaster.DevBulletSize, BulletSpeed * StageMaster.DevBulletSpeed, damage);
        bullet_bullet.Rotate(rotate);
        bullet_bullet.is_ally = false;
        bullet_bullet.is_enemy = true;
    }

    protected void Laser()
    {
        Laser(0);
    }

    protected void Laser(float rotate)
    {
        audioSource.volume = 0.25f * TextAlive.volume();
        audioSource.PlayOneShot(fireSound);
        var bullet = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
        var bullet_bullet = bullet.GetComponent<Bullet>();
        bullet_bullet.Init(BulletScale * StageMaster.DevBulletSize, BulletSpeed * StageMaster.DevBulletSpeed, BulletDamage);
        bullet_bullet.Rotate(rotate);
        bullet_bullet.is_ally = false;
        bullet_bullet.is_enemy = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>())
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet.is_ally)
            {
                Skyway.Instance.writeStream("damage", EnemyID.ToString() + ";" + bullet.GetDamage(), false);
                RemainLife -= bullet.GetDamage();
                if (RemainLife <= 0 && !is_dead)
                {
                    is_dead = true;
                    Skyway.Instance.writeStream("destroy", EnemyID.ToString(), true);
                    Skyway.Instance.writeStream("point", Point.ToString(), true);
                }
                Destroy(collision.gameObject);
            }
        }
    }

    public void Damage(int damage)
    {
        RemainLife -= damage;
    }
}