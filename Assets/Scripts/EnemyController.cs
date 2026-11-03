
using System.Collections;
using UnityEngine;

public class EnemyController : Character
{
    [SerializeField] private float _radioDeDeteccion = 10f;
    [SerializeField] private float _radioDePersecucion = 15f;
    [SerializeField] private LayerMask _playerLayer;

    [SerializeField] private float _rangoDeAtaque = 2f;
    [SerializeField] private float _daño = 10f;
    [SerializeField] private float _cooldown = 2f;
    [SerializeField] private float _delay = 0.5f;

    [SerializeField] private EnemyMovement _movement;
    [SerializeField] private Animator _animator;

    private Character _objetivo;
    private bool _atacando;
    private bool _estaMuerta;

    protected override void Awake()
    {
        _health = _maxHealth;
        if (_movement == null)
            _movement = GetComponent<EnemyMovement>();

        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_estaMuerta) return;

        if (_objetivo == null)
        {
            DetectarJugador();
            return;
        }

        float distancia = Vector3.Distance(
            transform.position,
            _objetivo.transform.position
        );

        if (distancia > _radioDePersecucion)
        {
            _objetivo = null;
            _movement.olvidarObjetivo();
            return;
        }

        if (_atacando) return;

        if (distancia <= _rangoDeAtaque)
        {
            StartCoroutine(Atacar());
        }
        else
        {
            _movement.LocatedPlayer(_objetivo.gameObject);
        }
    }

    private void DetectarJugador()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            _radioDeDeteccion,
            _playerLayer
        );

        if (colliders.Length == 0) return;

        Character jugadorDetectado =
            colliders[0].GetComponentInParent<Character>();

        if (jugadorDetectado == null) return;

        _objetivo = jugadorDetectado;
        _movement.LocatedPlayer(_objetivo.gameObject);
    }

    private IEnumerator Atacar()
    {
        _atacando = true;
        _movement.Detenerse();

        bool usarSegundoAtaque = Random.value > 0.5f;

        if (usarSegundoAtaque)
            _animator.SetTrigger("Ataque2");
        else
            _animator.SetTrigger("Ataque1");

        yield return new WaitForSeconds(_delay);

        if (_objetivo != null)
        {
            float distancia = Vector3.Distance(
                transform.position,
                _objetivo.transform.position
            );

            if (distancia <= _rangoDeAtaque + 0.3f)
            {
                Vector3 direccion =
                    _objetivo.transform.position - transform.position;

                direccion.y = 0f;
                direccion.Normalize();

                _objetivo.AplicarKnockback(direccion, 5f);
                _objetivo.TakeDamage(_daño);
            }
        }

        yield return new WaitForSeconds(_cooldown);

        _atacando = false;

        if (_objetivo != null)
        {
            float distancia = Vector3.Distance(
                transform.position,
                _objetivo.transform.position
            );

            if (distancia > _rangoDeAtaque)
                _movement.Reanudar();
        }
    }

    protected override void CheckLife()
    {
        if (_health <= 0 && !_estaMuerta)
        {
            _estaMuerta = true;
            _movement.Detenerse();
            _animator.SetTrigger("Muerte");
        }
    }

    public void DestruirAraña()
    {
        Destroy(gameObject);
    }
}
