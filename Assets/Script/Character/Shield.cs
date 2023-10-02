using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int HP = -1;
    public bool is_dummy = false;
    
    public void Init()
    {
        switch (Skyway.Instance.Level)
        {
            case Level.Easy:
                HP = 200;
                break;
            case Level.Normal:
                HP = 150;
                break;
            case Level.Hard:
                HP = 100;
                break;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!is_dummy)
        {
            if (HP < 0)
            {
                Init();
            }
            var bullet = collision.GetComponent<Bullet>();
            if (bullet)
            {
                if (bullet.is_enemy)
                {
                    Destroy(collision.gameObject);
                    HP -= bullet.GetDamage();
                    if (HP < 0)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
