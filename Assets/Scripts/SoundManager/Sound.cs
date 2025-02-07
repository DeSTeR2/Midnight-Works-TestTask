using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType {
    Buy,
    Win, 
}

[Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip audio;
    public float timeToPlayAgain = 0f;
}
