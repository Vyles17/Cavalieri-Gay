using UnityEngine;
using UnityEngine.InputSystem;

// Scriptino generale per settare il manager di gioco

public enum GameStatus
{
    Running,
    Dialogue,
    Paused //Aggiungere qua i vari stati di gioco che aggiungeremo
}

public class GameManager : MonoBehaviour
{
    //Singleton del GM
    public static GameManager Instance;

    //bools per gli stati di gioco
    [HideInInspector] public bool isPaused = false;

    //stati di gioco e i loro bool
    public GameStatus status;
    public bool IsPaused => status == GameStatus.Paused;
    public bool IsDialogue => status == GameStatus.Dialogue;
    public bool IsGameplay => status == GameStatus.Running;

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

    //metodo per settare gli stati
    public void SetGameStatus(GameStatus newStatus)
    {
        status = newStatus;
    }

    //metodo per mettere in pausa
    private void Pause(InputAction.CallbackContext context)
    {
        if (status == GameStatus.Paused)
            ResumeGame();
        else if (status == GameStatus.Running)
            PauseGame();
    }

    private void PauseGame()
    {
        SetGameStatus(GameStatus.Paused);

        Time.timeScale = 0f;

        OnPauseChanged?.Invoke(true);
    }

    public void ResumeGame()
    {
        SetGameStatus(GameStatus.Running);

        Time.timeScale = 1f;

        OnPauseChanged?.Invoke(false);
    }
}
