using UnityEngine;

//script di movimento per il Cavaliere Bianco

public class CBMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private CharacterController controller;

    //ci gettiamo lo script degli input, che funzionerà attraverso i settings di movimento di questo script
    private CBInput input;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<CBInput>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 move = input.MoveDirection; //la direzione data dall'input del player

        if (move.magnitude <= 0.1f)
            return;

        float speed = input.IsRunning
            ? runSpeed // se il player sta tenendo premuto "Shift", CB corre
            : walkSpeed; //sennò cammina

        move.Normalize(); //per farlo andare alla stessa velocità anche in diagonale

        Quaternion targetRotation = Quaternion.LookRotation(move); //la rotazione del player rispetto al movimento
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        controller.Move(move * speed * Time.deltaTime); //e ora vai, figlio mio
    }

}
