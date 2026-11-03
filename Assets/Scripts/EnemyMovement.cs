
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _velocidad = 2f;
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
        else
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
        Reanudar();
    }


    public void olvidarObjetivo()
    {
        _objetivo = null;
        _tieneDestino = false;
        Reanudar();
        ElegirDestino();
    }

    public void DarseVuelta(bool value)
    {
        
    }


    private void ElegirDestino()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 direccionMov =
                Random.insideUnitSphere * _patrolRadiud;

            direccionMov += _centroDePatrulla.position;

            if (NavMesh.SamplePosition(
                direccionMov,
                out NavMeshHit hit,
                2f,
                NavMesh.AllAreas))
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

        bool camina =
            !_navMeshAgent.isStopped &&
            _navMeshAgent.velocity.sqrMagnitude > 0.05f;

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
