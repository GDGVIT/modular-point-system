using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryItemTile : MonoBehaviour
{
    public Image tileItemImage;
    public TMP_Text tileText;

    public void setupTile(InventoryItemSO item, UnityAction<InventoryItemSO> PurchaseItem)
    {
        Button tileButton = GetComponent<Button>();
        if (tileButton != null) // Check if the button and item are not null
        {
            tileButton.onClick.AddListener(() => PurchaseItem(item));
        }
        else Debug.LogError("Button component or ShopItemScriptableIObject is null for tile at index ");

        if (tileItemImage != null && item.image != null) // Check if the image component and item image are not null
        {
            tileItemImage.sprite = item.image; // Set the item image
        }
        else Debug.LogError("Image component or item image is null for tile at index ");

        if (tileText != null)
        {
            tileText.text = item.displayName;
        }
        else Debug.LogError("Text is not given");
    }
}