using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider masVolSlider;
    [SerializeField] private Slider SEVolSlider;
    [SerializeField] private Slider BGMVolSlider;

    void Start()
    {
        SoundManager.Instance.ChangeMasterVolume(masVolSlider.value);
        masVolSlider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
    }
}
