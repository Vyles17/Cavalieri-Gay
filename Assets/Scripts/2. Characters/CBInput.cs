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
    private Vector3 clickTarget;
    private bool hasClickTarget;
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

        ReadKeyboard();
        ReadMouse();
    }

    //metodo per il movimento tramite WASD:
    private void ReadKeyboard()
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
            hasClickTarget = false;
        }

        //sennò esegue il click-to-move
        else if (hasClickTarget)
        {
            //settiamo la direzione
            Vector3 direction = clickTarget - transform.position;
            direction.y = 0;

            //se siamo arrivati al target, ci fermiamo
            if (direction.magnitude <= 0.1f)
            {
                hasClickTarget = false;
                MoveDirection = Vector3.zero;
            }

            //sennò lo raggiungiamo
            else
            {
                MoveDirection = direction;
            }
        }

        //sennò se ne sta fermo
        else
        {
            MoveDirection = Vector3.zero;
        }
    }

    //metodo per il movimento col puntatore
    private void ReadMouse()
    {
        //se il mouse è sopra la UI, ignoriamo il click
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //con un solo click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            mouseHoldTime = 0f;
            isHoldingMouse = true;
            SetClickTarget();
        }


        //o tenendo premuto/trascinando il cursore
        if (Mouse.current.leftButton.isPressed && isHoldingMouse)
        {
            mouseHoldTime += Time.deltaTime;

            //dopo una piccola soglia consideriamo il movimento come "mouse tenuto premuto"
            if (mouseHoldTime >= 0.1f)
            {
                SetClickTarget();
            }
        }

        //quando rilasciamo il cursore
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            // Se stavamo tenendo premuto il mouse,
            // cancelliamo il movimento
            if (mouseHoldTime >= 0.1f)
            {
                hasClickTarget = false;
                MoveDirection = Vector3.zero;
            }

            isHoldingMouse = false;
            mouseHoldTime = 0f;
        }

    }

    //metodo per settare la direzione nello spazio con un raycast
    private void SetClickTarget()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            clickTarget = hit.point;
            hasClickTarget = true;
        }
    }

}
