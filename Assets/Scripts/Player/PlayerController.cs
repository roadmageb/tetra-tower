using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public CharacterController2D controller;
    private float horizontalMove = 0f;
    public int hp = 0;
    private Queue<InputCode> rawInputs;
    
    private void GetInput()
    {
        
        if (Input.GetAxisRaw("Horizontal") > 0) { rawInputs.Enqueue(InputCode.Right); }
        if (Input.GetAxisRaw("Horizontal") < 0) { rawInputs.Enqueue(InputCode.Left); }
        if (Input.GetAxisRaw("Vertical") > 0) { rawInputs.Enqueue(InputCode.Up); }
        if (Input.GetAxisRaw("Vertical") < 0) { rawInputs.Enqueue(InputCode.Down); }
        if (Input.GetButton("Action1")) { rawInputs.Enqueue(InputCode.Action1); }
        if (Input.GetButton("Action2")) { rawInputs.Enqueue(InputCode.Action2); }
        if (Input.GetButton("Action3")) { rawInputs.Enqueue(InputCode.Action3); }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        rawInputs = new Queue<InputCode>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        controller.Jump(Input.GetButtonDown("Jump"), Input.GetButton("Jump"), Input.GetButtonUp("Jump"));
        GetInput();
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime);
    }
}
