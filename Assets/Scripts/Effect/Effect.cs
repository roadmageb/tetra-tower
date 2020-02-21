using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public void SetAnim(AnimationClip anim, Vector3 pos, Vector3 scale)
    {
        Animator animator = GetComponent<Animator>();
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        aoc["Effect"] = anim;
        animator.runtimeAnimatorController = aoc;
        transform.position = pos;
        transform.localScale = scale;
        animator.SetTrigger("start");
    }

    public void StoreThis()
    {
        EffectPool.Instance.StoreEffect(gameObject);
    }
}
