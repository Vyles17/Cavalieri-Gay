using UnityEngine;
using UnityEngine.UI;
using TMPro;

//script per la UI degli oggetti che mettiamo nell'inventario
public class InventoryItemUI : MonoBehaviour
{
    //la sua immagine e la sua quantità
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;

    public void Setup(InventorySlot slot)
    {
        //l'icona dell'oggetto
        icon.sprite = slot.item.icon;

        //e la sua quantità
        quantityText.text = slot.quantity > 1 ? slot.quantity.ToString(): "";
    }
}
