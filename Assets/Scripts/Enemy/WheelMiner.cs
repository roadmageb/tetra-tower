using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMiner : Enemy
{
    [SerializeField] private GameObject wheelMinerPickaxe;

    public void AttackPattern1()
    {
        GameObject pickAxe = Instantiate(wheelMinerPickaxe, transform.parent);
        pickAxe.transform.position = transform.position + new Vector3(0.2f * (pickAxe.transform.localScale.x > 0 ? -1 : 1), 1.2f);
        pickAxe.transform.localScale = transform.localScale;
        pickAxe.transform.eulerAngles = pickAxe.transform.eulerAngles * (pickAxe.transform.localScale.x > 0 ? 1 : -1);
        pickAxe.AddComponent<WheelMinerPickAxe>();
        StartCoroutine(PickAxeMovement(pickAxe));
    }

    private IEnumerator PickAxeMovement(GameObject pickAxe)
    {
        Vector2 from = pickAxe.transform.position;
        Vector2 to = target.transform.position;
        for(float timer = 0; timer < 1; timer += Time.deltaTime)
        {
            yield return null;
            if(pickAxe == null)
            {
                break;
            }
            pickAxe.transform.position = Vector3.Lerp(from, to, timer);
        }
    }

    public override void IdleAction()
    {
        base.IdleAction();
        Patrol();
    }

}

public class WheelMinerPickAxe : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1), 5);
    }
}
