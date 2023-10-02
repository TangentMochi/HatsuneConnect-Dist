using System.Collections;
using UnityEngine;
using TMPro;


public class WordText : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    public TMP_Text text;
    void Start()
    {
#if !UNITY_EDITOR
        text.text = "<color=black>" + TextAlive.findWord() + "</color>";
#endif
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        if(transform.position.x < -15 || 15 < transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
