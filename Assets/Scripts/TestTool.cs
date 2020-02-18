using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTool : MonoBehaviour
{
    public InputField damageInput;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
