using UnityEngine;

//script per il menu di pausa
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    void Start()
    {
        //all'inizio il menu di pausa è nascosto
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.OnPauseChanged += ToggleMenu;
    }

    private void OnDisable()
    {
        GameManager.OnPauseChanged -= ToggleMenu;
    }

    private void ToggleMenu(bool paused)
    {
        pauseMenu.SetActive(paused);
    }
}
