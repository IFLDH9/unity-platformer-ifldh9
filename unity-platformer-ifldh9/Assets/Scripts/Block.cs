using UnityEngine;

public abstract class Block : RuleTile
{
    public bool breakable { get; set; }
    public Animator animation { get; set; }
    public abstract void Drop(Vector3 pos);
}
