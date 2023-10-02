using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rin : BasePlayer
{
    [SerializeField] private GameObject laser_ballet;
    [SerializeField] private float SkillBulletSize;
    [SerializeField] private float SkillBulletSpeed;
    [SerializeField] private int SkillDamage;
    [SerializeField] private int LaserLen = 200;
    [SerializeField] private float LaserInterval = 0.005f;
    [SerializeField] private AudioClip skillClip;

    protected override void Skill()
    {
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
        StartCoroutine(SkillFire());
    }

    public override void SkillDummy()
    {
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
        StartCoroutine(Dummy());
    }

    private IEnumerator SkillFire()
    {
        Vector3 position = transform.position;
        for (int i=0; i < LaserLen; i++)
        {
            var bullet_tmp = Instantiate(laser_ballet, position, Quaternion.identity);
            var bullet_ins = bullet_tmp.GetComponent<Bullet>();
            bullet_ins.Init(SkillBulletSize + StageMaster.AddBulletSize, SkillBulletSpeed + StageMaster.AddBulletSpeed, SkillDamage);
            bullet_ins.is_ally = true;
            bullet_ins.is_enemy = false;
            yield return new WaitForSeconds(LaserInterval);
        }
    }

    private IEnumerator Dummy()
    {
        Vector3 position = transform.position;
        for (int i = 0; i < LaserLen; i++)
        {
            var bullet_tmp = Instantiate(laser_ballet, position, Quaternion.identity);
            var bullet_ins = bullet_tmp.GetComponent<Bullet>();
            bullet_ins.Init(SkillBulletSize + StageMaster.AddBulletSize, SkillBulletSpeed + StageMaster.AddBulletSpeed, SkillDamage);
            bullet_ins.is_ally = false;
            bullet_ins.is_enemy = false;
            yield return new WaitForSeconds(LaserInterval);
        }
    }
}
