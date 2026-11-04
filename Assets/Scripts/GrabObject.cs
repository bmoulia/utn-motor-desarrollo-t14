using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public float distanciaAgarre = 5f;

    //objetoAgarrado guarda cual caja estás sosteniendo
    private Rigidbody objetoAgarrado;

    public float alcance = 5;
    public float velocidadRotacion = 3f;
    private Camera cam;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _weaponRayClip;

    [Header("Rayo visual")]
    [SerializeField] private LineRenderer _rayo;
    [SerializeField] private Material _materialRayo;
    [SerializeField] private Transform _puntoDisparo;   // de donde sale el rayo (mano, arma, pecho)
    [SerializeField] private float _anchoRayo = 0.08f;
    [SerializeField] private bool _parpadeo = true;    // el rayo "vibra" un poco

    void Start()
    {
        cam = Camera.main;
        _audioSource.clip = _weaponRayClip;

        if (_rayo != null)
        {
            _rayo.positionCount = 2;
            _rayo.useWorldSpace = true;
            _rayo.enabled = false;
            if (_materialRayo != null) _rayo.material = _materialRayo;
        }
        else
        {
            Debug.LogWarning("GrabObject en " + gameObject.name + ": no tiene asignado el Rayo (LineRenderer)");
        }
    }

    void Update()
    {
        //desde donde y hacia donde apunta la mira
        Vector3 origen = cam.transform.position;
        Vector3 direccion = cam.transform.forward;

        RaycastHit hit;

        // al apretar el clic, intento agarrar
        if (Input.GetMouseButtonDown(0))
        {
            PlaySound(true);
            if (Physics.Raycast(origen, direccion, out hit, alcance))
            {
                if (hit.collider.CompareTag("Agarrable"))
                {
                    Rigidbody candidato = hit.collider.GetComponent<Rigidbody>();

                    // no dejo agarrar el objeto si estoy parado encima
                    if (!EstoyParadoSobre(candidato))
                    {
                        objetoAgarrado = candidato;
                    }
                }
            }
        }

        // cuando suelto el clic
        if (Input.GetMouseButtonUp(0))
        {
            Soltar();
        }

        // si me subo a un objeto mientras lo tengo agarrado, lo suelto
        if (objetoAgarrado != null && EstoyParadoSobre(objetoAgarrado))
        {
            Soltar();
        }

        // mientras tengo algo agarrado, lo muevo con fisica hacia el punto
        if (objetoAgarrado != null)
        {
            // ajustar distancia con la rueda
            distanciaAgarre += Input.GetAxis("Mouse ScrollWheel") * 5f;
            distanciaAgarre = Mathf.Clamp(distanciaAgarre, 5f, 10f);

            Vector3 puntoDestino = cam.transform.position + cam.transform.forward * distanciaAgarre;
            Vector3 direccionHaciaDestino = puntoDestino - objetoAgarrado.position;
            objetoAgarrado.linearVelocity = direccionHaciaDestino * 10f;

            //rotacion con teclas: si no apretás nada, queda en cero y el objeto no gira
            Vector3 ejeRotacion = Vector3.zero;

            if (Input.GetKey(KeyCode.Q)) ejeRotacion += Vector3.up;
            if (Input.GetKey(KeyCode.E)) ejeRotacion -= Vector3.up;
            if (Input.GetKey(KeyCode.R)) ejeRotacion += cam.transform.right;
            if (Input.GetKey(KeyCode.F)) ejeRotacion -= cam.transform.right;

            objetoAgarrado.angularVelocity = ejeRotacion * velocidadRotacion;
        }

        ActualizarRayo(origen, direccion);
    }

    // dibuja el rayo mientras mantenés el clic
    private void ActualizarRayo(Vector3 origen, Vector3 direccion)
    {
        if (_rayo == null) return;

        bool usandoHabilidad = Input.GetMouseButton(0);
        _rayo.enabled = usandoHabilidad;
        if (!usandoHabilidad) return;

        // punto de salida: la mano si está asignada, si no un punto adelante del pecho
        Vector3 inicio;
        if (_puntoDisparo != null) inicio = _puntoDisparo.position;
        else inicio = transform.position + Vector3.up * 1.2f + transform.forward * 0.5f;

        // punto final: el objeto agarrado, lo que toca la mira, o el alcance maximo
        Vector3 fin;
        if (objetoAgarrado != null)
        {
            fin = objetoAgarrado.position;
        }
        else if (Physics.Raycast(origen, direccion, out RaycastHit hitRayo, alcance))
        {
            fin = hitRayo.point;
        }
        else
        {
            fin = origen + direccion * alcance;
        }

        _rayo.SetPosition(0, inicio);
        _rayo.SetPosition(1, fin);

        float ancho = _anchoRayo;
        if (_parpadeo) ancho *= Random.Range(0.6f, 1.4f);
        _rayo.startWidth = ancho;
        _rayo.endWidth = ancho * 0.5f;
    }

    // tira una esfera hacia abajo desde los pies del player y se fija si toca el objeto
    private bool EstoyParadoSobre(Rigidbody rb)
    {
        if (rb == null) return false;

        Vector3 origenPies = transform.position + Vector3.up * 0.5f;
        RaycastHit hitPies;

        if (Physics.SphereCast(origenPies, 0.25f, Vector3.down, out hitPies, 0.7f))
        {
            return hitPies.rigidbody == rb;
        }
        return false;
    }

    private void Soltar()
    {
        PlaySound(false);
        objetoAgarrado = null;
    }

    private void PlaySound(bool click)
    {
        if (click)
        {
            if (!_audioSource.isPlaying) _audioSource.Play();
        }
        else if (_audioSource.isPlaying) _audioSource.Stop();
    }
}