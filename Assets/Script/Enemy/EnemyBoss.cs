using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBoss : BaseEnemy
{
    [SerializeField] private Sprite SpriteA;
    [SerializeField] private Sprite SpriteB;
    [SerializeField] private Sprite SpriteC;
    [SerializeField] private Sprite SpriteD;
    [SerializeField] private Sprite SpriteE;
    [SerializeField] private Sprite SpriteF;
    [SerializeField] private Sprite SpriteG;
    [SerializeField] private Sprite SpriteH;
    [SerializeField] private Sprite SpriteI;

    private int pos = 0;
    private int beat_index = -1;
    private string text_char;
    private string text_phrase;
    private float deg;
    private float now_deg;
    private string word;
    private int now_wave = 0;
    private int next_wave = 0;
    private int move_flag = 0;
    private float flag_hf = 0f;
    private float flag_hc = 0f;
    private readonly float[] flag_h = { -4.5f, -3.5f, -2.5f, -1.5f, 0f, 1.5f, 2.5f, 3.5f, 4.5f};
    private int flag_i = 0;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        Random.InitState(EnemyID);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void EnemyMove()
    {
        now_wave = next_wave;
        switch(Skyway.Instance.Level)
        {
            case Level.Easy:
                switch (now_wave)
                {
                    case 0:
                        next_wave = 1;
                        StartCoroutine(MoveA());
                        break;
                    case 1:
                        next_wave = 2;
                        StartCoroutine(MoveB());
                        break;
                    case 2:
                        next_wave = 0;
                        StartCoroutine(MoveC());
                        break;
                }
                break;
            case Level.Normal:
                switch (now_wave)
                {
                    case 0:
                        next_wave = 1;
                        StartCoroutine(MoveA());
                        break;
                    case 1:
                        next_wave = 2;
                        StartCoroutine(MoveB());
                        break;
                    case 2:
                        next_wave = 3;
                        StartCoroutine(MoveC());
                        break;
                    case 3:
                        next_wave = 4;
                        StartCoroutine(MoveD());
                        break;
                    case 4:
                        next_wave = 5;
                        StartCoroutine(MoveE());
                        break;
                    case 5:
                        next_wave = 0;
                        StartCoroutine(MoveF());
                        break;
                }
                break;
            case Level.Hard:
                switch (now_wave)
                {
                    case 0:
                        next_wave = 1;
                        StartCoroutine(MoveA());
                        break;
                    case 1:
                        next_wave = 2;
                        StartCoroutine(MoveB());
                        break;
                    case 2:
                        next_wave = 6;
                        StartCoroutine(MoveC());
                        break;
                    case 6:
                        next_wave = 7;
                        StartCoroutine(MoveG());
                        break;
                    case 7:
                        next_wave = 8;
                        StartCoroutine(MoveH());
                        break;
                    case 8:
                        next_wave = 0;
                        StartCoroutine(MoveI());
                        break;
                }
                break;
        }
    }
    

    private IEnumerator MoveA()
    {
        now_wave = 0;
        spriteRenderer.sprite = SpriteA;
        move_flag += 1;
        transform.DOMove(new Vector3(6, flag_h[move_flag % 9], 0), 8f);
        yield return new WaitForSeconds(8);
        EnemyMove();
    }

    private IEnumerator MoveB()
    {
        now_wave = 1;
        spriteRenderer.sprite = SpriteB;
        yield return new WaitForSeconds(8);
        transform.DOComplete();
        EnemyMove();
    }

    private IEnumerator MoveC()
    {
        now_wave = 2;
        spriteRenderer.sprite = SpriteC;
        move_flag += 1;
        transform.DOMove(new Vector3(6, flag_h[move_flag % 9], 0), 8f);
        yield return new WaitForSeconds(8);
        EnemyMove();
    }

    private IEnumerator MoveD()
    {
        now_wave = 3;
        spriteRenderer.sprite = SpriteD;
        move_flag+=2;
        transform.DOMove(new Vector3(9, flag_h[move_flag % 9], 0), 4f);
        yield return new WaitForSeconds(4);
        EnemyMove();
    }

    private IEnumerator MoveE()
    {
        now_wave = 4;
        spriteRenderer.sprite = SpriteE;
        for (int i = 0; i < 5; i++)
        {
            flag_hf = flag_h[Random.Range(0, 10)];
            yield return new WaitForSeconds(2);
        }
        EnemyMove();
    }

    private IEnumerator MoveF()
    {
        now_wave = 5;
        spriteRenderer.sprite = SpriteF;
        yield return new WaitForSeconds(8);
        EnemyMove();
    }

    private IEnumerator MoveG()
    {
        now_wave = 6;
        spriteRenderer.sprite = SpriteG;
        move_flag+=2;
        transform.DOMove(new Vector3(9, flag_h[move_flag % 9], 0), 4f);
        yield return new WaitForSeconds(4);
        EnemyMove();
    }

    private IEnumerator MoveH()
    {
        now_wave = 7;
        spriteRenderer.sprite = SpriteH;
        for (int i = 0; i < 5; i++)
        {
            flag_hf = flag_h[Random.Range(0, 10)];
            yield return new WaitForSeconds(2);
        }
        EnemyMove();
    }

    private IEnumerator MoveI()
    {
        now_wave = 8;
        spriteRenderer.sprite = SpriteI;
        yield return new WaitForSeconds(5);
        EnemyMove();
    }

    private void Update()
    {
        switch(now_wave)
        {
            case 0:
                //A
                if (-1 < TextAlive.findBeat() && beat_index != TextAlive.findBeat())
                {
                    beat_index = TextAlive.findBeat();
                    Laser(15f * (beat_index % 4) + -30f);
                }
                break;
            case 1:
                //B
                if (TextAlive.findPhrase() != "" && text_phrase != TextAlive.findPhrase())
                {
                    text_phrase = TextAlive.findPhrase();
                    deg = 120f / TextAlive.findPhraseCount();
                    pos++;
                    if (pos % 2 == 0)
                    {
                        now_deg = 45f;
                    }
                    else
                    {
                        now_deg = -45f;
                    }
                    if (pos % 3 == 1)
                    {
                        transform.DOMove(new Vector3(init_pos.x, 0f, init_pos.z), 1f);
                    }
                    else if (pos % 3 == 0)
                    {
                        transform.DOMove(new Vector3(init_pos.x, -4.5f, init_pos.z), 1f);
                    }
                    else
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
                    }
                    else
                    {
                        now_deg += deg;
                    }
                }
                break;
            case 2:
                //C
                if ((TextAlive.findWord() != "" && word != TextAlive.findWord()) || FireInterval / StageMaster.DevBulletInterval < last_fire)
                {
                    last_fire = 0;
                    word = TextAlive.findWord();
                    Fire((TextAlive.position() % 120) - 60);
                }
                break;
            case 3:
                //D
                if (0.1f < last_fire)
                {
                    last_fire = 0f;
                    Laser();
                }
                break;
            case 4:
                //E
                if (0.2f < last_fire)
                {
                    last_fire = 0f;
                    if (!(flag_hf - 0.02f < transform.position.y && transform.position.y < flag_hf + 0.02f))
                    {
                        Laser();
                    }
                }
                transform.position = new Vector3(transform.position.x, Mathf.Sin(Mathf.PI * flag_hc) * 4.5f, transform.position.z);
                flag_hc += Time.deltaTime;
                break;
            case 5:
                //F
                if (0.65f < last_fire)
                {
                    last_fire = 0;
                    for (int deg = 0; deg < 360; deg += 10)
                    {
                        Laser(deg + flag_i);
                    }
                    flag_i += 5;
                }
                break;
            case 6:
                //G
                if (0.1f < last_fire)
                {
                    last_fire = 0f;
                    Fire();
                }
                break;
            case 7:
                //H
                if (0.2f < last_fire)
                {
                    last_fire = 0f;
                    if (!(flag_hf - 0.02f < transform.position.y && transform.position.y < flag_hf + 0.02f))
                    {
                        Fire();
                    }
                }
                transform.position = new Vector3(transform.position.x, Mathf.Sin(Mathf.PI * flag_hc) * 4.5f, transform.position.z);
                flag_hc += Time.deltaTime;
                break;
            case 8:
                //I
                if (0.65f < last_fire)
                {
                    last_fire = 0;
                    for (int deg = 0; deg < 360; deg += 10)
                    {
                        Fire(deg + flag_i);
                    }
                    flag_i += 5;
                }
                break;
        }
        last_fire += Time.deltaTime;
    }
}
