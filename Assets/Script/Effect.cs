using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] protected List<Sprite> sprites;
    [SerializeField] private float detime = 0.5f;

    private int now_spr = 0;
    private float time = 0f;
    private float ctime = 0f;

    void Update()
    {
        if (0.05 < time && sprites.Count != 0)
        {
            time = 0f;
            GetComponent<SpriteRenderer>().sprite = sprites[now_spr % sprites.Count];
            now_spr++;
        }

        if (detime < ctime)
        {
            Destroy(gameObject);
        }

        time += Time.deltaTime;
        ctime += Time.deltaTime;
    }
}
