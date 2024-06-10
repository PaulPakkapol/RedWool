using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    public PlayerRunData data;
    
    public float lastOnGroundTime { get; private set; }
    public Rigidbody2D rb { get; private set; }
    private Vector2 _moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        lastOnGroundTime -= Time.deltaTime;
        _moveInput.x = Input.GetAxisRaw("Horizontal");
    }
    
}
