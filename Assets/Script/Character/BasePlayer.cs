using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BasePlayer : MonoBehaviour
{
    public bool is_dummy = false;

    [SerializeField] protected float CharacterSpeed = 0.2f;
    [SerializeField] protected float BulletSize = 1;
    [SerializeField] protected float BulletSpeed = 0.3f;
    [SerializeField] protected float BulletInterval = 0.2f;
    [SerializeField] protected int BulletDamage = 5;
    [SerializeField] protected float SkillInterval = 20f;
    [SerializeField] protected List<Sprite> sprites;
    [SerializeField] protected Shield shield;
    [SerializeField] protected int SkillCost = 200;
    protected AudioSource audioSource;

    protected GameObject bullet_prefab;
    private float mouse_y;
    private float last_fired;
    private float last_motion;
    private int now_motion = 0;
    private float last_skill;
    private int sync_move = 0;
    private AudioClip fireSound;

    public StageMaster master;

    // Use this for initialization
    protected void Start()
    {
        last_skill = SkillInterval;
        bullet_prefab = (GameObject)Resources.Load("Bullet");
        audioSource = GetComponent<AudioSource>();
        fireSound = (AudioClip)Resources.Load("SE/SE3");
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!is_dummy)
        {
            if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.A)) && BulletInterval - StageMaster.AddBulletInterval < last_fired)
            {
                last_fired = 0;
                var bullet_tmp = Instantiate(bullet_prefab, transform.position, Quaternion.identity);
                var bullet_ins = bullet_tmp.GetComponent<Bullet>();
                bullet_ins.Init(BulletSize + StageMaster.AddBulletSize, BulletSpeed + StageMaster.AddBulletSpeed, BulletDamage);
                bullet_ins.is_ally = true;
                bullet_ins.is_enemy = false;
                audioSource.volume = 0.25f * TextAlive.volume();
                audioSource.PlayOneShot(fireSound);
                Skyway.Instance.writeStream("fire", "");
            }
            if (Input.GetMouseButtonDown(1))
            {
                mouse_y = Input.mousePosition.y;
            }
            if (Input.GetKey(KeyCode.Space) && CheckSkill())
            {
                Skill();
            }

            last_fired += Time.deltaTime;
        } else
        {
            shield.is_dummy = true;
        }

        last_motion += Time.deltaTime;
        last_skill += Time.deltaTime;
        if (0.15f < last_motion && sprites.Count != 0)
        {
            last_motion = 0;
            now_motion++;
            if (now_motion >= sprites.Count) now_motion = 0;
            GetComponent<SpriteRenderer>().sprite = sprites[now_motion];
        }
    }

    protected void FixedUpdate()
    {
        if (!is_dummy)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    mouse_y += 5;
                } else
                {
                    mouse_y += 10;
                }
                
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    mouse_y -= 5;
                } else
                {
                    mouse_y -= 10;
                }
                    
            }

            var to_y = Camera.main.ScreenToWorldPoint(new Vector3(0, mouse_y, 0)).y;
            if (5.0f < to_y)
            {
                mouse_y = Camera.main.WorldToScreenPoint(new Vector3(0f, 5.0f, 0f)).y;
            }
            if (-4.25f > to_y)
            {
                mouse_y = Camera.main.WorldToScreenPoint(new Vector3(0f, -4.25f, 0f)).y;
            }
            var pos_y = transform.position.y;
            if (pos_y > to_y)
            {
                if (pos_y - to_y >= CharacterSpeed)
                {
                    transform.position = new Vector3(transform.position.x, pos_y - CharacterSpeed, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, to_y, transform.position.z);
                }
                if (5 < sync_move)
                {
                    sync_move = 0;
                    Skyway.Instance.writeStream("goto", transform.position.y.ToString());
                } else
                {
                    sync_move++;
                }
                
            }
            else if (pos_y < to_y)
            {
                if (to_y - pos_y >= CharacterSpeed)
                {
                    transform.position = new Vector3(transform.position.x, pos_y + CharacterSpeed, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, to_y, transform.position.z);
                }
                if (5 < sync_move)
                {
                    sync_move = 0;
                    Skyway.Instance.writeStream("goto", transform.position.y.ToString());
                } else
                {
                    sync_move++;
                }
            }
            // TODO プレイヤー位置の同期
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!is_dummy && !shield.gameObject.activeSelf)
        {
            var bullet = collision.GetComponent<Bullet>();
            if (bullet)
            {
                if (bullet.is_enemy)
                {
                    Destroy(collision.gameObject);
                    Skyway.Instance.writeStream("point", (-bullet.GetDamage()).ToString(), true);
                }
            }
        }
    }

    protected bool CheckSkill()
    {
        if (SkillCost < StageMaster.Point && SkillInterval < last_skill)
        {
            last_skill = 0f;
            StartCoroutine(SkillReady());
            Skyway.Instance.writeStream("skill", "");
            Skyway.Instance.writeStream("point", (-SkillCost).ToString(), true);
            return true;
        }
        return false;
    }

    private IEnumerator SkillReady()
    {
        yield return new WaitForSeconds(SkillInterval);
        master.EventUI(Skyway.Instance.my_ins.character, "スキル使用可");
    }

    protected abstract void Skill();

    public abstract void SkillDummy();

    public void Guard()
    {
        shield.gameObject.SetActive(true);
    }

    public void FireDummy()
    {
        var bullet_tmp = Instantiate(bullet_prefab, transform.position, Quaternion.identity);
        var bullet_ins = bullet_tmp.GetComponent<Bullet>();
        bullet_ins.Init(BulletSize + StageMaster.AddBulletSize, BulletSpeed + StageMaster.AddBulletSpeed, BulletDamage);
        bullet_ins.is_ally = false;
        bullet_ins.is_enemy = false;
    }

    public void GoToDummy(float pos_y)
    {
        if (is_dummy)
        {
            transform.position = new Vector3(transform.position.x, pos_y, transform.position.z);
        }
    }
}
