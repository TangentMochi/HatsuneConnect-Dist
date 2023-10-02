using UnityEngine;
using DG.Tweening;

public class EnemyA : BaseEnemy
{
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
        transform.DOMove(new Vector3(transform.position.x, Mathf.Min(init_pos.y + (EnemyID % 5), 4.5f), transform.position.z), time).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (Skyway.Instance.Level == Level.Hard) Fire();
            transform.DOMove(new Vector3(transform.position.x, Mathf.Max(init_pos.y - (EnemyID % 4), -4f), transform.position.z), time).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (Skyway.Instance.Level == Level.Hard) Fire();
                EnemyMove();
            });
        });
    }

    void Update()
    {
        if ((FireInterval / StageMaster.DevBulletInterval) < last_fire)
        {
            last_fire = 0f;
            switch(Skyway.Instance.Level)
            {
                case Level.Hard:
                    Fire(25);
                    Fire();
                    Fire(-25);
                    break;
                case Level.Normal:
                    Fire(15);
                    Fire(-15);
                    break;
                case Level.Easy:
                    Fire();
                    break;
                default:
                    Fire();
                    break;
            }
        }
        last_fire += Time.deltaTime;
    }
}
