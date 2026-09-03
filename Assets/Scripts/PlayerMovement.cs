using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;
    public float movementSpeed = 5.0f;
    public float mouseSensibility = 200.0f;
    public float gravity = -9.81f;
    private float velocidadVertical;
    public float jump = 5f;



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
    }
}
