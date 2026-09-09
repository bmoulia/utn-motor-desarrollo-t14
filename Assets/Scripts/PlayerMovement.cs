using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;
    public float movementSpeed = 5.0f;
    public float mouseSensibility = 200.0f;
    public float gravity = -9.81f;
    private float velocidadVertical;
    public float jump = 5f;
    private string lastPlayerMovementState = "";

    [SerializeField] PlayerStatus _playerStatus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        //gravedad
        if(controller.isGrounded && velocidadVertical < 0)
        {
            velocidadVertical = -2f;
        }
        //caida
        velocidadVertical += gravity * Time.deltaTime;
        //salto
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocidadVertical = jump;
        }

        //movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensibility * Time.deltaTime;
        transform.Rotate(0, mouseX, 0);


        //movimiento del personaje con "WASD"
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 direccion = transform.right * x + transform.forward * y;

        Vector3 movimiento = direccion * movementSpeed;
        movimiento.y = velocidadVertical;

        controller.Move(movimiento * Time.deltaTime);

        
        calculatePlayerMovementState(movimiento);

    }

    //esto es para determiar el estado actual del movimiento del player para poder triggerear sonidos y animaciones.
    private void calculatePlayerMovementState(Vector3 mov)
    {
        string playerMovementState;
        if (velocidadVertical > 0 && !controller.isGrounded) playerMovementState = "Jumping";
        else if (velocidadVertical < 0 && !controller.isGrounded) playerMovementState = "Falling";
        else if (mov.x != 0 || mov.z != 0) playerMovementState = "Walking";
        else playerMovementState = "Static";


            _playerStatus.GetPlayerMovementState(playerMovementState);

    }
}
