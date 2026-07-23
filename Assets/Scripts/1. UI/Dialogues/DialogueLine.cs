using UnityEngine;

//script per le battute di chi parla nel dialogo
[System.Serializable]
public class DialogueLine 
{
    public DialogueCharacter character;

    [TextArea(2, 5)] //ritornarci dopo
    public string text;
}
