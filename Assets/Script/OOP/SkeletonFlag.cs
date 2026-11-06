using UnityEngine;

public class SkeletonFlag : Enemy
{
    public bool flag = true;

    public override void Serang()
    {
        Debug.Log("Skeleton menyerang dengan bendera!");
        base.Serang();
    }
}
