﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private CharacterController2D controller;
    float horizontalMove = 0f;
    public int hp = 0;

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        controller.Jump(Input.GetButtonDown("Jump"), Input.GetButton("Jump"), Input.GetButtonUp("Jump"));
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime);

    }
}
