using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class LevelMenu : MonoBehaviour, IPointerEnterHandler
{
    [TextArea]
    public string message;
    public TMP_Text text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.text = message;
    }
}
