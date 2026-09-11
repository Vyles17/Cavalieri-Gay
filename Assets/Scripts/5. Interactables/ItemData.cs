using System.Collections.Generic;
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

//enum delle azioni disponibili pe rgli oggetti 
public enum ItemAction
{
    Eat,
    Use,
    Equip,
    Examine,
    Drop
}

//shortcut per creare nuovi oggetti in scena
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]

public class ItemData : ScriptableObject
{
    //tutti i campi da riempire per il nostro oggetto, self explainatory
    [Header("Info")]
    public string itemName;
    [TextArea]
    public string description;
    public ItemType itemType;
    public Sprite icon;
    public GameObject worldPrefab;
    public int maxStack = 1;

    //le azioni che potrà sbloccare quando ci clicchi sopra
    [Header("Actions")]
    public List<ItemAction> actions = new List<ItemAction>();

    //dimensioni dell'oggetto nella griglia
    [Header("Grid")]
    public int width = 1;
    public int height = 1;

}
