using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float movementSpeed = 5.0f;
    public float mouseSensibility = 200.0f;
    public float gravity = -9.81f;
    private float velocidadVertical;
    public float jump = 5f;

    [SerializeField] PlayerStatus _playerStatus;
    public Animator animator;

    //que tan rapido cambia la animacion (para que no salte de golpe)
    public float suavizadoAnimacion = 0.1f;

    //empuje que recibe el player cuando lo golpean (knockback)
    private Vector3 _empuje;
    public float frenadoEmpuje = 5f;

    //se apaga cuando el player muere
    public bool puedeMoverse = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        //si no se asignó el animator en el Inspector, lo busco solo
        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //gravedad
        if (controller.isGrounded && velocidadVertical < 0)
        {
            velocidadVertical = -2f;
        }

        float x = 0f;
        float y = 0f;

        if (puedeMoverse)
        {
            //salto (solo si está en el piso)
            if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                velocidadVertical = Mathf.Sqrt(jump * -2f * gravity);

                if (animator != null)
                {
                    animator.SetTrigger("Jump");
                }
            }

            //movimiento del mouse
            float mouseX = Input.GetAxis("Mouse X") * mouseSensibility * Time.deltaTime;
            transform.Rotate(0, mouseX, 0);

            //movimiento del personaje con "WASD"
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
        }

        //caida
        velocidadVertical += gravity * Time.deltaTime;

        Vector3 direccion = transform.right * x + transform.forward * y;
        Vector3 movimientoPropio = direccion * movementSpeed;

        //sumo el empuje de los golpes
        Vector3 movimiento = movimientoPropio + _empuje;
        movimiento.y = velocidadVertical;

        controller.Move(movimiento * Time.deltaTime);

        //el empuje se va frenando solo
        _empuje = Vector3.Lerp(_empuje, Vector3.zero, frenadoEmpuje * Time.deltaTime);
        if (_empuje.sqrMagnitude < 0.01f) _empuje = Vector3.zero;

        ActualizarAnimaciones(x, y);
        calculatePlayerMovementState(movimientoPropio);
    }

    //le pasa al Animator hacia donde se mueve el player
    //VelX: -1 izquierda, 1 derecha / VelY: -1 atras, 1 adelante
    private void ActualizarAnimaciones(float x, float y)
    {
        if (animator == null) return;

        animator.SetFloat("VelX", x, suavizadoAnimacion, Time.deltaTime);
        animator.SetFloat("VelY", y, suavizadoAnimacion, Time.deltaTime);
    }

    //lo llama PlayerStatus cuando un enemigo te golpea
    public void AplicarEmpuje(Vector3 fuerza)
    {
        _empuje = new Vector3(fuerza.x, 0f, fuerza.z);

        //saltito chico para que el golpe se sienta
        if (controller.isGrounded) velocidadVertical = 2f;
    }

    //se usa al reaparecer para no arrastrar empujes ni caidas
    public void ResetearMovimiento()
    {
        _empuje = Vector3.zero;
        velocidadVertical = 0f;
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