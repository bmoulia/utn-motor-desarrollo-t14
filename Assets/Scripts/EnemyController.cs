using System.Collections;
using UnityEngine;

public class EnemyController : Character
{
    private enum Estado { Patrullando, Persiguiendo, Buscando }

    [Header("Deteccion")]
    [SerializeField] private float _radioDeDeteccion = 12f;
    [SerializeField] private float _radioDePersecucion = 18f;
    [SerializeField] private float _radioDeOido = 3f;           // a esta distancia te detecta aunque no te vea
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _capasQueTapanVision = 1; // Default = paredes, muebles, cajas
    [SerializeField] private float _alturaOjos = 0.5f;

    [Header("Memoria")]
    [SerializeField] private float _tiempoDeMemoria = 3f;  // cuanto te sigue sin verte
    [SerializeField] private float _tiempoDeBusqueda = 4f; // cuanto te busca donde te vio por ultima vez

    [Header("Ataque")]
    [SerializeField] private float _rangoDeAtaque = 2f;
    [SerializeField] private float _daño = 10f;
    [SerializeField] private float _cooldown = 2f;
    [SerializeField] private float _delay = 0.5f;
    [SerializeField] private float _fuerzaEmpuje = 8f;

    [SerializeField] private EnemyMovement _movement;
    [SerializeField] private Animator _animator;

    [Header("Sonidos")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _attackClip;

    private Estado _estado = Estado.Patrullando;
    private Character _objetivo;
    private Vector3 _ultimaPosicionConocida;
    private float _tiempoSinVer;
    private float _tiempoBuscando;
    private bool _atacando;
    private bool _estaMuerta;

    public bool EstaEnCombate => _estado == Estado.Persiguiendo;

    protected override void Awake()
    {
        _health = _maxHealth;

        if (_movement == null)
            _movement = GetComponent<EnemyMovement>();

        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();
        
        _audioSource.clip = _attackClip;
    }

    private void Update()
    {
        if (_estaMuerta || _atacando) return;

        switch (_estado)
        {
            case Estado.Patrullando:
                if (IntentarDetectar()) EmpezarPersecucion();
                break;

            case Estado.Persiguiendo:
                ActualizarPersecucion();
                break;

            case Estado.Buscando:
                ActualizarBusqueda();
                break;
        }
        switch (_estado)
        {
            case Estado.Persiguiendo:
                GameManager._instance.GetUpdateGameStatus("InCombat", "True");
                break;
            default:
                GameManager._instance.GetUpdateGameStatus("InCombat", "False");
                break;
        }
    }

    // ---------- DETECCION ----------

    private bool IntentarDetectar()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radioDeDeteccion, _playerLayer);

        foreach (Collider col in colliders)
        {
            Character candidato = col.GetComponentInParent<Character>();
            if (candidato == null || candidato == this) continue;

            float distancia = Vector3.Distance(transform.position, candidato.transform.position);

            if (distancia <= _radioDeOido || PuedeVer(candidato))
            {
                _objetivo = candidato;
                return true;
            }
        }

