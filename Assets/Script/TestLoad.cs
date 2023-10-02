using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadScene(string name)
    {
        FadeSence.Instance.LoadSence(name);
    }
}
