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

    //larghezza dell'oggetto (considerando la rotazione)
    public int Width
    {
        get
        {
            return rotated ? item.height : item.width;
        }
    }

    //altezza dell'oggetto (considerando la rotazione)
    public int Height
    {
        get
        {
            return rotated ? item.width : item.height;
        }
    }
}
