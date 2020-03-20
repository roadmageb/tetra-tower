using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Transform lifeStoneUI;
    public Transform miniMap;
    public Text keyCountText;

    private void LateUpdate()
    {
        keyCountText.text = PlayerController.Instance.keyAmount.ToString();
    }
}
