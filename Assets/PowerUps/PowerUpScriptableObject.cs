using UnityEngine;

public class PowerUpScriptableObject : ScriptableObject
{
    public GameObject powerUpPrefab;
    public string powerUpName;
    public string description;
    public float duration;
    public ItemEffect[] effects;
}
