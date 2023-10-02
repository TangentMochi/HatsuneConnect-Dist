using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageImage : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite sprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = sprite;
    }
}
