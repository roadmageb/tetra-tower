using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private StringAnim[] skillAnimSerialize;
    [SerializeField]
    private StringAnim[] effectAnimSerialize;
    public Dictionary<string, AnimationClip> skillAnim;
    public Dictionary<string, AnimationClip> effectAnim;


    private void Awake()
    {
        skillAnim = new Dictionary<string, AnimationClip>();
        effectAnim = new Dictionary<string, AnimationClip>();
        foreach(StringAnim s in skillAnimSerialize)
        {
            skillAnim.Add(s.name, s.anim);
        }
        foreach (StringAnim s in effectAnimSerialize)
        {
            effectAnim.Add(s.name, s.anim);
        }
    }
}
