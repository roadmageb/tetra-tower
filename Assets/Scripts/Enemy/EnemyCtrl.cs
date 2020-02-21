using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    Enemy enemy;
    public float stunImmTime = 6, freezeImmTime = 6;

    public bool isStun;
    bool isStunImm;
    float totalStun, currentStun, totalStunImm, currentStunImm;

    //tickBurn: 다음 프레임에 화상 피해를 입어야 할 때 true
    public bool isBurn;
    bool tickBurn;
    float timerBurn;
    int leftBurn;

    public bool isFreeze;
    bool isFreezeImm;
    float totalFreeze, currentFreeze, totalFreezeImm, currentFreezeImm;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (isStun)
        {
            currentStun += Time.deltaTime;
            if (currentStun > totalStun) EndStun();
        }
        if (isFreeze)
        {
            currentFreeze += Time.deltaTime;
            Debug.Log("Freeze");
            if (currentFreeze > totalFreeze) EndFreeze();
        }

        if (isBurn) timerBurn += Time.deltaTime;
        if (isBurn && timerBurn > 1)
        {
            timerBurn -= 1f;
            tickBurn = true;
            if (--leftBurn <= 0) isBurn = false;

            enemy.GetDamage(PlayerController.Instance.hp * 0.15f);
        }

        if (isStunImm) currentStunImm += Time.deltaTime;
        if (isStunImm && currentStunImm > totalStunImm) isStunImm = false;

        if (isFreezeImm) currentFreezeImm += Time.deltaTime;
        if (isFreezeImm && currentFreezeImm > totalFreezeImm) isFreezeImm = false;
    }
    public void EndStun()
    {
        isStun = false;
        isStunImm = true;
        totalStunImm = stunImmTime;
        currentStunImm = 0;

        enemy.transform.Find("EnemyPlaceHolder").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        enemy.animator.SetBool("CtrlPtoE", false);
    }
    public void EndFreeze() 
    {
        isFreeze = false;
        isFreezeImm = true;
        totalFreezeImm = freezeImmTime;
        currentFreezeImm = 0;

        enemy.transform.Find("EnemyPlaceHolder").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        enemy.animator.SetBool("CtrlPtoE", false);
    }
    public Dictionary<CtrlPtoE, bool> ApplyCtrl(AttackPtoE attack)
    {
        Dictionary<CtrlPtoE, bool> result = new Dictionary<CtrlPtoE, bool>();
        foreach (KeyValuePair<CtrlPtoE, float> ctrl in attack.ctrls)
        {
            switch(ctrl.Key)
            {
                case CtrlPtoE.Stun:
                    result.Add(ctrl.Key, ApplyStun(ctrl.Value));
                    break;
                case CtrlPtoE.Freeze:
                    result.Add(ctrl.Key, ApplyFreeze(ctrl.Value));
                    break;
                case CtrlPtoE.Burn:
                    result.Add(ctrl.Key, ApplyBurn((int)ctrl.Value));
                    break;
            }
        }
        return result;
    }
    public bool BurnTickCheck()
    {
        if(tickBurn)
        {
            tickBurn = false;
            return true;
        }
        return false;
    }
    public bool ApplyStun(float f)
    {
        if (isStunImm)
        {
            return false;
        }
        else
        {
            if(isStun)
            {
                totalStun = totalStun > f ? totalStun : f;
            }
            else
            {
                isStun = true;
                totalStun = f;
                currentStun = 0;
            }

            enemy.transform.Find("EnemyPlaceHolder").GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
            enemy.animator.SetBool("CtrlPtoE", true);
            return true;
        }
    }
    public bool ApplyFreeze(float f)
    {
        if (isFreezeImm)
        {
            Debug.Log("a");
            return false;
        }
        else
        {
            if(isFreeze)
            {
                Debug.Log("b");
                totalFreeze = f;
            }
            else
            {
                Debug.Log("c");
                isFreeze = true;
                totalFreeze = f;
                currentFreeze = 0;
            }

            enemy.transform.Find("EnemyPlaceHolder").GetComponent<SpriteRenderer>().color = new Color(0, 0, 1);
            enemy.animator.SetBool("CtrlPtoE", true);
            return true;
        }
    }
    public bool ApplyBurn(int n)
    {
        leftBurn = Mathf.Min(5, leftBurn + n);
        if(!isBurn)
        {
            isBurn = true;
            timerBurn = 0;
        }

        enemy.transform.Find("EnemyPlaceHolder").GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        return true;
    }
}
