using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private CharacterController2D controller;
    float horizontalMove = 0f;
    public int hp = 0;
    private bool[] actionChecker, arrowChecker;

    public List<ComboInfo> possibleComboes;

    private int inputCheckCount = 0, inputFrameLimit = 5;
    
    private void GetInput()
    {
        if(inputCheckCount < inputFrameLimit)
        {

            if (Input.GetAxisRaw("Vertical") > 0) { arrowChecker[(int)InputArrow.Up] = true; }
            if (Input.GetAxisRaw("Vertical") < 0) { arrowChecker[(int)InputArrow.Down] = true; }
            if (Input.GetAxisRaw("Horizontal") != 0) { arrowChecker[(int)InputArrow.Front] = true; }

            if (Input.GetButtonDown("Action1")) { actionChecker[(int)InputAction.Action1] = true; }
            if (Input.GetButtonDown("Action2")) { actionChecker[(int)InputAction.Action2] = true; }
            if (Input.GetButtonDown("Action3")) { actionChecker[(int)InputAction.Action3] = true; }
            inputCheckCount++;
        }
        else
        {
            InputArrow currentInputArrow;
            int currentInputAction = 0;
            //Check action buttons
            for(int i = 0; i < actionChecker.Length; i++)
            {
                currentInputAction <<= 1;
                currentInputAction += actionChecker[i] ? 1 : 0;
                actionChecker[i] = false;
            }

            //Check arrow buttons
            if (arrowChecker[(int)InputArrow.Up] && arrowChecker[(int)InputArrow.Front]) currentInputArrow = InputArrow.UpFront;
            else if (arrowChecker[(int)InputArrow.Down] && arrowChecker[(int)InputArrow.Front]) currentInputArrow = InputArrow.DownFront;
            else if (arrowChecker[(int)InputArrow.Up]) currentInputArrow = InputArrow.Up;
            else if (arrowChecker[(int)InputArrow.Down]) currentInputArrow = InputArrow.Down;
            else if (arrowChecker[(int)InputArrow.Front]) currentInputArrow = InputArrow.Front;
            else currentInputArrow = InputArrow.NULL;

            for (int i = 0; i < possibleComboes.Count; i++)
            {
                possibleComboes[i].CheckCombo(currentInputArrow, currentInputAction);
            }

            for (int i = 0; i < arrowChecker.Length; i++)
            {
                arrowChecker[i] = false;
            }
            inputCheckCount = 0;
        }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        arrowChecker = new bool[(int)InputArrow.Front + 1];
        actionChecker = new bool[(int)InputAction.NULL];
        possibleComboes = new List<ComboInfo>();
    }

    private void Start()
    {
        possibleComboes.Add(new ComboInfo(InputArrow.NULL, new int[2] { 1, 2 }));
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
