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
    public Animator animator;

    //private float x, y;

    //public Rigidbody rb;
    //public Transform groundCheck;
    //public float groundDistance = 0.1f;


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
        
        //salto (solo si está en el piso)
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocidadVertical = Mathf.Sqrt(jump * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
        
        //caida
        velocidadVertical += gravity * Time.deltaTime;

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

        //animator.SetFloat("VelX", x);TUVE QUE COMENTAR ESTO PORQUE DA ERROR Y NO ME DEJA COMPILAR
        //animator.SetFloat("VelY", y);TUVE QUE COMENTAR ESTO PORQUE DA ERROR Y NO ME DEJA COMPILAR
    }
}
