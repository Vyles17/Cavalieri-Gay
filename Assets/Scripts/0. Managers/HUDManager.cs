using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//scriptino per il manager di tutte le cose dell'HUD
public class HUDManager : MonoBehaviour
{
    //i gameObj dei vari menu
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject pauseMenu;

    //evento da richiamare quando apriamo/chiudiamo l'inventario
    public static System.Action<bool> OnInventoryChanged;
    //e il suo bool per sapere se è aperto
    private bool inventoryOpen = false;
    private InputMap inputMap;

    private void Awake()
    {
        //ci prendiamo l'inputmap
        inputMap = new InputMap();
    }
    private void OnEnable()
    {
        inputMap.Enable();

        inputMap.UI.Inventory.performed += OnInventoryInput;
        inputMap.UI.PauseMenu.performed += OnPauseInput;

    }

    private void OnDisable()
    {
        inputMap.UI.Inventory.performed -= OnInventoryInput;
        inputMap.UI.PauseMenu.performed -= OnPauseInput;

        inputMap.Disable();
    }
    void Start()
    {
        //all'inizio i menu sno nascosti
        if (inventory != null)
            inventory.SetActive(false);
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    //funzione per aprire/chiudere il menu di pausa col bottone
    public void TogglePause()
    {
        if (GameManager.Instance.IsPaused)
        {
            GameManager.Instance.ResumeGame();

            if (pauseMenu != null)
                pauseMenu.SetActive(false);
        }

        else if (GameManager.Instance.IsGameplay)
        {
            GameManager.Instance.PauseGame();

            if (pauseMenu != null)
                pauseMenu.SetActive(true);
        }
    }

    //metodo per mettere in pausa con l'input system
    private void OnPauseInput(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    //funzione per aprire/chiudere l'inventario col bottone
    public void ToggleInventory()
    {
        //se il gioco è in pausa non possiamo aprire o chiudere i menu
        if (GameManager.Instance.IsPaused)
            return;

        inventoryOpen = !inventoryOpen;

        if (inventory != null)
            inventory.SetActive(inventoryOpen);

        OnInventoryChanged?.Invoke(inventoryOpen);
    }

    // funzione per aprire/chiudere l'inventario con l'input system
    private void OnInventoryInput(InputAction.CallbackContext context)
    {
        ToggleInventory();
    }
}
