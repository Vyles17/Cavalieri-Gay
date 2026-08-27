using UnityEngine;

//scriptino per il manager di tutte le cose dell'HUD
public class HUDManager : MonoBehaviour
{
    //l'oggetto dell'inventario
    [SerializeField] private GameObject inventory;
    //evento da richiamare quando apriamo/chiudiamo l'inventario
    public static System.Action<bool> OnInventoryChanged;
    //e il suo bool per sapere se è aperto
    private bool inventoryOpen = false;

    void Start()
    {
        //all'inizio l'inventario è nascosto
        if (inventory != null)
            inventory.SetActive(false);
    }

    //funzione per aprire l'inventari
    public void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;

        if (inventory != null)
            inventory.SetActive(inventoryOpen);

        OnInventoryChanged?.Invoke(inventoryOpen);
    }
}
