using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text result;

    public void Result(int score)
    {
        DOVirtual.Int(0, score, 5f, value =>
        {
            result.text = value.ToString();
        }).SetEase(Ease.InOutCubic);
    }
}
