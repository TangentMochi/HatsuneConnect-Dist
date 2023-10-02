using System.Collections;
using UnityEngine;
using TMPro;

public class RoomIDText : MonoBehaviour
{
    [SerializeField] TMP_Text display_text;
    private void Update()
    {
        display_text.text = "ルームID: " + Skyway.Instance.room_id;
    }
}
