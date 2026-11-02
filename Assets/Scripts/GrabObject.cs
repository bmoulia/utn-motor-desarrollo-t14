using Unity.Mathematics;
using UnityEngine;

public class GrabObject : MonoBehaviour
{

    public float distanciaAgarre = 3f;
    
    //objetoAgarrado guarda cual caja estás sosteniendo
    private Rigidbody objetoAgarrado;


    public float alcance = 5;
    private Camera cam;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _weaponRayClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        cam = Camera.main; 
        _audioSource.clip = _weaponRayClip;

    }

    // Update is called once per frame
    void Update()
    {
        //desde donde y hacia donde sale el rayolaser
        Vector3 origen = cam.transform.position;
        Vector3 direccion = cam.transform.forward;

        //dibuja el rayolaser en la escena
        Debug.DrawRay(origen, direccion * alcance, Color.blue);

        //deteccion de colision rayolaser segun la docu
        RaycastHit hit;
        if(Physics.Raycast(origen, direccion, out hit, alcance))
        {
            if (hit.collider.CompareTag("Agarrable"))
            {
                Debug.Log("Puedo agarrar: " + hit.collider.name);
            }
        }


        //mecanicas de agarre

        // al apretar el clic, intento agarrar
        if (Input.GetMouseButtonDown(0))
        {
            PlaySound(true);
            if (Physics.Raycast(origen, direccion, out hit, alcance))
            {
                if (hit.collider.CompareTag("Agarrable"))
                {
                    objetoAgarrado = hit.collider.GetComponent<Rigidbody>();
                }
            }
        }

        // cuando suelto el clic
        if (Input.GetMouseButtonUp(0))
        {
            PlaySound(false);
            objetoAgarrado = null;
        }

        // mientras tengo algo agarrado, lo muevo con fisica hacia el punto
        if (objetoAgarrado != null)
        {
            Vector3 puntoDestino = cam.transform.position + cam.transform.forward * distanciaAgarre;
            Vector3 direccionHaciaDestino = puntoDestino - objetoAgarrado.position;
            objetoAgarrado.linearVelocity = direccionHaciaDestino * 10f;
            //esto frena la rotacion cuando agarras un objeto
            objetoAgarrado.angularVelocity = Vector3.zero;   
        }


        //mecanica de rotacion
        //unity lee el scroll con Input.GetAxis("Mouse ScrollWheel") segun la docu
        if (objetoAgarrado != null)
        {
            // ajustar distancia con la rueda
            distanciaAgarre += Input.GetAxis("Mouse ScrollWheel") * 5f;
            //control de cercania
            distanciaAgarre = Mathf.Clamp(distanciaAgarre, 5f, 10f);


            Vector3 puntoDestino = cam.transform.position + cam.transform.forward * distanciaAgarre;
            Vector3 direccionHaciaDestino = puntoDestino - objetoAgarrado.position;
            objetoAgarrado.linearVelocity = direccionHaciaDestino * 10f;
            objetoAgarrado.angularVelocity = Vector3.zero;
        }

    }

    private void PlaySound(bool click)
    {
        if (click)
        {
            if(!_audioSource.isPlaying) _audioSource.Play();
        }else if (_audioSource.isPlaying) _audioSource.Stop();

    }
}
