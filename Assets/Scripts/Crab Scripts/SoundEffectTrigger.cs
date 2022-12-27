using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SoundEffectTrigger : MonoBehaviour
{
    [SerializeReference] AudioClip[] audioClip;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    //  Move step event
    private void MoveStep()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    // private void Attack()
    // {
    //     AudioClip clip = GetRandomClip();
    //     audioSource.PlayOneShot(clip);
    // }

    // private void HeavyAttack()
    // {
    //     AudioClip clip = GetRandomClip();
    //     audioSource.PlayOneShot(clip);
    // }

    // private void DashOut()
    // {
    //     AudioClip clip = GetRandomClip();
    //     audioSource.PlayOneShot(clip);
    // }

    // private void DashBack()
    // {
    //     AudioClip clip = GetRandomClip();
    //     audioSource.PlayOneShot(clip);
    // }

    private AudioClip GetRandomClip()
    {
        int index = Random.Range(0, audioClip.Length - 1);
        return audioClip[index];
    }
}
