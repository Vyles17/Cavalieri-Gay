using UnityEngine;

//script per la UI dell'inventario
public class InventoryUI : MonoBehaviour
{
    //ci gettiamo le ref dei vari scripts per l'inventario
    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private RectTransform gridContainer;
    [SerializeField] private RectTransform itemsContainer;
    [SerializeField] private GameObject GridPrefab;
    [SerializeField] private GameObject itemPrefab;

    //e la dimensione degli slot della griglia e la loro spaziatura
    [Header("Grid")]
    [SerializeField] private float slotSize = 50f;
    [SerializeField] private float spacing = 2f;

    private void OnEnable()
    {
        inventory.OnInventoryChanged += RefreshUI;
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= RefreshUI;
    }
    private void Start()
    {
        //settiamo la UI all'inizio del gioco per averla aggiornata
        RefreshUI();
        RefreshGrid();
    }

    //metodo per aggiornare la UI dell'invnentario
    public void RefreshUI()
    {
        //aggiorno la dimensione del contenitore e della griglia
        UpdateContainerSize();
        RefreshGrid();

        // resetto la UI con il nuovo oggetto aggiunto, prima cancellando la vecchia UI
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        //e poi creandone una nuova con anche l'oggetto appena aggiunto
        foreach (InventorySlot slot in inventory.Items)
        {
            CreateItemUI(slot);
        }
    }

    //metodo per aggiornare la griglia dell'inventario
    private void RefreshGrid()
    {
        //cancello le vecchie celle
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        //e creo le nuove
        for (int y = 0; y < inventory.Height; y++)
        {
            for (int x = 0; x < inventory.Width; x++)
            {
                Instantiate(GridPrefab, gridContainer);
            }
        }
    }

    //metodo per creaere una nuova icona dell'oggetto appena aggiunto nella UI dell inv.
    private void CreateItemUI(InventorySlot slot)
    {
        //creo l'oggetto e getto il suo transform
        GameObject itemObject = Instantiate(itemPrefab, itemsContainer);
        RectTransform rect = itemObject.GetComponent<RectTransform>();

        //resetto la dimensione dell'oggetto in base agli slot che occupa
        float width = slot.Width * slotSize + (slot.Width - 1) * spacing;
        float height = slot.Height * slotSize + (slot.Height - 1) * spacing;
        rect.sizeDelta = new Vector2(width, height);

        //e la sua posizione nella griglia
        float x = slot.positionX * (slotSize + spacing);
        float y = slot.positionY * (slotSize + spacing);
        rect.anchoredPosition = new Vector2(x, -y);

        //imposto la sua icona e quantità
        InventoryItemUI itemUI = itemObject.GetComponent<InventoryItemUI>();
        itemUI.Setup(slot);
    }

    //metodo per allargare la griglia quando la updateremo
    private void UpdateContainerSize()
    {
        float gridWidth = inventory.Width * slotSize + (inventory.Width - 1) * spacing;
        float gridHeight = inventory.Height * slotSize + (inventory.Height - 1) * spacing;
        
        gridContainer.sizeDelta = new Vector2(gridWidth, gridHeight);
        itemsContainer.sizeDelta = new Vector2(gridWidth, gridHeight);
    }
}
