using System;
using UnityEngine;


public enum SoundType {
    Buy,
    Sold,
    Nature1,
    Nature2,
    Nature3,
    Nature4,
}

[Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip audio;
    public float timeToPlayAgain = 0f;
}
