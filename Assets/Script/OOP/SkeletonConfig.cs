using UnityEngine;

[CreateAssetMenu(fileName = "SkeletonConfig", menuName = "PvZ/Skeleton Config")]

public class SkeletonConfig : ScriptableObject
{
    public int hp = 100;
    public float kecepatan = 2f;
    public float jarakDeteksi = 5f;
    public float jarakSerang = 1.5f;
}
