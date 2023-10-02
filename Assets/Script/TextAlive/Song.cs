using System.Collections;
using UnityEngine;

public struct Song
{
    public Song(string songUrl, int beatId, int chordId, int repetitiveSegmentId, int lyricId, int lyricDiffId)
    {
        this.songUrl = songUrl;
        this.beatId = beatId;
        this.chordId = chordId;
        this.repetitiveSegmentId = repetitiveSegmentId;
        this.lyricId = lyricId;
        this.lyricDiffId = lyricDiffId;
    }
    public string songUrl;
    public int beatId, chordId, repetitiveSegmentId, lyricId, lyricDiffId;
}

