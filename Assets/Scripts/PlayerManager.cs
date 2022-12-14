using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
 
    [Header("World Settings")]
    public static readonly float gravity = 9.81f;
    public static readonly float friction = 2.5f;

    [Header("Fever Time Settings")]
    public static readonly float maxSliderValue = 100.0f;
    public static float valueCountMultiplier = 1.0f;
    public static readonly float feverTimeDuration = 20.0f;
    public static float feverTimeDurationCountMultiplier = 1.0f;
}
