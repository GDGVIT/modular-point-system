using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UpgradeItemTile : MonoBehaviour
{
    public Image tileItemImage;
    public TMP_Text tileText;

    public void setupTile(UpgradeItem item, UnityAction<UpgradeItem> PurchaseItem)
    {
        Button tileButton = GetComponent<Button>();
        if (tileButton != null) // Check if the button and item are not null
        {
            tileButton.onClick.AddListener(() => PurchaseItem(item));
        }
        else Debug.LogError("Button component or UpgradeItem is null for tile at index ");

        if (tileItemImage != null && item.data.itemImage != null) // Check if the image component and item image are not null
        {
            tileItemImage.sprite = item.data.itemImage; // Set the item image
            tileItemImage.color = item.data.itemImageTint; // Set the item image tint color
        }
        else Debug.LogError("Image component or item image is null for tile at index ");

        if (tileText != null) // Check if the text component are not null
        {
            tileText.text = item.data.itemPrice.ToString(); // Set the item price text
        }
        else Debug.LogError("Text component or item price is null for tile at index ");
    }
}
