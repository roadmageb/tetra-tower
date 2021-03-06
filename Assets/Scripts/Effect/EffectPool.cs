﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : Singleton<EffectPool>
{
    public int initEffectNum = 10;
    public int initHitParticleNum = 10;
    public GameObject effectPrefab;
    public GameObject hitParticle;
    private List<GameObject> effectPool;
    private List<GameObject> hitParticlePool;
    void Start()
    {
        effectPool = new List<GameObject>();
        while(effectPool.Count < initEffectNum)
        {
            effectPool.Add(Instantiate(effectPrefab));
        }
        hitParticlePool = new List<GameObject>();
        while (hitParticlePool.Count < initHitParticleNum)
        {
            GameObject obj = Instantiate(hitParticle);
            obj.GetComponent<HitParticleController>().Init(this);
            hitParticlePool.Add(obj);
        }
    }

    public GameObject PopEffect()
    {
        GameObject obj;
        if(effectPool.Count > 0)
        {
            obj = effectPool[0];
            effectPool.RemoveAt(0);
        }
        else
        {
            obj = Instantiate(effectPrefab);
        }
        obj.SetActive(true);
        return obj;
    }
    public void StartEffect(AnimationClip anim, Vector3 pos, Vector3 scale)
    {
        GameObject obj = PopEffect();
        obj.GetComponent<Effect>().SetAnim(anim, pos, scale);
    }
    public void StoreEffect(GameObject obj)
    {
        if(obj.GetComponent<Effect>())
        {
            obj.SetActive(false);
            effectPool.Add(obj);
        }
        else if(obj.GetComponent<HitParticleController>())
        {
            hitParticlePool.Add(obj);
        }
    }


    public GameObject PopHitParticle()
    {
        GameObject obj;
        if (hitParticlePool.Count > 0)
        {
            obj = hitParticlePool[0];
            hitParticlePool.RemoveAt(0);
        }
        else
        {
            obj = Instantiate(hitParticle);
            obj.GetComponent<HitParticleController>().Init(this);
        }
        return obj;
    }
    public void StartHitParticle(Vector3 pos, float angle)
    {
        GameObject obj = PopHitParticle();
        obj.GetComponent<HitParticleController>().StartParticle(pos, angle);
    }
}
