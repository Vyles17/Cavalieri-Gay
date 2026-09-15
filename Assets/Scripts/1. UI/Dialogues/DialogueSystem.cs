using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Funzionamento base dei dialoghi
public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue")]
    public TextMeshProUGUI textComponent; //il nostro testo
    public DialogueLine[] lines; //array delle linee di testo
    [SerializeField] private float textSpeed = 0.05f; //velocità con cui scorre il testo

    [Header("Player UI")]
    [SerializeField] private GameObject player;
    [SerializeField] private Image playerIcon;
    [SerializeField] private TextMeshProUGUI playerName;

    [Header("NPC UI")]
    [SerializeField] private GameObject npc;
    [SerializeField] private Image npcIcon;
    [SerializeField] private TextMeshProUGUI npcName;

    private int index; //index per sapere a che punto del dialogo siamo
    private InputMap inputMap; //input map per scorrere il testo
    private Coroutine typingCoroutine; //coroutine per il testo che viene scorso

    private void Awake()
    {
        //ci gettiamo l'inputMap
        inputMap = new InputMap();
    }
    private void OnEnable()
    {
        inputMap.Enable();
        inputMap.UI.Dialogue.performed += OnNextDialogue;
    }

    private void OnDisable()
    {
        inputMap.UI.Dialogue.performed -= OnNextDialogue;
        inputMap.Disable();
    }

    //metodo per preparare i dialoghi
    public void StartDialogue()
    {
        if (lines.Length == 0)
            return;

        GameManager.Instance.SetGameStatus(GameStatus.Dialogue);

        index = 0; //partiamo dall'inizio
        StartCurrentLine(); //e iniziamo a "typeare" il dialogo
    }

    //metodo per iniziare il dialogo effettivo
    void StartCurrentLine()
    {
        UpdateDialogueUI(); //aggiorno la UI
        textComponent.text = string.Empty;  //all'inizio il dialogo è vuoto (fare check poi quando lo utilizzerò lol)

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine); //checkino che sia effettivamente tutto fermo all'inizio

        typingCoroutine = StartCoroutine(TypeLine()); //così possiamo iniziare la coroutine
    }

    //coroutine per il testo che scorre
    IEnumerator TypeLine()
    {
        //per ogni lettera nella frase
        foreach (char c in lines[index].text)
        {
            //aggiungi una lettera alla velocità da noi stabilita
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        typingCoroutine = null; //e la fermiamo
    }

    //metodo per scorrere le frasi
    void NextLine()
    {
        //se abbiamo altre frasi nell'index, le scorriamo
        if (index < lines.Length - 1)
        {
            index++;
            StartCurrentLine();
        }

        //altrimenti disattiviamo il panel del dialogo e riattiviamo il moviemnto del player
        else
        {
            GameManager.Instance.SetGameStatus(GameStatus.Running);

            gameObject.SetActive(false);
        }
    }

    //metodo per aggiornare la UI in base se chi parla è a DX o a SX del panel
    void UpdateDialogueUI()
    {
        DialogueCharacter character = lines[index].character;

        //se è il protagonista, lo attivo e disattivo l'icona e il nome a DX
        if (character.isPlayer)
        {
            player.SetActive(true);
            npc.SetActive(false);

            playerName.text = character.characterName;
            playerIcon.sprite = character.icon;
        }

        //sennò faccio il contrario
        else
        {
            player.SetActive(false);
            npc.SetActive(true);

            npcName.text = character.characterName;
            npcIcon.sprite = character.icon;
        }
    }

    //metodo per passare al prossimo testo
    void OnNextDialogue(InputAction.CallbackContext context)
    {
        //se non premiamo i tasti dell'inputmap, non succede niente
        if (!inputMap.UI.Dialogue.WasPressedThisFrame())
            return;

        //se c'è una coroutine in corso, la fermiamo, e visualizziamo il testo per intero
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            textComponent.text = lines[index].text;
            return;
        }

        //sennò andiamo alla prossima riga
        NextLine();
    }
}