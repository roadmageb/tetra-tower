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
        pickAxe.GetComponent<WheelMinerPickAxe>().damage = attackPattern[0].attackDamage;
        StartCoroutine(PickAxeMovement(pickAxe));
    }

    private IEnumerator PickAxeMovement(GameObject pickAxe)
    {
        float flyTime = 1;
        Vector2 from = pickAxe.transform.position;
        Vector2 to = target.transform.position;
        Vector2 center = new Vector2((from.x + to.x) / 2, Mathf.Max(from.y, to.y) + 5);
        for (float timer = 0; timer < flyTime + 1; timer += Time.deltaTime)
        {
            yield return null;
            if (pickAxe == null)
            {
                break;
            }
            float t = timer / flyTime;
            pickAxe.transform.position = Mathf.Pow(1 - t, 2) * from + 2 * t * (1 - t) * center + Mathf.Pow(t, 2) * to;
        }
        Destroy(gameObject);
    }

    public override void TraceAction()
    {
        Patrol();
        if (Time.time - lastAttackedTime > attackDelay && Vector3.Distance(rb.position, target.position) <= attackRange)
        {
            AttackStart();
        }
    }
    public override void AttackAction()
    {
        base.AttackAction();
        Patrol();
    }
}
    
public class WheelMinerPickAxe : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            PlayerController.Instance.GetDamage(damage);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1), 5);
    }
}
