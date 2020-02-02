using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private CharacterController2D controller;
    float horizontalMove = 0f;
    public int hp = 0;
    private Queue<InputCode> rawInputs;

    private bool[] inputChecker;

    private int inputCount = 0, inputFrameLimit = 5;
    
    private void GetInput()
    {
        if(inputCount < inputFrameLimit)
        {
            /*if (Input.GetAxisRaw("Horizontal") > 0) { rawInputs.Enqueue(InputCode.Right); }
            if (Input.GetAxisRaw("Horizontal") < 0) { rawInputs.Enqueue(InputCode.Left); }
            if (Input.GetAxisRaw("Vertical") > 0) { rawInputs.Enqueue(InputCode.Up); }
            if (Input.GetAxisRaw("Vertical") < 0) { rawInputs.Enqueue(InputCode.Down); }
            if (Input.GetButton("Action1")) { rawInputs.Enqueue(InputCode.Action1); }
            if (Input.GetButton("Action2")) { rawInputs.Enqueue(InputCode.Action2); }
            if (Input.GetButton("Action3")) { rawInputs.Enqueue(InputCode.Action3); }*/


            if (Input.GetAxisRaw("Vertical") > 0) { inputChecker[(int)InputCode.Up] = true; }
            if (Input.GetAxisRaw("Vertical") < 0) { inputChecker[(int)InputCode.Down] = true; }
            if (Input.GetAxisRaw("Horizontal") > 0) { inputChecker[(int)InputCode.Right] = true; }
            if (Input.GetAxisRaw("Horizontal") < 0) { inputChecker[(int)InputCode.Left] = true; }
            if (Input.GetButtonDown("Action1")) { inputChecker[(int)InputCode.Action1] = true; }
            if (Input.GetButtonDown("Action2")) { inputChecker[(int)InputCode.Action2] = true; }
            if (Input.GetButtonDown("Action3")) { inputChecker[(int)InputCode.Action3] = true; }
            inputCount++;
        }
        else
        {
            string test = "";
            for(int i = 0; i < inputChecker.Length; i++)
            {
                if (inputChecker[i])
                {
                    test += (InputCode)i;
                }
                inputChecker[i] = false;
            }
            if(test != "")
                Debug.Log(test);
            inputCount = 0;
        }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        rawInputs = new Queue<InputCode>();
        inputChecker = new bool[(int)InputCode.NULL];
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
