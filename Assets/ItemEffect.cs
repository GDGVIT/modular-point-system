using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public abstract void ApplyEffect(object obj);
    public abstract void RemoveEffect(object obj);
}
