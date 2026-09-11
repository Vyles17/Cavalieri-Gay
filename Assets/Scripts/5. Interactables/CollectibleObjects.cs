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
    public virtual void Collect(Inventory inventory)
    {
        //utilizzo il metodo della classe Inventory per aggiungere la quantità scelta
        int addedAmount = inventory.AddItem(itemData, 1);

        //se raccolgo tutta la quantità di oggeti presente, cancella il game obj dalla scena
        if (addedAmount > 0)
        {
            Destroy(gameObject);
        }
    }
}
