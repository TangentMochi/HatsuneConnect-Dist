using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyD : BaseEnemy
{
    private int beat_index = -1;
    private int now_seq = 0;

    protected override void EnemyMove()
    {
        float[] pos_y = { -3.5f, -2f, 0f, 2f, 3.5f };
        transform.DOMove(new Vector3(transform.position.x, pos_y[now_seq % 5], transform.position.z), 1f).OnComplete(() =>
        {
            now_seq += EnemyID % 3 + 1;
            EnemyMove();
        });
    }

    private void FixedUpdate()
    {
        if (-1 < TextAlive.findBeat() && beat_index != TextAlive.findBeat())
        {
            beat_index = TextAlive.findBeat();
            Laser(15f * (beat_index % 4) + -30f);
        }
    }
}