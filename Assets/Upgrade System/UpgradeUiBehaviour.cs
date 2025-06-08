using UnityEngine;

public class UpgradeUiBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject upgradeUiTile;// Prefab for each shop item tile
    [SerializeField] private Transform tileParent; // Parent transform where shop tiles will be instantiated and the content of the scroll rect
    [SerializeField] private bool randomize = false;
    [SerializeField] private int count = 0; // Number of items to display in the shop UI

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if(randomize)
        {
            // Shuffle the items in AllShopItems list
            for (int i = 0; i < count; i++)
            {
                GameObject tile = Instantiate(upgradeUiTile, tileParent);
                if (tile != null)//Null check to avoid null pointer error
                {
                    int a = Random.Range(0, UpgradeManager.Instance.AllShopItems.Count);
                    if (UpgradeManager.Instance.AllShopItems[a] != null)
                    {
                        tile.GetComponent<UpgradeItemTile>().setupTile(UpgradeManager.Instance.AllShopItems[a], UpgradeManager.Instance.PurchaseItem);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < UpgradeManager.Instance.AllShopItems.Count; i++)
            {
                GameObject tile = Instantiate(upgradeUiTile, tileParent);
                if (tile != null)//Null check to avoid null pointer error
                {
                    if (UpgradeManager.Instance.AllShopItems[i] != null)
                    {
                        tile.GetComponent<UpgradeItemTile>().setupTile(UpgradeManager.Instance.AllShopItems[i], UpgradeManager.Instance.PurchaseItem);
                    }
                }
            }
        }
    }

    void OnDisable()
    {
        // Clear the tiles when the UI is disabled
        foreach (Transform child in tileParent)
        {
            Destroy(child.gameObject);
        }
    }
}
