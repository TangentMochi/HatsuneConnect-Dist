using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhraseText : MonoBehaviour
{
    [SerializeField] private TMP_Text phrase;

    void FixedUpdate()
    {
#if !UNITY_EDITOR
        phrase.text = TextAlive.findPhrase();
#endif
    }
}
