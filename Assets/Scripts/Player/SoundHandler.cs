using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundHandler : MonoBehaviour
{
    public float volume = .025f;

    public AudioClip[] jumpSounds;
    public AudioClip[] runSounds;
    public AudioClip[] damageSounds;
    public AudioClip[] deathSounds;

    AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        audio.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)], volume);     
    }
    public void PlayRunSound()
    {
        audio.PlayOneShot(runSounds[Random.Range(0, runSounds.Length)], volume);
    }
    public void PlayDamageSound()
    {
        audio.PlayOneShot(damageSounds[Random.Range(0, damageSounds.Length)], volume);
    }
    public void PlayDeathSound()
    {
        audio.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)], volume);
    }

}
