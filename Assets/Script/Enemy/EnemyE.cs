using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyE : BaseEnemy
{
    private string text_char;

    protected override void EnemyMove()
    {
        transform.DOMove(new Vector3(transform.position.x, 4.5f, transform.position.z), 2f).OnComplete(() =>
        {
            transform.DOMove(new Vector3(transform.position.x, -4f, transform.position.z), 2f).OnComplete(() =>
            {
                EnemyMove();
            });
        });
    }

    private void FixedUpdate()
    {
        if (TextAlive.findChar() != "" && text_char != TextAlive.findChar())
        {
            text_char = TextAlive.findChar();
            Fire();
        }
    }
}