using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private CharacterController2D controller;
    float horizontalMove = 0f;
    public int hp = 0;
    private Queue<List<InputCode>> rawInputs;
    private bool[] inputChecker;

    private int inputCheckCount = 0, inputFrameLimit = 5;
    private int inputCount = 0;
    
    private void GetInput()
    {
        if(inputCheckCount < inputFrameLimit)
        {

            if (Input.GetAxisRaw("Vertical") > 0) { inputChecker[(int)InputCode.Up] = true; }
            if (Input.GetAxisRaw("Vertical") < 0) { inputChecker[(int)InputCode.Down] = true; }
            if (Input.GetAxisRaw("Horizontal") > 0) { inputChecker[(int)InputCode.Right] = true; }
            if (Input.GetAxisRaw("Horizontal") < 0) { inputChecker[(int)InputCode.Left] = true; }
            if (Input.GetButtonDown("Action1")) { inputChecker[(int)InputCode.Action1] = true; }
            if (Input.GetButtonDown("Action2")) { inputChecker[(int)InputCode.Action2] = true; }
            if (Input.GetButtonDown("Action3")) { inputChecker[(int)InputCode.Action3] = true; }
            inputCheckCount++;
        }
        else
        {
            inputCount++;
            List<InputCode> inputs = new List<InputCode>();
            for(int i = 0; i < inputChecker.Length; i++)
            {
                if (inputChecker[i])
                {
                    inputs.Add((InputCode)i);
                }
                inputChecker[i] = false;
            }
            rawInputs.Enqueue(inputs);
            inputCheckCount = 0;
        }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        rawInputs = new Queue<List<InputCode>>();
        inputChecker = new bool[(int)InputCode.NULL];
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        controller.Jump(Input.GetButtonDown("Jump"), Input.GetButton("Jump"), Input.GetButtonUp("Jump"));
        GetInput();
    }

    private void LateUpdate()
    {
        List<InputCode> asdf = rawInputs.Dequeue();
        string test = "";
        for (int i = 0; i < asdf.Count; i++) test += (InputCode)i;
        if (test != "") Debug.Log(test);
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime);
    }
}
