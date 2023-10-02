using System;
using UnityEngine;

[Serializable]
public class SongleSong
{
    public string url;
    public Artist artist;
    public int id;
    public double duration;
    public string permalink;
    public string code;
    public double rmsAmplitude;
    public DateTime createdAt;
    public DateTime updatedAt;
    public DateTime recognizedAt;
    public string title;
}

[Serializable]
public class Artist
{
    public int id;
    public string name;
}