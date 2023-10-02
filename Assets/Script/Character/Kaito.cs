using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaito : BasePlayer
{
    [SerializeField] private AudioClip skillClip;
    [SerializeField] private GameObject effectGame;

    protected override void Skill()
    {
        Skyway.Instance.writeStream("anti", "", true);
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
        Instantiate(effectGame, transform.position, Quaternion.identity, transform);
    }

    public override void SkillDummy()
    {
        audioSource.volume = 0.5f * TextAlive.volume();
        audioSource.PlayOneShot(skillClip);
        Instantiate(effectGame, transform.position, Quaternion.identity, transform);
    }
}
