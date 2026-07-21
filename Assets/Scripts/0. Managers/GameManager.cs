using UnityEngine;
using UnityEngine.InputSystem;

// Scriptino generale per settare il manager di gioco

public enum GameStatus
{
    Running,
    Paused,
}

public class GameManager : MonoBehaviour
{
    //Singleton del GM
    public static GameManager Instance;

    //bools per gli stati di gioco
    [HideInInspector] public bool isPaused = false;
    public GameStatus status = GameStatus.Running;

    //Input map per gestire i comandi per la UI e il suo evento
    private InputMap inputMap;
    public static System.Action<bool> OnPauseChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        //ci gettiamo l'input map
        inputMap = new InputMap();
    }
    void OnEnable()
    {
        inputMap.Enable();
        inputMap.UI.PauseMenu.performed += Pause;
    }

    void OnDisable()
    {
        inputMap.UI.PauseMenu.performed -= Pause;
        inputMap.Disable();
    }
    private void Start()
    {
        //all'inizio, il gioco non è in pausa (rivedere più avanti?)
        status = GameStatus.Running;
        isPaused = false;
    }
    private void Pause(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        //setto il bool, lo stato e la velocità di gioco, faccio partire l'evento che richiama la UI
        isPaused = true;
        status = GameStatus.Paused;

        Time.timeScale = 0f;

        OnPauseChanged?.Invoke(true);
    }

    public void ResumeGame()
    {
        //setto il bool, lo stato e la velocità di gioco, faccio partire l'evento che richiama la UI
        isPaused = false;
        status = GameStatus.Running;

        Time.timeScale = 1f;

        OnPauseChanged?.Invoke(false);
    }
}
