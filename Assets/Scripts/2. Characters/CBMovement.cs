using UnityEngine;
using UnityEngine.InputSystem;

//script di movimento per il Cavaliere Bianco

public class CBMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction walkAction;
    private InputAction runAction;
    private void Awake()
    {
        controller = GetComponent<CharacterController>(); //getto i componenti
        playerInput = GetComponent<PlayerInput>();

        walkAction = playerInput.actions.FindAction("Walk"); //e le Action Map, se esistono e hanno questo nome preciso
        runAction = playerInput.actions.FindAction("Run");
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float speed; //setto una nuova variabile per la velocità del player

        if (runAction.IsPressed()) //se il player sta tenendo premuto "Shift", CB corre
            speed = runSpeed;
        else
            speed = walkSpeed; //sennò cammina


        Vector2 input = walkAction.ReadValue<Vector2>(); //mi prendo i valori dell'actionMap "Walk"
        Vector3 move = new Vector3(input.x, 0f, input.y); //li traduco per lo spazio 3D in cui si muove il player

        move = move.normalized; //normalizzo la velocità per quando ci spostiamo in diagonale

        Quaternion targetRotation = Quaternion.LookRotation(move); //la rotazione del player rispetto al movimento
        transform.rotation = Quaternion.Slerp( transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        controller.Move(move * speed * Time.deltaTime); //e ora vai, figlio mio
    }

}
