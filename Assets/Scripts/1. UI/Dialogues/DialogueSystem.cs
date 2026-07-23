using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//Funzionamento base dei dialoghi
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent; //il nostro testo
    public string[] lines; //array delle linee di testo
    [SerializeField] private float textSpeed; //velocità con cui scorre il testo
    private int index; //index per sapere a che punto del dialogo siamo
    private InputMap inputMap; //input map per scorrere il testo

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

    //metodo per iniziare i dialoghi
    public void StartDialogue()
    {
        textComponent.text = string.Empty; //all'inizio il dialogo è vuoto (fare check poi quando lo utilizzerò lol)
        index = 0; //partiamo dall'inizio
        StartCoroutine(TypeLine()); //e iniziamo a "typeare" il dialogo
    }

    //coroutine per il testo che scorre
    IEnumerator TypeLine() 
    {
        //per ogni lettera nella frase
        foreach (char c in lines[index].ToCharArray())
        {
            //aggiungi una lettera alla velocità da noi stabilita
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    //metodo per scorrere le frasi
    void NextLine()
    {
        //se abbiamo altre frasi nell'index, le scorriamo
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }

        //altrimenti disattiviamo il panel del dialogo
        else
        {
            gameObject.SetActive(false);
        }
    }

    void OnNextDialogue(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //il dialogo scorre quando usiamo gli input dell'inputMap
        if (inputMap.UI.Dialogue.WasPressedThisFrame())
        {
            //se abbiamo più frasi, passiamo alla prossima
            if (textComponent.text == lines[index])
            {
                NextLine();
            }

            //altrimenti ci fermiamo
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }
}