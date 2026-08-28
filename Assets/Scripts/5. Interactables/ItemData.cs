using UnityEngine;

//scriptable obj per gli oggetti che si possono collezionare
public enum ItemType
{
    Consumable, //oggetti che influenzano le stats
    Weapon, //le armi
    Equipment, //equipaggiamenti
    Artifact, //Gli artefatti per completare le missioni
    Gold, //le monete
    Miscellaneous //oggetti collezionabili utili per essere rivenduti
}

//shortcut per creare nuovi oggetti in scena
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]

public class ItemData : ScriptableObject
{
    //tutti i campi da riempire per il nostro oggetto, self explainatory
    public string itemName;
    [TextArea]
    public string description;

    public ItemType itemType;

    public Sprite icon;

    public GameObject worldPrefab;

    public int maxStack = 1;

    //dimensioni dell'oggetto nella griglia
    public int width = 1;
    public int height = 1;
}
