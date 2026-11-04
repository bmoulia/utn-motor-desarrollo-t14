using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _velocidad = 2f;              // velocidad patrullando
    [SerializeField] private float _velocidadPersecucion = 4.5f; // velocidad persiguiendo
    [SerializeField] private float _patrolRadiud = 5f;
    [SerializeField] private float _tiempoDeEspera = 2f;

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _centroDePatrulla;
    [SerializeField] private Transform _modelo;

    private NavMeshAgent _navMeshAgent;
    private GameObject _objetivo;

    private float _tiempoDeEsperaActual;
    private bool _tieneDestino;
    private bool _estaDetenido;
    private bool _modoBusqueda; // yendo a la ultima posicion donde vio al player

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    private void Start()
    {
        _navMeshAgent.speed = _velocidad;

        if (_centroDePatrulla == null)
        {
            _centroDePatrulla = transform;
        }

        ElegirDestino();
    }

    private void Update()
    {
        if (_estaDetenido)
        {
            ActualizarAnimaciones();
            return;
        }

        if (_objetivo != null)
        {
            PerseguirObjetivo();
        }
        else if (!_modoBusqueda)
        {
            Patrulla();
        }

        ActualizarRotacion();
        ActualizarAnimaciones();
    }

    public void LocatedPlayer(GameObject player)
    {
        _objetivo = player;
        _tieneDestino = false;
        _modoBusqueda = false;
        _navMeshAgent.speed = _velocidadPersecucion;
        Reanudar();
    }

    public void olvidarObjetivo()
    {
        _objetivo = null;
        _tieneDestino = false;
        _modoBusqueda = false;
        _navMeshAgent.speed = _velocidad;
        Reanudar();
        ElegirDestino();
    }

    //va hasta un punto (la ultima posicion donde vio al player)
    public void IrA(Vector3 posicion)
    {
        _objetivo = null;
        _modoBusqueda = true;
        _navMeshAgent.speed = _velocidadPersecucion;
        Reanudar();
        _navMeshAgent.SetDestination(posicion);
    }

    public bool LlegoADestino()
    {
        return !_navMeshAgent.pathPending &&
               _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 0.3f;
    }

    //gira el modelo para mirar a un punto (se usa al atacar)
    public void MirarHacia(Vector3 punto)
    {
        if (_modelo == null) return;

        Vector3 direccion = punto - transform.position;
        direccion.y = 0f;
        if (direccion.sqrMagnitude < 0.01f) return;

        _modelo.rotation = Quaternion.LookRotation(direccion.normalized) * Quaternion.Euler(0, 180, 0);
    }

    private void ElegirDestino()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 direccionMov = Random.insideUnitSphere * _patrolRadiud;
            direccionMov += _centroDePatrulla.position;

            if (NavMesh.SamplePosition(direccionMov, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(hit.position);
                _tieneDestino = true;
                return;
            }
        }

        _tieneDestino = false;
        _tiempoDeEsperaActual = 0f;
    }

    private void PerseguirObjetivo()
    {
        if (_objetivo == null) return;

        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_objetivo.transform.position);
    }

    private void Patrulla()
    {
        if (!_tieneDestino)
        {
            _tiempoDeEsperaActual += Time.deltaTime;

            if (_tiempoDeEsperaActual >= _tiempoDeEspera)
            {
                ElegirDestino();
            }

            return;
        }

        if (!_navMeshAgent.pathPending &&
            _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _tieneDestino = false;
            _tiempoDeEsperaActual = 0f;
        }
    }

    private void ActualizarAnimaciones()
    {
        if (_animator == null) return;

        bool camina = !_navMeshAgent.isStopped && _navMeshAgent.velocity.sqrMagnitude > 0.05f;
        _animator.SetBool("Camina", camina);
    }

    private void ActualizarRotacion()
    {
        if (_modelo == null) return;

        if (_navMeshAgent.velocity.sqrMagnitude > 0.05f)
        {
            Vector3 direccion = _navMeshAgent.velocity.normalized;
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

            _modelo.rotation = Quaternion.Slerp(
                _modelo.rotation,
                rotacionObjetivo * Quaternion.Euler(0, 180, 0),
                Time.deltaTime * 10f
            );
        }
    }

    public void Detenerse()
    {
        _estaDetenido = true;
        _navMeshAgent.isStopped = true;
    }

    public void Reanudar()
    {
        _estaDetenido = false;
        _navMeshAgent.isStopped = false;
    }
}