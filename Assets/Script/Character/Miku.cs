using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miku : BasePlayer
{
    [SerializeField] private float SkillBulletSize;
    [SerializeField] private float SkillBulletSpeed;
    [SerializeField] private int SkillDamage;
    [SerializeField] private AudioClip skillClip;
    protected override void Skill()
    {
        StartCoroutine(SkillCol(false));
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
    }

    private IEnumerator SkillCol(bool is_dummy)
    {
        for (float i = -45f; i < 45f; i+=5f)
        {
            var bullet_tmp = Instantiate(bullet_prefab, transform.position, Quaternion.identity);
            var bullet_ins = bullet_tmp.GetComponent<Bullet>();
            bullet_ins.Init(SkillBulletSize + StageMaster.AddBulletSize, SkillBulletSpeed + StageMaster.AddBulletSpeed, SkillDamage);
            bullet_ins.Rotate(i);
            bullet_ins.is_ally = !is_dummy;
            bullet_ins.is_enemy = false;
        }
        yield return new WaitForSeconds(0.05f);
        for (float i = -45f; i < 45f; i += 5f)
        {
            var bullet_tmp = Instantiate(bullet_prefab, transform.position, Quaternion.identity);
            var bullet_ins = bullet_tmp.GetComponent<Bullet>();
            bullet_ins.Init(SkillBulletSize + StageMaster.AddBulletSize, SkillBulletSpeed + StageMaster.AddBulletSpeed, SkillDamage);
            bullet_ins.Rotate(i);
            bullet_ins.is_ally = !is_dummy;
            bullet_ins.is_enemy = false;
        }
    }

    public override void SkillDummy()
    {
        StartCoroutine(SkillCol(true));
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
    }
}