        return false;
    }

    //tira un rayo desde los ojos hasta el player: si choca primero con una pared, no te ve
    private bool PuedeVer(Character objetivo)
    {
        Vector3 ojos = transform.position + Vector3.up * _alturaOjos;
        Vector3 destino = objetivo.transform.position + Vector3.up * 1f;
        Vector3 direccion = destino - ojos;

        if (Physics.Raycast(ojos, direccion.normalized, out RaycastHit hit, direccion.magnitude + 0.5f,
            _capasQueTapanVision | _playerLayer, QueryTriggerInteraction.Ignore))
        {
            return hit.collider.GetComponentInParent<Character>() == objetivo;
        }

        return false;
    }

    // ---------- ESTADOS ----------

    private void EmpezarPersecucion()
    {
        _estado = Estado.Persiguiendo;
        _tiempoSinVer = 0f;
        _ultimaPosicionConocida = _objetivo.transform.position;
        _movement.LocatedPlayer(_objetivo.gameObject);
    }

    private void ActualizarPersecucion()
    {
        if (_objetivo == null)
        {
            VolverAPatrullar();
            return;
        }

        float distancia = Vector3.Distance(transform.position, _objetivo.transform.position);

        if (PuedeVer(_objetivo))
        {
            _ultimaPosicionConocida = _objetivo.transform.position;
            _tiempoSinVer = 0f;
        }
        else
        {
            _tiempoSinVer += Time.deltaTime;
        }

        //si te alejaste mucho o hace rato que no te ve, va a buscarte donde te vio
        if (distancia > _radioDePersecucion || _tiempoSinVer > _tiempoDeMemoria)
        {
            EmpezarBusqueda();
            return;
        }

        if (distancia <= _rangoDeAtaque)
        {
            StartCoroutine(Atacar());
        }
    }

    private void EmpezarBusqueda()
    {
        _estado = Estado.Buscando;
        _tiempoBuscando = 0f;
        _movement.IrA(_ultimaPosicionConocida);
    }

    private void ActualizarBusqueda()
    {
        if (IntentarDetectar())
        {
            EmpezarPersecucion();
            return;
        }

        //cuando llega, se queda un rato "buscando" y despues vuelve a patrullar
        if (_movement.LlegoADestino())
        {
            _tiempoBuscando += Time.deltaTime;

            if (_tiempoBuscando >= _tiempoDeBusqueda)
            {
                VolverAPatrullar();
            }
        }
    }

    private void VolverAPatrullar()
    {
        _estado = Estado.Patrullando;
        _objetivo = null;
        _movement.olvidarObjetivo();
    }

    // ---------- ATAQUE ----------

    private IEnumerator Atacar()
    {
        _atacando = true;
        _movement.Detenerse();
        _movement.MirarHacia(_objetivo.transform.position);
        _audioSource.Play();

        if (_animator != null)
        {
            if (Random.value > 0.5f) _animator.SetTrigger("Ataque2");
            else _animator.SetTrigger("Ataque1");
        }

        yield return new WaitForSeconds(_delay);

        if (_estaMuerta) yield break;

        if (_objetivo != null)
        {
            float distancia = Vector3.Distance(transform.position, _objetivo.transform.position);

            if (distancia <= _rangoDeAtaque + 0.3f)
            {
                Vector3 direccion = _objetivo.transform.position - transform.position;
                direccion.y = 0f;
                direccion.Normalize();

                _objetivo.AplicarKnockback(direccion, _fuerzaEmpuje);
                _objetivo.TakeDamage(_daño);
            }
        }

        yield return new WaitForSeconds(_cooldown);

        _atacando = false;

        if (_estaMuerta) yield break;

        if (_objetivo != null)
        {
            float distancia = Vector3.Distance(transform.position, _objetivo.transform.position);

            if (distancia > _rangoDeAtaque)
                _movement.Reanudar();
        }
    }

    // ---------- VIDA ----------

    //si le pegás con un objeto, te persigue aunque no te haya visto
    public override void TakeDamage(float damage)
    {
        if (_estaMuerta || damage <= 0f) return;

        base.TakeDamage(damage);

        if (!_estaMuerta && _estado != Estado.Persiguiendo)
        {
            PlayerStatus player = FindFirstObjectByType<PlayerStatus>();
            if (player != null)
            {
                _objetivo = player;
                EmpezarPersecucion();
            }
        }
    }

    protected override void CheckLife()
    {
        if (_health <= 0 && !_estaMuerta)
        {
            _estaMuerta = true;
            _movement.Detenerse();

            if (_animator != null) _animator.SetTrigger("Muerte");
            else Destroy(gameObject);
        }
    }

    //la llama el evento de la animacion de muerte, no cambiarle el nombre
    public void DestruirAraña()
    {
        Destroy(gameObject);
    }

    //dibuja los radios en la escena cuando seleccionás la araña
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radioDeDeteccion);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _radioDeOido);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _rangoDeAtaque);
    }
}