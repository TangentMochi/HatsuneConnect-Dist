using UnityEngine;

public class Turret : MonoBehaviour
{
    public bool is_dummy;
    private GameObject bullet_prefab;
    private float last_fired;

    [SerializeField] private float BulletSize;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private float BulletInterval;
    [SerializeField] private int BulletDamage;
    [SerializeField] private int HP = 100;
    [SerializeField] private int remain_life;

    private void Start()
    {
        remain_life = HP;
        bullet_prefab = (GameObject)Resources.Load("Bullet");
    }

    private void Update()
    {
        if (BulletInterval - StageMaster.AddBulletInterval < last_fired)
        {
            last_fired = 0;
            var bullet_tmp = Instantiate(bullet_prefab, transform.position, Quaternion.identity);
            var bullet_ins = bullet_tmp.GetComponent<Bullet>();
            bullet_ins.Init(BulletSize + StageMaster.AddBulletSize, BulletSpeed + StageMaster.AddBulletSpeed, BulletDamage);
            bullet_ins.GetComponent<Bullet>().is_ally = !is_dummy;
            bullet_ins.GetComponent<Bullet>().is_enemy = false;
            bullet_ins.Rotate(10);

            bullet_tmp = Instantiate(bullet_prefab, transform.position, Quaternion.identity);
            bullet_ins = bullet_tmp.GetComponent<Bullet>();
            bullet_ins.Init(BulletSize + StageMaster.AddBulletSize, BulletSpeed + StageMaster.AddBulletSpeed, BulletDamage);
            bullet_ins.GetComponent<Bullet>().is_ally = !is_dummy;
            bullet_ins.GetComponent<Bullet>().is_enemy = false;
            bullet_ins.Rotate(0);

            bullet_tmp = Instantiate(bullet_prefab, transform.position, Quaternion.identity);
            bullet_ins = bullet_tmp.GetComponent<Bullet>();
            bullet_ins.Init(BulletSize + StageMaster.AddBulletSize, BulletSpeed + StageMaster.AddBulletSpeed, BulletDamage);
            bullet_ins.GetComponent<Bullet>().is_ally = !is_dummy;
            bullet_ins.GetComponent<Bullet>().is_enemy = false;
            bullet_ins.Rotate(-10);
        }
        last_fired += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var bullet = collision.GetComponent<Bullet>();
        if (bullet)
        {
            if (bullet.is_enemy && !is_dummy)
            {
                Destroy(collision.gameObject);
                remain_life -= bullet.GetDamage();
                if (remain_life < 0) Skyway.Instance.writeStream("turret", "");
            }
        }
    }
}
