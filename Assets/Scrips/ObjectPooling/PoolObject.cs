using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    public string PoolID { get; set; }
    public bool IsBeingUsed { get; protected set; }

    public virtual void Initialize(object poolManager = null)
    {
        Deactivate();
    }

    public virtual void EnableObject()
    {
        IsBeingUsed = true;
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        IsBeingUsed = false;
        gameObject.SetActive(false);
    }

    public abstract void ReturnToPool();
}