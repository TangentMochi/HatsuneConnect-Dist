using System.Collections;
using UnityEngine;

public class LaserColor : MonoBehaviour
{
    private Color[] colors = {
        new Color(64f / 255f, 177f / 255f, 185f / 255f),
        new Color(255f / 255f, 104f / 255f, 1f / 255f),
        new Color(253f / 255f, 225f / 255f, 19f / 255f),
        new Color(203f / 255f, 1f / 255f, 17f / 255f),
        new Color(239f / 255f, 123f / 255f, 158f / 255f),
        new Color(58f / 255f, 103f / 255f, 196f / 255f)
    };
    private int now_pos = 0;
    private string now_chord = "N";

    private void FixedUpdate()
    {
        string chord = TextAlive.findChord();
        if (chord != "N" && chord != now_chord)
        {
            now_chord = chord;
            GetComponent<SpriteRenderer>().color = colors[now_pos];
            now_pos++;
            if (6 <= now_pos)
            {
                now_pos = 0;
            }
        }
    }
}
