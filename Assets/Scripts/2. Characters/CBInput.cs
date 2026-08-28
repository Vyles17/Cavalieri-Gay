using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

//Script che ci servirà per tutti gli input del cavaliere bianco dati dal player
public class CBInput : MonoBehaviour
{
    //gettiamo l'input map del player
    private PlayerInput playerInput;

    //e le sue azioni
    private InputAction walkAction;
    private InputAction runAction;

    //reference per il click-to-move
    [Header("Click Movement")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask interactableLayer;
    private Vector3 clickTarget;
    private bool hasClickTarget;

    //il collezionabile verso cui ci muoviamo
    private CollectibleObjects collectibleTarget;

    //gestione dei click del mouse
    private float mouseHoldTime = 0f;
    private bool isHoldingMouse = false;

    //valori che ci prendiamo dallo script CBMovement per muovere il personaggio
    public Vector3 MoveDirection { get; private set; }
    public bool IsRunning { get; private set; }


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        //getto le Action Map, se esistono e hanno questo nome preciso
        walkAction = playerInput.actions.FindAction("Walk");
        runAction = playerInput.actions.FindAction("Run");
    }


    private void Update()
    {
        //per far si che il CB non si muova quando il gioco è in pausa
        if (GameManager.Instance.status != GameStatus.Running)
        {
            MoveDirection = Vector3.zero;
            return;
        }

        bool isUsingKeyboard = ReadKeyboard();
        ReadMouse();
        ReadClickMovement(isUsingKeyboard);
    }

    //bool per il movimento tramite WASD:
    private bool ReadKeyboard()
    {
        //l'input di movimento che verrà letto e settato
        Vector2 input = walkAction.ReadValue<Vector2>();

        //valori nello spazio X,Y per il movimento con WASD
        Vector3 keyboardMove = new Vector3(input.x, 0f, input.y);

        //se premiamo il tasto per correre, correrà
        IsRunning = runAction.IsPressed();

        //il  movimento con WASD ha priorità sul clickto-move
        if (keyboardMove.magnitude > 0.1f)
        {
            MoveDirection = keyboardMove;
            //se iniziamo a muoverci con WASD, ci dimentichiamo della posizione/oggetto che stavamo raggiungendo
            hasClickTarget = false;
            collectibleTarget = null;

            //se stiamo facendo questo processo, allora il bool è true
            return true;
        }

        //sennò è false
        return false;

    }

    //metodo per il movimento col puntatore:
    private void ReadMouse()
    {
        //muoversi con un solo click...
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            mouseHoldTime = 0f;
            isHoldingMouse = true;
            HandleClick();
        }


        //...o tenendo premuto/trascinando il cursore
        if (Mouse.current.leftButton.isPressed && isHoldingMouse)
        {
            mouseHoldTime += Time.deltaTime;

            //dopo una piccola soglia consideriamo il movimento come "mouse tenuto premuto"
            if (mouseHoldTime >= 0.3f)
            {
                HandleClick();
            }
        }

        //quando rilasciamo il cursore
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            //se stavamo tenendo premuto il mouse, cancelliamo il movimento
            if (mouseHoldTime >= 0.3f)
            {
                hasClickTarget = false;
                MoveDirection = Vector3.zero;
            }

            isHoldingMouse = false;
            mouseHoldTime = 0f;
        }

    }

    //metodo per gestire il movimento verso un target
    private void ReadClickMovement(bool isUsingKeyboard)
    {
        // WASD ha la priorità sul click-to-move
        if (isUsingKeyboard)
            return;

        //se non abbiamo cliccato un target, sta fermo
        if (!hasClickTarget)
        {
            MoveDirection = Vector3.zero;
            return;
        }

        //settiamo la direzione
        Vector3 direction = clickTarget - transform.position;
        direction.y = 0;

        //se siamo arrivati al target, ci fermiamo
        if (direction.magnitude <= 0.1f)
        {
            hasClickTarget = false;
            MoveDirection = Vector3.zero;

            //se abbiamo cliccato su un oggetto collezionabile, lo raccogliamo
            if (collectibleTarget != null)
            {
                collectibleTarget.Collect();
                collectibleTarget = null;
            }
        }

        //sennò lo raggiungiamo
        else
        {
            MoveDirection = direction;
        }

    }

    //metodo raycastino per vedere cosa stiamo cliccando
    private void HandleClick()
    {
        //se il mouse è sopra la UI, ignoriamo il click
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //creiamo il ray dalla camera verso il puntatore
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        //se non colpiamo niente col raycast, non succede niente
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
            return;

        //se nel nostro raggio colpiamo un collectible...
        CollectibleObjects collectible = hit.collider.GetComponent<CollectibleObjects>();

        if (collectible != null)
        {
            //salviamo l'oggetto che abbiamo cliccato
            collectibleTarget = collectible;

            //calcoliamo la distanza 
            float distance = Vector3.Distance(transform.position, collectible.transform.position);

            //se siamo già abbastanza vicini, lo raccogliamo direttamente
            if (distance <= collectible.InteractionDistance)
            {
                hasClickTarget = false;
                MoveDirection = Vector3.zero;

                collectibleTarget.Collect();
                collectibleTarget = null;
                return;
            }

            //se siamo lontani, calcoliamo il punto in cui fermarci
            Vector3 directionToCollectible = collectible.transform.position - transform.position;

            directionToCollectible.y = 0f;

            clickTarget = collectible.transform.position - directionToCollectible.normalized * collectible.InteractionDistance;

            hasClickTarget = true;

            return;
        }

        //se abbiamo cliccato il terreno, ci spostiamo
        if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
        {
            clickTarget = hit.point;
            hasClickTarget = true;
            // e segniamo che non stiamo andando verso un collectible
            collectibleTarget = null;
        }
    }
}
