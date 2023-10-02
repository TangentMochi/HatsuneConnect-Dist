using UnityEngine;
using DG.Tweening;

public class EnemyB : BaseEnemy
{
    private string word;
    private GameObject wordbullet;
    private int now_seq = 0;


    protected override void EnemyMove()
    {
        var time = 0f;
        switch (Skyway.Instance.Level)
        {
            case Level.Hard:
                time = 0.5f;
                break;
            case Level.Normal:
                time = 1f;
                break;
            case Level.Easy:
                time = 1.5f;
                break;
        }
        float[] pos_y = { -4.5f, -1.5f, 0f, 1.5f, 4.5f };
        transform.DOMove(new Vector3(transform.position.x,  pos_y[now_seq % 5], transform.position.z), time).SetEase(Ease.Linear).OnComplete(() =>
        {
            now_seq += EnemyID % 3 + 1;
            EnemyMove();
        });
    }

    void Update()
    {
        if ((TextAlive.findWord() != "" && word != TextAlive.findWord()) || FireInterval / StageMaster.DevBulletInterval < last_fire)
        {
            last_fire = 0;
            word = TextAlive.findWord();
            Fire((TextAlive.position() % 120) - 60);
        }
        last_fire += Time.deltaTime;
    }

    private void WordFire()
    {
        var bullet = Instantiate(wordbullet, transform.position, Quaternion.identity);
        var bullet_bullet = bullet.GetComponent<Bullet>();
        bullet_bullet.Init(BulletScale, BulletSpeed, BulletDamage);
        bullet_bullet.is_ally = false;
        bullet_bullet.is_enemy = true;
    }
}
