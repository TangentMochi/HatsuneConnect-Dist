using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool is_enemy = false;
    public bool is_ally = false;

    protected float Scale=0;
    protected float Speed=0;
    protected int Damage=0;
    protected float angele = Mathf.PI / 2;

    protected void FixedUpdate()
    {
        var angele = is_enemy ? this.angele * -1 : this.angele;
        transform.position = new Vector3(Mathf.Sin(angele) * Speed + transform.position.x, Mathf.Cos(angele) * Speed + transform.position.y, transform.position.z);
        //transform.localPosition = new Vector3(transform.localPosition.x + delta, transform.localPosition.y, transform.localPosition.z);
        if (transform.position.x < -10 || 10 < transform.position.x)
        {
            Destroy(gameObject);
        }
    }

    public void Init(float Scale, float Speed, int Damage)
    {
        this.Scale = Scale;
        this.Speed = Speed;
        this.Damage = Damage;
        transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
    }

    public void Rotate(float degZ)
    {
        angele = (degZ + 90) / 180 * Mathf.PI;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, degZ);
    }

    public int GetDamage()
    {
        return this.Damage;
    }
}
