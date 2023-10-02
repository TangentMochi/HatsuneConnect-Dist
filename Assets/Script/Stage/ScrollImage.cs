using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollImage : MonoBehaviour
{
    [SerializeField] float pos_x;
    [SerializeField] float interval;
    [SerializeField] float speed;

    [SerializeField] Sprite StageA;
    [SerializeField] Sprite StageB;
    [SerializeField] Sprite StageC;
    [SerializeField] Sprite StageD;
    [SerializeField] Sprite StageE;
    [SerializeField] Sprite StageF;


    void Start()
    {
        Stage stage = Skyway.Instance.Stage;
        switch(stage)
        {
            case Stage.A:
                ChangeSprite(StageA);
                break;
            case Stage.B:
                ChangeSprite(StageB);
                break;
            case Stage.C:
                ChangeSprite(StageC);
                break;
            case Stage.D:
                ChangeSprite(StageD);
                break;
            case Stage.E:
                ChangeSprite(StageE);
                break;
            case Stage.F:
                ChangeSprite(StageF);
                break;
            default:
                ChangeSprite(StageA);
                break;
        }
    }

    void ChangeSprite(Sprite sprite)
    {
        foreach(SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.sprite = sprite;
        }
    }

    void Update()
    {
        transform.position = new Vector3(pos_x % interval, transform.position.y, transform.position.z);
        pos_x += Time.deltaTime * speed;
    }
}
