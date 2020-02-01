using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand
{
    public PosCond posCond;
    public DirCond dirCond;
    public KeyCond[] keyCond;
    public SkillCommand(PosCond posCond, DirCond dirCond, KeyCond[] keyCond)
    {
        this.posCond = posCond;
        this.dirCond = dirCond;
        this.keyCond = (KeyCond[])keyCond.Clone();
    }
}
