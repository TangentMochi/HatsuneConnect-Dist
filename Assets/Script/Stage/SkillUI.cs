using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private TMP_Text mesage;
    [SerializeField] private Image charimage;

    [SerializeField] private Sprite miku;
    [SerializeField] private Sprite len;
    [SerializeField] private Sprite rin;
    [SerializeField] private Sprite luka;
    [SerializeField] private Sprite meiko;
    [SerializeField] private Sprite kaito;

    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Event(Character character, string meg)
    {
        switch(character)
        {
            case Character.Miku:
                charimage.sprite = miku;
                break;
            case Character.Rin:
                charimage.sprite = rin;
                break;
            case Character.Len:
                charimage.sprite = len;
                break;
            case Character.Luka:
                charimage.sprite = luka;
                break;
            case Character.Meiko:
                charimage.sprite = meiko;
                break;
            case Character.Kaito:
                charimage.sprite = kaito;
                break;
        }
        mesage.text = meg;
        rectTransform.DOComplete();
        rectTransform.anchoredPosition = new Vector3(-1280f, 0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), 0.2f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            rectTransform.DOAnchorPos(new Vector2(1280f, 0f), 0.2f).SetEase(Ease.InOutQuad).SetDelay(0.5f);
        });

    }
}