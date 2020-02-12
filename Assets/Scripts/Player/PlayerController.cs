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

    public int keyAmount = 0;
    public float keyPercent = 0f;

    public float damage = 0f;
    public SkillInfo playingSkill = null;
    
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
                        playingSkill = possibleComboes[i].skill;
                        PlaySkillAnim();
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

    public void PlaySkillWeapon(int option)
    {
        playingSkill.wp.PlaySkill(playingSkill.num, option);
    }
    public void PlaySkillAnim()
    {
        Animator animator = GetComponent<Animator>();
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        aoc["Attack"] = playingSkill.wp.GetAnim(playingSkill.num);
        animator.runtimeAnimatorController = aoc;
        animator.SetTrigger("start");
    }
    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        arrowChecker = new bool[(int)InputArrow.Front + 1];
        actionChecker = new bool[(int)InputAction.NULL];
        possibleComboes = new List<ComboInfo>();
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
        GetInput();
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime);
    }
}
