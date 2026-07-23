using UnityEngine;

//scriptable object per il personaggio che parla nel dialogo

[CreateAssetMenu(fileName = "New Dialogue Character", menuName = "Dialogue/Character")]
public class DialogueCharacter : ScriptableObject
{
    public string characterName; //il suo nome
    public Sprite icon; //la sua icona
    public bool isPlayer; //check per sapere se è il CB o un NPC, se quindi sta a dx o a sx nella box di dialogo
}
