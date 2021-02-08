using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    [SerializeField] SoundsSO characterSounds;
    [SerializeField] AudioSource footStepsAS;

    public void PlayFootSteps()
    {
        var clip = characterSounds.Clips[Random.Range(0, characterSounds.Clips.Count - 1)];
        footStepsAS.PlayOneShot(clip);
    }
}
