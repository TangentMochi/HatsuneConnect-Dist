using System.Collections;
using UnityEngine;
using DG.Tweening;


public class EnemyC : BaseEnemy
{
    private int pos = 0;
    private string text_char;
    private string text_phrase;
    private float deg;
    private float now_deg;

    private void Start()
    {
        pos = EnemyID;
    }

    protected override void EnemyMove()
    {
        
    }

    void Update()
    {
        if (TextAlive.findPhrase() != "" && text_phrase != TextAlive.findPhrase())
        {
            text_phrase = TextAlive.findPhrase();
            deg = 120f / TextAlive.findPhraseCount();
            pos++;
            if (pos % 2 == 0)
            {
                now_deg = 45f;
            } else
            {
                now_deg = -45f;
            }
            if (pos % 3 == 1)
            {
                transform.DOMove(new Vector3(init_pos.x, 0f, init_pos.z), 1f);
            } else if(pos % 3 == 0)
            {
                transform.DOMove(new Vector3(init_pos.x, -4.5f, init_pos.z), 1f);
            } else
            {
                transform.DOMove(new Vector3(init_pos.x, 4.5f, init_pos.z), 1f);
            }
        }
        if (TextAlive.findChar() != "" && text_char != TextAlive.findChar())
        {
            text_char = TextAlive.findChar();
            Laser(now_deg);
            if (pos % 2 == 0)
            {
                now_deg -= deg;
            } else
            {
                now_deg += deg;
            }
        }
    }
}
