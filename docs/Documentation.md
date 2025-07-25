
# 🧩 Project Documentation
## 📦 Project Name
**Description**:  
This unity package enables you to reduce the time required to prototype and feel out game ideas from weeks to just days. It takes away the boring and repetitive parts of game development and helps you focus on the core functionality of the game 

---

## 🗂️ Table of Contents
- [Save System](#save-system)
- [Inventory System](#inventory-system)
- [Shop System](#shop-system)
- [Upgrade System](#upgrade-system)
- [XP Management System](#xp-management-system)
- [Powerup System](#power-up-system)
- [Stats Class](#stats-class)
- [Projectile Gun System](#projectile-gun-system)

---

# Save System

#### Overview
This system allows saving any object by creating a custom class to store its data and implements the `ISaveFuncs` interface using JSON serialization.

#### Key Components
- `ISaveFuncs`: Interface with `SaveData()` and `LoadData(...)`
- `SaveEntry`: Struct storing `id`, `type`, `jsonData`
- `SaveManager`: Handles saving/loading all registered objects
- Uses: `JsonUtility` or `Newtonsoft.Json`

#### Example Usage
```csharp
public class PlayerStats : MonoBehaviour, ISaveFuncs
{
    public string id => "player_stats";
    int health = 80;

    void ISaveFuncs.LoadData(object data)
    {
        if (data is PlayerData d)
        {
            health = d.health;
        }
    }

    object ISaveFuncs.SaveData()
    {
        return new PlayerData
        {
            health = health;
        };
    }
    
    class PlayerData{
		int health;
	}
}
```

---

# Inventory System

#### Overview
This system allows addition, equip items as well as display the items currently held. It currently allows only storing scriptable objects.
#### Key Components
- `IInventoryItem` : Interface to make a object storable in the inventory with functions like `EquipItem` and `AddItem`
- `Inventory` : Base class to handle all the interactions between the user and the inventory
- `InventoryUIBehaviour` : Script to create the inventory UI during runtime based on the number of objects in the inventory
- `InventoryItemTile` : Script to initialize custom UI tile used to display information of item in the inventory slot

### Example Usage
```csharp
public class gunObject : InventoryItemSO
{
    string InventoryItemSO.displayName { get => Name; }
    Sprite InventoryItemSO.image{ get => sprite; }
    int InventoryItemSO.quantity{ get => quantity; }

    string Name;
    public Sprite sprite;
    public int quantity;

	public void EquipItem()
    {
        if (Player.playerInstance != null)
        {
            Player.playerInstance.gun.activeGun = this;
            Player.playerInstance.gun.SetUpGun();
        }
    }
  
    public void AddItem()
    {
        if (Inventory.instance != null)
        {
            Inventory.instance.objects.Add(this);
        }
    }
}
```


---

## Shop System

### Key Components
- `ShopItemScriptableIObject`: Scriptable object to store all the information of a shop item
- `ShopItemTile` : Script to initialize the custom UI tile of shop items
- `ShopManager` : Base class which handles the initialization of the UI and handles purchasing of the items

### Example Usage of item effects for shop items
```csharp
public class ColorItemFunction : ItemEffectBase
{
    public Material player;
    public override void ApplyEffect(object obj)
    {
        if (obj is ShopItemScriptableIObject item)
            player.SetColor("_BaseColor", item.itemImageTint);
    }

    public override void RemoveEffect(object obj)
    {
        //Can add code to remove the effect
    }
}
```

### Example of a Shop Item Scriptable Object

![Upgrade Example](docs/images/upgrade.png)


---

## Upgrade System

### Overview
This system allows you to create and apply upgrades permanently (by integrating with save system) as well as keep it temporary. It also allows you to randomize (which can be modified to use the weight random class) as well as display a selected few upgrades every time.

### Key Components
- `UpgradeItem` : Local instance of the upgrade which holds the `isPurchased` bool and number of times the upgrade can be bought
- `UpgradeItemScriptableIObject` : Stores the information regarding the upgrade and a list of `itemEffects` which should be applied
- `UpgradeManager` : Manages the purchasing of upgrades and can be used to increase functionality of the system
- `UpgradeUIBehaviour` : Script to create the upgrades UI during runtime based on the number of objects in the list
- `UpgradeItemTile` : Script to initialize custom UI tile used to display information the upgrade

### Example
- This example shows a damage increase upgrade which applies a increase in damage to all `Stats` in the list.
```csharp
[CreateAssetMenu(menuName = "Upgrade Effects/Damage Increase", order = 0)]
public class DamageIncreaseEffect : UpgradeItemEffects
{
    public StatValue stat; // The stat to be affected by this upgrade
    public Stats[] statsToAffect; // Array of stats to be affected by this upgrade
    public override void Apply(object item)
    {
	    if(item is UpgradeItem item)
        {
	        foreach (Stats Stat in statsToAffect)
	        {
	            Stat.unlockUpgrade(new StatValue(StatTypes.damage, stat.value));
	        }
	    }
    }
}
```
### Example of Scriptable Object

![Shop Example](docs/images/shop.png)

---

## Power-Up System

### Key Components
- `PowerUpManager` : Manages all the power-ups in the scene from methods like `SpawnPowerUp`,`ActivatePowerUp` and `RemovePowerUp`
- `PowerUpPickableItem` : Script to check for collisions and call the event `OnItemPick`
- `PowerUpScriptableObject` : Scriptable object to store information about the power-up including a list of `itemEffects`
### Example
- This example shows a power-up which activates a `bool` which controls if the gun had infinite ammo or not
```csharp
[CreateAssetMenu(fileName = "Infinte Ammo", menuName = "PowerUp Effects/Infinte Ammo Effect")]
public class infinteAmmo : PowerUpEffects
{
    public override void ApplyEffect(object powerUp)
    {
	    if(powerUp is PowerUpScriptableObject item)
        {
	        if (Player.playerInstance != null)
	        {
	            Player.playerInstance.gun.activeGun.infiniteAmmo = true;
	        }
		}
    }

    public override void RemoveEffect(object powerUp)
    {
	    if(powerUp is PowerUpScriptableObject item)
	    {
	        if (Player.playerInstance != null)
	        {
	            Player.playerInstance.gun.activeGun.infiniteAmmo = false;
		    }
	    }
    }
}
```
---

## Stats Class
### Overview
- Uses a Enum `StatTypes` which holds all the stats available in your game
- Using a custom class `StatValue` which holds the enum of the stat and the value of that particular stat
- Using two lists of `StatValue`
	- `defaultStats` : Holds the stats which the object has by default
	- `appliedUpgrades` : Holds the upgrades attached to the stats object during runtime
- Using the `unlockUpgrade` we can add upgrades to the stats class during runtime 

---
## Projectile Gun System
### Overview
- Uses the `GunIScriptableObject` which inherits from the stats class
	- These scriptable objects holds all the information about the gun
	- Other properties regarding the `fireRate` , `damage` can be modified by modifying the respective StatValue property
- The `bullet` script is used to define the life span of a given bullet
- The `GunManager` script handles/holds the current `GunObject` the player holds
	- It has functions like `Shoot` and `isReloading` to check the basic functionalities of a gun

### Example of a Gun Scriptable Object
![Gun Example](docs/images/gun.png)