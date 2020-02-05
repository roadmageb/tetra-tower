public enum LifeStoneType
{
    NULL, Normal, Gold
}

public enum InputAction
{
    Action1, Action2, Action3, NULL
}

public enum InputArrow
{
    Up, Down, Front, UpFront, DownFront, NULL
}

public enum ItemRank
{
    Monomino, Domino, Tromino, Tetromino, Pentomino
}

public enum PosCond
{
    None, Ground, Midair
}

public enum DirCond
{
    None, Front, Up, Down, UpDiag, DownDiag, Neutral
}

public enum KeyCond
{
    Z=1,
    X=2,
    ZX=3,
    C=4,
    ZC=5,
    XC=6,
    ZXC=7
}

public enum CtrlPtoE
{
    Stun, Burn, Freeze
}

public enum CtrlEtoP
{
    Slow, KeyLeft, KeyRight, KeyDown, KeySpc, Paralysis
}
