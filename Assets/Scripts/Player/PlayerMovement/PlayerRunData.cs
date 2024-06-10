using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunData : MonoBehaviour
{
    [Header("Run")] 
    public float maxSpeed;
    public float acceleration;
    public float deceleration;
    [HideInInspector] public float accelAmount;
    [HideInInspector] public float decelAmount;
    [Range(0.01f, 1)] public float accelInAir;
    [Range(0.01f, 1)] public float decelInAir;
    public bool doConserveMemontum;

    private void OnValidate()
    {
        accelAmount = (50 * acceleration) / maxSpeed;
        decelAmount = (50 * deceleration) / maxSpeed;

        acceleration = Mathf.Clamp(acceleration, 0.01f, maxSpeed);
        deceleration = Mathf.Clamp(deceleration, 0.01f, maxSpeed);

    }
}
