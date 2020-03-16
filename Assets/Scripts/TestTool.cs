using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTool : MonoBehaviour
{
    public InputField damageInput;
    public Enemy enemy;

    public void TestGetDamage()
    {
        if (damageInput.text != "")
        {
            PlayerController.Instance.GetDamage(int.Parse(damageInput.text));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ItemManager.Instance.CreateItem(Vector2.zero, ItemRank.Monomino, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AttackPtoE temp = new AttackPtoE(3);
            temp.AddCtrl(CtrlPtoE.Stun, 6);
            //enemy.GainAttack(temp);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            enemy.GetDamage(2);
        }
    }
}
