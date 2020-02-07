using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public CharacterController2D controller;
    private float horizontalMove = 0f;
    public int hp = 0;
    private bool[] actionChecker, arrowChecker;
    private bool isInputOn = false;

    public List<ComboInfo> possibleComboes;

    private int inputCheckCount = 0, inputFrameLimit = 5;
    private int comboSuccessCounter = 0;
    public int comboTimer = 60, comboCounter = 0;
    
    private void GetInput()
    {
        if (Input.GetButtonDown("Action1") || Input.GetButtonDown("Action2") || Input.GetButtonDown("Action3"))
        {
            isInputOn = true;
        }
        if (isInputOn)
        {
            if (inputCheckCount < inputFrameLimit)
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
                for (int i = actionChecker.Length - 1; i >= 0; i--)
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

                bool successCheck = false, perfectComboCheck = false;
                bool[] comboEnded = new bool[possibleComboes.Count];
                bool[] perfectComboes = new bool[possibleComboes.Count];
                for (int i = 0; i < possibleComboes.Count; i++)
                {
                    bool isPerfectCombo = false;
                    successCheck |= possibleComboes[i].CheckCombo(currentInputArrow, currentInputAction, comboSuccessCounter, out comboEnded[i], out perfectComboes[i]);
                    perfectComboCheck |= perfectComboes[i];
                }

                for(int i = 0; i < possibleComboes.Count; i++)
                {
                    if (comboEnded[i] && (!perfectComboCheck || perfectComboes[i]))
                    {
                        possibleComboes[i].DoCombo();
                    }
                }

                comboSuccessCounter = successCheck ? comboSuccessCounter + 1 : 0;
                if(successCheck)
                {
                    comboCounter = 0;
                }

                for (int i = 0; i < arrowChecker.Length; i++)
                {
                    arrowChecker[i] = false;
                }
                inputCheckCount = 0;
                if (!Input.GetButtonDown("Action1") && !Input.GetButtonDown("Action2") && !Input.GetButtonDown("Action3"))
                {
                    isInputOn = false;
                }
            }
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
        possibleComboes.Add(new ComboInfo(InputArrow.NULL, new int[2] { 3, 1 }, "A"));
        possibleComboes.Add(new ComboInfo(InputArrow.NULL, new int[3] { 3, 1, 1 }, "B"));
        possibleComboes.Add(new ComboInfo(InputArrow.Down, new int[2] { 1, 1 }, "C"));
        possibleComboes.Add(new ComboInfo(InputArrow.Up, new int[2] { 1, 1 }, "D"));
        possibleComboes.Add(new ComboInfo(InputArrow.Neutral, new int[2] { 1, 1 }, "E"));
    }

    // Update is called once per frame
    void Update()
    {
        if(comboCounter < comboTimer)
        {
            comboCounter++;
        }
        else
        {
            comboSuccessCounter = 0;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal");
        controller.Jump(Input.GetButtonDown("Jump"), Input.GetButton("Jump"), Input.GetButtonUp("Jump"));
        controller.Move(horizontalMove * Time.fixedDeltaTime);
        GetInput();
    }

    private void FixedUpdate()
    {
    }
}
