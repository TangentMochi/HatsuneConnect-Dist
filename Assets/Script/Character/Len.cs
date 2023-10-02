using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Len : BasePlayer
{
    [SerializeField] private GameObject Turret;
    [SerializeField] private AudioClip skillClip;

    private GameObject turrent_ins;

    protected override void Skill()
    {
        if (turrent_ins) Destroy(turrent_ins);
        turrent_ins = Instantiate(Turret, transform.position, Quaternion.identity);
        turrent_ins.GetComponent<Turret>().is_dummy = false;
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
    }

    public override void SkillDummy()
    {
        if (turrent_ins) Destroy(turrent_ins);
        turrent_ins = Instantiate(Turret, transform.position, Quaternion.identity);
        turrent_ins.GetComponent<Turret>().is_dummy = true;
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
    }
}
