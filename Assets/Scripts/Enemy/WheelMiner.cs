using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMiner : Enemy
{
    [SerializeField] private GameObject wheelMinerPickaxe;

    public void AttackPattern1()
    {
        GameObject pickAxe = Instantiate(wheelMinerPickaxe, transform.position, Quaternion.identity, transform.parent);
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
}
