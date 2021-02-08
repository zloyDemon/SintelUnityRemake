using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "ScriptableObjects/Sounds", order = 1)]
public class SoundsSO : ScriptableObject
{
    [SerializeField] List<AudioClip> clips;

    public List<AudioClip> Clips => clips;

    public AudioClip GetSound(string name)
    {
        var clip = clips.Find(a => string.Equals(a.name, name));
        return clip;
    }
}
