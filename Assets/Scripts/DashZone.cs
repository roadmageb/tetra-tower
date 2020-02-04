using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashZone : MonoBehaviour
{
    [SerializeField] private InputCode dashDir;
    private bool isDashable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isDashable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isDashable = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashable)
        {
            if (dashDir == InputCode.Up && Input.GetKeyDown(KeyCode.UpArrow) ||
               dashDir == InputCode.Down && Input.GetKeyDown(KeyCode.DownArrow) ||
               dashDir == InputCode.Left && Input.GetKeyDown(KeyCode.LeftArrow) ||
               dashDir == InputCode.Right && Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(PlayerController.Instance.controller.Dash(dashDir, transform.position));
            }
        }
    }
}
