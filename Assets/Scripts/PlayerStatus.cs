using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : Character
{
    [SerializeField] float _lowHealthTreshold = 0.3f;     // porcentaje: 0.3 = 30% de vida
    [SerializeField] private int _playerlifes = 3;         // vidas totales
    [SerializeField] private float _playerRespawnPorcentage = 1f; // 1 = reaparece con vida llena
    [SerializeField] private GameObject _respawnPont;
    [SerializeField] private Transform _spawnPoint;

    [Header("Daño")]
    [SerializeField] private float _tiempoInvulnerableGolpe = 0.6f;
    [SerializeField] private float _tiempoInvulnerableRespawn = 2f;
    private float _invulnerableActual = 0f;

    [Header("Muerte")]
    [SerializeField] private GameObject _pantallaMuerte;   // opcional: panel de "Perdiste"
    [SerializeField] private float _esperaAntesDeReiniciar = 2f;

    public enum playerState
    {
        Healthy,
        LowHealth,
        Dead
    }

    private playerState _playerCurrentState;
    private playerState _playerNewState;
    private string _playerMovementState;
    private string _LastplayerMovementState;

    private float _jumpCooldown = 0.5f;
    private float _currentJumpCooldown = 0f;
    private float _landCooldown = 0.5f;
    private float _currentLandCooldown = 0f;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _walkAudioSource;

    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _landClip;

    private PlayerMovement _movement;
    private CharacterController _controller;

    //esto lo lee la barra de vida
    public float VidaActual => _health;
    public float VidaMaxima => _maxHealth;
    public int Vidas => _playerlifes;
    public playerState Estado => _playerCurrentState;

    protected override void Awake()
    {
        //IMPORTANTE: esto inicializa la vida. Antes no se llamaba y arrancabas con 0.
        base.Awake();

        if (!_spawnPoint && _respawnPont) _spawnPoint = _respawnPont.transform;

        _movement = GetComponent<PlayerMovement>();
        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        _playerCurrentState = playerState.Healthy;
        _playerNewState = playerState.Healthy;
        _walkAudioSource.clip = _walkClip;

        if (_pantallaMuerte != null) _pantallaMuerte.SetActive(false);
    }

    void Update()
    {
        if (_invulnerableActual > 0f) _invulnerableActual -= Time.deltaTime;

        if (_playerCurrentState != _playerNewState)
        {
            _playerCurrentState = _playerNewState;
        }

        chekSound();

        if (_currentJumpCooldown > 0) _currentJumpCooldown -= Time.deltaTime;
        else if (_currentJumpCooldown < 0) _currentJumpCooldown = 0;
        if (_currentLandCooldown > 0) _currentLandCooldown -= Time.deltaTime;
        else if (_currentLandCooldown < 0) _currentLandCooldown = 0;
    }

    public override void TakeDamage(float damage)
    {
        if (_playerCurrentState == playerState.Dead) return;
        if (_invulnerableActual > 0f) return;

        _invulnerableActual = _tiempoInvulnerableGolpe;

        //resta la vida y llama a CheckLife
        base.TakeDamage(damage);

        ActualizarEstadoDeVida();
    }

    //el player usa CharacterController, no Rigidbody, asi que el empuje lo hace PlayerMovement
    public override void AplicarKnockback(Vector3 direccion, float fuerza)
    {
        if (_playerCurrentState == playerState.Dead) return;
        if (_invulnerableActual > 0f) return;
        if (_movement == null) return;

        direccion.y = 0f;
        direccion.Normalize();
        _movement.AplicarEmpuje(direccion * fuerza);
    }

    private void ActualizarEstadoDeVida()
    {
        if (_playerNewState == playerState.Dead) return;

        if (_health / _maxHealth <= _lowHealthTreshold) _playerNewState = playerState.LowHealth;
        else _playerNewState = playerState.Healthy;
    }

    protected override void CheckLife()
    {
        if (_health > 0f) return;

        _playerlifes--;

        if (_playerlifes > 0) Respawn();
        else Morir();
    }

    private void Respawn()
    {
        _health = _maxHealth * _playerRespawnPorcentage;

        //hay que apagar el CharacterController para teletransportar, si no Unity pisa la posicion
        if (_controller != null) _controller.enabled = false;
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        if (_controller != null) _controller.enabled = true;

        if (_movement != null) _movement.ResetearMovimiento();

        _invulnerableActual = _tiempoInvulnerableRespawn;
        ActualizarEstadoDeVida();
    }

    private void Morir()
    {
        _health = 0f;
        _playerNewState = playerState.Dead;

        if (_movement != null) _movement.puedeMoverse = false;
        if (_walkAudioSource.isPlaying) _walkAudioSource.Stop();

        if (_pantallaMuerte != null)
        {
            _pantallaMuerte.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            //si no hay pantalla de muerte, reinicia el nivel solo
            Invoke(nameof(Reintentar), _esperaAntesDeReiniciar);
        }
    }

    //se puede asignar a un boton "Reintentar" de la pantalla de muerte
    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //La logica que controla el sonido del jugador
    private void chekSound()
    {
        if (_playerMovementState == "Jumping" && _LastplayerMovementState != "Jumping" && _currentJumpCooldown <= 0)
        {
            _audioSource.PlayOneShot(_jumpClip);
            _currentJumpCooldown = _jumpCooldown;
        }
        else if (_playerMovementState != "Falling" && _LastplayerMovementState == "Falling" && _currentLandCooldown <= 0)
        {
            _audioSource.PlayOneShot(_landClip);
            _currentLandCooldown = _landCooldown;
        }
        else if (_playerMovementState == "Walking")
        {
            if (!_walkAudioSource.isPlaying) _walkAudioSource.Play();
        }
        else
        {
            if (_walkAudioSource.isPlaying) _walkAudioSource.Stop();
        }
    }

    public void GetPlayerMovementState(string movementState)
    {
        _LastplayerMovementState = _playerMovementState;
        _playerMovementState = movementState;
    }
}