using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletText : MonoBehaviour
{
    public TMP_Text text;

    void Start()
    {
#if !UNITY_EDITOR
        text.text = "<color=black>" + TextAlive.findChar() + "</color>";
#endif
    }
}
