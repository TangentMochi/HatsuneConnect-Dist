using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitRoom : MonoBehaviour
{
    [SerializeField] private Sprite NoMemberSprite;

    [SerializeField] private GameObject UserAObject;
    [SerializeField] private Sprite UserASprite;
    [SerializeField] private GameObject UserBObject;
    [SerializeField] private Sprite UserBSprite;
    [SerializeField] private GameObject UserCObject;
    [SerializeField] private Sprite UserCSprite;
    [SerializeField] private GameObject UserDObject;
    [SerializeField] private Sprite UserDSprite;
    [SerializeField] private GameObject UserEObject;
    [SerializeField] private Sprite UserESprite;

    // Start is called before the first frame update
    void Start()
    {
        Skyway.Instance.ChangeMembersHandler.AddListener(OnChangeMember);
    }

    void OnChangeMember(int member)
    {
        member = member - 1;
        if (0 < member)
        {
            UserAObject.GetComponent<Image>().sprite = UserASprite;
            UserAObject.GetComponentInChildren<TMP_Text>().SetText("");
        }else
        {
            UserAObject.GetComponent<Image>().sprite = NoMemberSprite;
            UserAObject.GetComponentInChildren<TMP_Text>().SetText("Waiting");
        }
        if (1 < member)
        {
            UserBObject.GetComponent<Image>().sprite = UserBSprite;
            UserBObject.GetComponentInChildren<TMP_Text>().SetText("");
        }
        else
        {
            UserBObject.GetComponent<Image>().sprite = NoMemberSprite;
            UserBObject.GetComponentInChildren<TMP_Text>().SetText("Waiting");
        }
        if (2 < member)
        {
            UserCObject.GetComponent<Image>().sprite = UserCSprite;
            UserCObject.GetComponentInChildren<TMP_Text>().SetText("");
        }
        else
        {
            UserCObject.GetComponent<Image>().sprite = NoMemberSprite;
            UserCObject.GetComponentInChildren<TMP_Text>().SetText("Waiting");
        }
        if (3 < member)
        {
            UserDObject.GetComponent<Image>().sprite = UserDSprite;
            UserDObject.GetComponentInChildren<TMP_Text>().SetText("");
        }
        else
        {
            UserDObject.GetComponent<Image>().sprite = NoMemberSprite;
            UserDObject.GetComponentInChildren<TMP_Text>().SetText("Waiting");
        }
        if (4 < member)
        {
            UserEObject.GetComponent<Image>().sprite = UserESprite;
            UserEObject.GetComponentInChildren<TMP_Text>().SetText("");
        }
        else
        {
            UserEObject.GetComponent<Image>().sprite = NoMemberSprite;
            UserEObject.GetComponentInChildren<TMP_Text>().SetText("Waiting");
        }
    }
}
