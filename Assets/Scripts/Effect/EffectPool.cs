using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : Singleton<EffectPool>
{
    public int initEffectNum = 10;
    public GameObject effectPrefab;
    public List<GameObject> pool;
    void Start()
    {
        pool = new List<GameObject>();
        while(pool.Count < initEffectNum)
        {
            pool.Add(Instantiate(effectPrefab));
        }
    }

    public GameObject PopEffect()
    {
        GameObject obj;
        if(pool.Count > 0)
        {
            obj = pool[0];
            pool.RemoveAt(0);
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
            pool.Add(obj);
        }
    }

}
