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
        ItemManager.Instance.CreateItem(new Vector2(1, 0), ItemRank.Monomino);
        ItemManager.Instance.CreateItem(Vector2.zero, 4, 0);
        /*PlayerController.Instance.possibleComboes.Add(new ComboInfo("A", new int[1] { 1 }, InputArrow.NULL, PosCond.None));
        PlayerController.Instance.possibleComboes.Add(new ComboInfo("B", new int[1] { 1 }, InputArrow.Up, PosCond.None));
        PlayerController.Instance.possibleComboes.Add(new ComboInfo("C", new int[1] { 1 }, InputArrow.NULL, PosCond.Midair));
        PlayerController.Instance.possibleComboes.Add(new ComboInfo("D", new int[1] { 1 }, InputArrow.Up, PosCond.Midair));*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AttackPtoE temp = new AttackPtoE(3);
            temp.AddCtrl(CtrlPtoE.Stun, 6);
            enemy.GainAttack(temp);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            enemy.GetDamage(2);
        }
    }
}
