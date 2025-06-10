using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats Object", order = 0)]
public class Stats : ScriptableObject
{
    public List<StatValue> defaultStats = new List<StatValue>();
    public List<StatValue> appliedUpgrades = new List<StatValue>();

    public StatValue getStat(StatTypes stat)
    {
        foreach (StatValue item in defaultStats)
        {
            if (item.stat == stat)
            {
                foreach (StatValue upgrade in appliedUpgrades)
                {
                    if (upgrade.stat == stat)
                    {
                        item.value += upgrade.value;
                    }
                }

                return item;
            }
        }

        Debug.LogError("Not found");
        return null;
    }

    public void setStat(StatValue stat)
    {
        foreach (StatValue item in defaultStats)
        {
            if (item.stat == stat.stat)
            {
                item.value += stat.value;
            }
        }
    }

    public void unlockUpgrade(UpgradeItemEffects statUpgrade)
    {
        if (!appliedUpgrades.Contains(statUpgrade.stat)) appliedUpgrades.Add(statUpgrade.stat);
    }

    public void resetUpgrades()
    {
        appliedUpgrades.Clear();
    }

    void OnDisable()
    {
        resetUpgrades();
    }
}

public enum StatTypes
{
    hitpoints,
    speed,
    damage,
    maxhitpoints,
    attackCooldown,
}

[System.Serializable]
public class StatValue
{
    public StatTypes stat;
    public int value;
}
