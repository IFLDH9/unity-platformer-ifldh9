using UnityEngine;


public abstract class Block : RuleTile
{
   public bool breakable;
   public Animator animation;
   public abstract void Drop(Vector3 pos);
}
