using UnityEngine;

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

    //stati di gioco e i loro bool
    public GameStatus status;
    public bool IsPaused => status == GameStatus.Paused;
    public bool IsDialogue => status == GameStatus.Dialogue;
    public bool IsGameplay => status == GameStatus.Running;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //all'inizio, il gioco non è in pausa (rivedere più avanti?)
        status = GameStatus.Running;
    }

    //metodo per settare gli stati
    public void SetGameStatus(GameStatus newStatus)
    {
        status = newStatus;
    }

    //metodo per settare la pausa
    public void PauseGame()
    {
        SetGameStatus(GameStatus.Paused);
        Time.timeScale = 0f;
    }

    //e quello per resumare il gioco
    public void ResumeGame()
    {
        SetGameStatus(GameStatus.Running);
        Time.timeScale = 1f;
    }
}
