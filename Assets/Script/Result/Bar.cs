using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Bar : MonoBehaviour
{
    [SerializeField] private GameObject mask_pos;
    //[SerializeField] private GameObject bar_pos;

    //[SerializeField] private TMP_Text result;

    [SerializeField] private float progress;

    public void Result(float per)
    {
        DOVirtual.Float(1f, 1 - per, 5f, value =>
        {
            //result.text = $"{100 - (value * 100)}%";
            progress = value;
        }).SetEase(Ease.OutCubic);
    }

    void Update()
    {
        mask_pos.transform.position = new Vector3(-Mathf.Max(progress * 12f, 0f), mask_pos.transform.position.y, mask_pos.transform.position.z);
        //mask_pos.transform.position = new Vector3(-(progress * 850) + 480, mask_pos.transform.position.y, mask_pos.transform.position.z);
        //bar_pos.transform.position = new Vector3(-progress + 480, bar_pos.transform.position.y, bar_pos.transform.position.z);
        //bar_pos.transform.position = new Vector3(480, bar_pos.transform.position.y, bar_pos.transform.position.z);
    }
}
