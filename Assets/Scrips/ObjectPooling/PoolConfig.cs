using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "Pools/PoolConfiguration")]
public class PoolConfig : ScriptableObject
{
    public PoolObject prefab;
    public int initialSize = 10;
    public bool expandable = false;
}
