using System.Collections;
using UnityEngine;
using DG.Tweening;

public class StageWordText : MonoBehaviour
{
    private GameObject wordObject;
    private string word;

    private void Start()
    {
        wordObject = (GameObject)Resources.Load("Word");
    }

    void Update()
    {
#if !UNITY_EDITOR
        if (TextAlive.findWord() != "" && word != TextAlive.findWord())
        {
            word = TextAlive.findWord();
            transform.position = new Vector3(transform.position.x, Random.Range(-4.5f, 4.5f), transform.position.z);
            Instantiate(wordObject, transform.position, Quaternion.identity);
        }
#endif
    }
}
