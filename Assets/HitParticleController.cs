using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleController : MonoBehaviour
{
    ParticleSystem ps;
    EffectPool parent;

    public void Init(EffectPool eff)
    {
        ps = GetComponent<ParticleSystem>();
        parent = eff;
    }

    public void StartParticle(Vector3 pos, float angle)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        transform.eulerAngles = new Vector3(-angle, 90, 90);
        ps.Play();
    }

    public void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        parent.StoreEffect(gameObject);
    }
}
