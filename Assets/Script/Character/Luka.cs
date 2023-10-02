using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luka : BasePlayer
{
    [SerializeField] private AudioClip skillClip;
    protected override void Skill()
    {
        Skyway.Instance.writeStream("shield", "", true);
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
    }

    public override void SkillDummy()
    {
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
    }
}
