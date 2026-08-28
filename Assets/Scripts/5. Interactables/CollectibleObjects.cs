using UnityEngine;

//script da dare agli oggetti collezionabili
public class CollectibleObjects : MonoBehaviour
{
    //il campo in cui inseriamo quale scriptable obj è
    [SerializeField] private ItemData itemData;

    //e a quanta distanza possiamo interagirci
    [SerializeField] private float interactionDistance = 2f;

    //variabili pubbliche, che servono a leggere i valori da altri scripts senza modificrli
    public ItemData ItemData
    {
        get
        {
            return itemData;
        }
    }
    public float InteractionDistance
    {
        get
        {
            return interactionDistance;
        }
    }

    //metodo per raccogliere gli oggetti (se siamo abbastanza vicini)
    public virtual void Collect()
    {
        //una volta messo nell'inventario...
        //Inventory.AddItem(itemData);

        //distruggiamo l'oggetto nel mondo
        Destroy(gameObject);
    }

    //per aprire il menu azioni quando l'oggetto viene collezionato (da sistemare più avanti)
    public virtual void OnHoldClick()
    {

    }
}
