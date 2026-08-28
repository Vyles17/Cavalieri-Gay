using UnityEngine;

//sotto-classe di inventario per capire cosa possiede il player
[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity;

    //posizione dell'oggetto nella griglia
    public int positionX;
    public int positionY;

    //se l'oggetto è ruotato
    public bool rotated;
}
