using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradeItem> AllShopItems; // List of shop items which are available in the sho
    public int playerCurrency;// Player's current currency amount
    public static UpgradeManager Instance { get; private set; } // Singleton instance of the UpgradeManager

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PurchaseItem(UpgradeItem item)
    {
        if (item.data.itemQuantity <= 0)
        {
            Debug.Log("Item is out of stock.");
            if (item.isPurchased)
            {
                Debug.Log("Item is already purchased but out of stock.");
                foreach (UpgradeItemEffects effect in item.effects)
                {
                    effect.Apply(item); // Invoke all the effects associated with the item
                }
            }

            return;
        }
        
        if (item.isPurchased)
        {
            Debug.Log("Item is already purchased and out of stock.");
            return;
        }
        
        // Assuming we have a method to check player's currency
        if (item.data.itemPrice <= playerCurrency)
        {
            item.isPurchased = true;// Mark the item as purchased
            playerCurrency -= item.data.itemPrice; // Deduct the item's price from player's currency
            item.data.itemQuantity--;// Decrease the item's quantity

            Debug.Log($"Purchased {item.data.itemName} for {item.data.itemPrice} coins.");

            foreach (UpgradeItemEffects effect in item.effects)
            {
                effect.Apply(item); // Invoke all the effects associated with the item
            }
        }
        else
        {
            Debug.Log("Not enough currency to purchase this item.");
        }
    }
}