using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

//classe per l'inventario
public class Inventory : MonoBehaviour
{
    //la dimensione della griglia
    [Header("Grid Size")]
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;

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

    //metodo per aggiungere oggetti nell'inventario
    public void AddItem(ItemData itemData)
    {

    }

    //metodo per dropparli nel mondo
    public void DropItem(ItemData itemData)
    {

    }

    //metodo per quando il player acquisisce più spazio per l'inventario
    public void UpgradeInventory(int newWidth, int newHeight)
    {
        // Non permettiamo di diminuire la griglia
        if (newWidth < width || newHeight < height)
        {
            return;
        }

        width = newWidth;
        height = newHeight;
    }
}
