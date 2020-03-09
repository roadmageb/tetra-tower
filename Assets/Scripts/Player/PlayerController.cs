using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Singleton<PlayerController>
{
    public LifeStoneManager lifeStoneManager;
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
    public Queue<SkillInfo> skillQueue;
    public SkillInfo playingSkill = null;

    private Animator animator;
    private AnimatorOverrideController aoc;

    public struct PlayerAttribute
    {
        public float gravityScale;
    }
    PlayerAttribute originPlayerAttribute;

    public IEnumerator MovePlayer(Vector3 _dest)
    {
        controller.m_Controllable = false;
        GetComponent<Collider2D>().enabled = false;
        Vector3 from = transform.position;
        Vector3 to = new Vector3(_dest.x, _dest.y, transform.position.z);
        for (float timer = 0; timer < Room.roomMoveTime; timer += Time.deltaTime)
        {
            yield return null;
            transform.position = Vector3.Lerp(from, to, timer / Room.roomMoveTime);
        }
        transform.position = to;
        controller.m_Controllable = true;
        GetComponent<Collider2D>().enabled = true;
    }

    public void PlayerAttack(Enemy enemy)
    {
        enemy.GainAttack(playingSkill.wp.CalcAttack(playingSkill.num, enemy));
    }
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
                else currentInputArrow = InputArrow.Neutral;

                bool successCheck = false, perfectComboCheck = false;
                bool[] comboEnded = new bool[possibleComboes.Count];
                bool[] perfectComboes = new bool[possibleComboes.Count];
                for (int i = 0; i < possibleComboes.Count; i++)
                {
                    successCheck |= possibleComboes[i].CheckCombo(currentInputArrow, currentInputAction, comboSuccessCounter, out comboEnded[i], out perfectComboes[i]);
                    perfectComboCheck |= perfectComboes[i];
                }

                for(int i = 0; i < possibleComboes.Count; i++)
                {
                    if (comboEnded[i] && (!perfectComboCheck || perfectComboes[i]))
                    {
                        skillQueue.Enqueue(possibleComboes[i].skill);
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
    
    public void GetDamage(int damage)
    {
        lifeStoneManager.DestroyLifeStone(damage);
    }

    public void PlaySkillWeapon(int option)
    {
        playingSkill.wp.PlaySkill(playingSkill.num, option);
    }
    public void PlaySkillAnim()
    {
        controller.m_Attacking = true;
        aoc["PlayerAttack"] = playingSkill.wp.GetAnim(playingSkill.num);
        animator.SetTrigger("Attack");
    }
    private void Awake()
    {
        lifeStoneManager = GameObject.Find("LifeStoneManager").GetComponent<LifeStoneManager>();
        skillQueue = new Queue<SkillInfo>();
        animator = GetComponent<Animator>();
        aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = aoc;
        controller = GetComponent<CharacterController2D>();
        arrowChecker = new bool[(int)InputArrow.Front + 1];
        actionChecker = new bool[(int)InputAction.NULL];
        possibleComboes = new List<ComboInfo>();

        originPlayerAttribute.gravityScale = GetComponent<Rigidbody2D>().gravityScale;
    }
    public void ResetPossibleComboes()
    {
        possibleComboes.Clear();
        foreach (Weapon wp in ItemManager.Instance.currentWeapons)
        {
            foreach (ComboInfo combo in wp.info.commands)
            {
                possibleComboes.Add(combo);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!controller.m_Attacking && skillQueue.Count > 0)
        {
            playingSkill = skillQueue.Dequeue();
            PlaySkillAnim();
        }
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

    public void ResetPlayerAttribute()
    {
        GetComponent<Rigidbody2D>().gravityScale = originPlayerAttribute.gravityScale;
    }
}
