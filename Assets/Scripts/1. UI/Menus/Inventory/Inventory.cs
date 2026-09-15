using System.Collections.Generic;
using UnityEngine;

//classe per l'inventario e la sua griglia
public class Inventory : MonoBehaviour
{
    //la dimensione della griglia
    [Header("Grid Size")]
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;

    //e gli oggetti che contiene
    [Header("Items")]
    [SerializeField] private List<InventorySlot> items = new List<InventorySlot>();

    //variabili che si possono prendere dagli altri script senza che li possano modificare
    public int Width
    {
        get
        {
            return width;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
    }

    public List<InventorySlot> Items
    {
        get
        {
            return items;
        }
    }

    //evento per aggiornare la UI
    public System.Action OnInventoryChanged;

    //metodo per aggiungere oggetti nell'inventario e in quale quantità (è true se avevamo effettivamente spazio per aggiungerlo)
    public int AddItem(ItemData itemData, int quantity = 1)
    {
        //controlliamo che l'oggetto esista e che la quantità da aggiungere sia valida
        if (itemData == null || quantity <= 0)
            return 0;

        //teniamo traccia di quanti oggetti ci rimangono ancora da aggiungere
        int remaining = quantity;

        //proviamo a stockare l'oggetto, se esiste già una "pila" di quel tipo di oggetto e non è piena
        if (itemData.maxStack > 1)
        {
            //controlliamo tutti gli oggetti dell'inventario
            foreach (InventorySlot slot in items)
            {
                //se questo slot contiene un oggetto diverso, passiamo al prossimo
                if (slot.item != itemData)
                    continue;

                //se lo stack è già pieno, passiamo al prossimo
                if (slot.quantity >= itemData.maxStack)
                    continue;

                //calcoliamo quanti oggetti possono ancora entrare in questo stack
                int space = itemData.maxStack - slot.quantity;

                //aggiungiamo gli oggetti allo stack
                int amountToAdd = Mathf.Min(space, remaining);
                slot.quantity += amountToAdd;

                //diminuiamo il numero di oggetti che dobbiamo ancora aggiungere
                remaining -= amountToAdd;

                //aggiorniamo la UI
                OnInventoryChanged?.Invoke();

                //se non ci rimane più niente da aggiungere, abbiamo finito
                if (remaining <= 0)
                    return quantity;
            }
        }

        //continuiamo finché abbiamo ancora oggetti da inserire
        while (remaining > 0)
        {
            //bool per capire se in questa ricerca siamo riusciti a trovare una posizione libera
            bool added = false;

            //controlliamo ogni posizione della griglia
            for (int y = 0; y < height && !added; y++)
            {
                for (int x = 0; x < width && !added; x++)
                {
                    //all'inizio proviamo con l'orientamento originale
                    if (CanPlaceItem(itemData, x, y, false))
                    {
                        //calcoliamo quanti oggetti mettere in questo nuovo stack senza mai superare il maxStack
                        int amountToAdd = Mathf.Min(itemData.maxStack, remaining);

                        //creiamo un nuovo slot
                        InventorySlot newSlot = new InventorySlot();

                        newSlot.item = itemData;
                        newSlot.quantity = amountToAdd;
                        newSlot.positionX = x;
                        newSlot.positionY = y;
                        newSlot.rotated = false;

                        //aggiungiamo il nuovo slot alla lista dell'inventario
                        items.Add(newSlot);

                        //togliamo gli oggetti aggiunti da quelli ancora da aggiungere
                        remaining -= amountToAdd;

                        //abbiamo trovato uno spazio
                        added = true;

                        //usciamo dal ciclo
                        break;
                    }

                    //se l'oggetto è ruotabile, proviamo anche ruotato
                    if (itemData.width != itemData.height && CanPlaceItem(itemData, x, y, true))
                    {
                        //calcoliamo quanti oggetti mettere nel nuovo stack
                        int amountToAdd = Mathf.Min(itemData.maxStack, remaining);

                        //creiamo il nuovo slot
                        InventorySlot newSlot = new InventorySlot();

                        newSlot.item = itemData;
                        newSlot.quantity = amountToAdd;
                        newSlot.positionX = x;
                        newSlot.positionY = y;
                        newSlot.rotated = true;

                        //aggiungiamo lo slot all'inventario
                        items.Add(newSlot);

                        //aggiorniamo la quantità ancora da inserire
                        remaining -= amountToAdd;

                        //abbiamo trovato uno spazio
                        added = true;

                        //usciamo dal ciclo
                        break;
                    }
                }
            }

            //se dopo aver controllato tutta la griglia non abbiamo trovato nessuno spazio libero, amen
            if (!added)
            {
                break;
            }
        }

        //aggiunti = quantità iniziale - quantità rimasta
        //(ricordarsi di mettere nel metodo del "raccogliere" il fatto che i restanti restanofuori) 
        return quantity - remaining;
    }


    //bool per controllare se un oggetto può essere inserito nella posizione indicata
    public bool CanPlaceItem(ItemData itemData, int positionX, int positionY, bool rotated, InventorySlot itemToIgnore = null)
    {
        int itemWidth = rotated ? itemData.height : itemData.width;
        int itemHeight = rotated ? itemData.width : itemData.height;

        //controlliamo che l'oggetto non sfoci oltre la griglia
        if (positionX < 0 || positionY < 0)
            return false;

        if (positionX + itemWidth > width)
            return false;

        if (positionY + itemHeight > height)
            return false;

        //e checkiamo tutti gli slot che l'oggetto occuperebbe
        for (int x = positionX; x < positionX + itemWidth; x++)
        {
            for (int y = positionY; y < positionY + itemHeight; y++)
            {
                //se è occupato, allora ritorna false
                if (IsSlotOccupied(x, y, itemToIgnore))
                {
                    return false;
                }
            }
        }

        //se è tutto ok, possiamo aggiungerlo
        return true;
    }

    //bool per controllare se lo slot è già occupato
    private bool IsSlotOccupied(int x, int y, InventorySlot itemToIgnore = null)
    {
        //controllo su tutti gli slots
        foreach (InventorySlot slot in items)
        {
            //ignoriamo l'oggetto che stiamo eventualmente spostando
            if (slot == itemToIgnore)
                continue;

            //controlliamo i limiti dell'oggetto
            int startX = slot.positionX;
            int endX = slot.positionX + slot.Width;

            int startY = slot.positionY;
            int endY = slot.positionY + slot.Height;

            //lo slot è dentro l'area occupata?
            if (x >= startX && x < endX && y >= startY && y < endY)
            {
                //allora c'è già un ogetto lì
                return true;
            }
        }

        //sennò è libero
        return false;
    }

    //bool per spostare l'oggetto negli slot all'interno dell'inventario
    public bool MoveItem(InventorySlot slot, int newX, int newY)
    {
        //se non c'è uno slot o siamo su uno slot di un menù diverso, è falso
        if (slot == null || !items.Contains(slot))
            return false;

        //se non ci sta, non si può spostare
        if (!CanPlaceItem(slot.item, newX, newY, slot.rotated, slot))
        {
            return false;
        }

        //sennò gli assegniamo la nuova posizoine
        slot.positionX = newX;
        slot.positionY = newY;

        //aggiorno la UI
        OnInventoryChanged?.Invoke();

        return true;
    }

    //bool per girare un oggetto di 90°
    public bool RotateItem(InventorySlot slot)
    {
        //se non c'è uno slot o siamo su uno slot di un menù diverso, è falso
        if (slot == null || !items.Contains(slot))
            return false;

        //gli oggetti quadrati non cambiano dimensione e non hanno bisogno di essere ruotati.
        if (slot.item.width == slot.item.height)
            return true;

        bool newRotation = !slot.rotated;

        //controlliamo che l'oggetto ci stia nella posizione attuale
        if (!CanPlaceItem(slot.item, slot.positionX, slot.positionY, newRotation, slot))
        {
            return false;
        }

        slot.rotated = newRotation;

        //aggiorno la UI
        OnInventoryChanged?.Invoke();

        return true;
    }

    //metodo per dropparli nel mondo, e in quale quantità
    public bool DropItem(InventorySlot slot, int amount = 1)
    {
        //se non c'è uno slot o siamo su uno slot di un menù diverso, è falso
        if (slot == null || !items.Contains(slot))
            return false;

        //rimuoviamo l'oggetto dall'inventario
        //(poi aggiungere qui la cosa di ripiazzarli nel mondo)!!!!!!
        items.Remove(slot);

        //aggiorno la UI
        OnInventoryChanged?.Invoke();

        return true;
    }

    //bool da usare quando rimuoviamo un oggetto dall'inventario (perchè lo mangiamo/vendiamo ecc) e in quale quantità
    public bool RemoveItem(InventorySlot slot, int amount = 1)
    {
        //se non c'è uno slot o siamo su uno slot di un menù diverso, è falso
        if (slot == null || !items.Contains(slot))
            return false;

        //checckini per non andare sotto lo zero nelle quantità
        if (amount <= 0 || amount > slot.quantity)
            return false;

        //rimuoviamo la quantità richiesta
        slot.quantity -= amount;

        //se lo stack è arrivato a zero, non ha più senso mantenere questo InventorySlot
        if (slot.quantity == 0)
            items.Remove(slot);

        //aggiorno la UI
        OnInventoryChanged?.Invoke();

        //la rimozione è avvenuta correttamente
        return true;
    }

    //metodo per gettarci la reale quantità di oggetti che abbiamo dello stesso tipo, anche se sono in pile diverse
    public int GetItemQuantity(ItemData itemData)
    {
        //partiamo da zero
        int totalQuantity = 0;

        //e iniziamo a contare negli slot le quantità
        foreach (InventorySlot slot in items)
        {
            if (slot.item == itemData)
            {
                totalQuantity += slot.quantity;
            }
        }

        return totalQuantity;
    }

    //metodo per quando il player acquisisce più spazio per l'inventario
    public void UpgradeInventory(int newWidth, int newHeight)
    {
        //non si può diminuire la griglia
        if (newWidth < width || newHeight < height)
        {
            return;
        }

        width = newWidth;
        height = newHeight;

        //per aggiornare la UI
        OnInventoryChanged?.Invoke();
    }
}
