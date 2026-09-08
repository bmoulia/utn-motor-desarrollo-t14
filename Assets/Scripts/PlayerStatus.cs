using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private static PlayerStatus _playerStatus;
    [SerializeField] float _lowHealthTreshold;
    [SerializeField] private int _playerlifes;
    [SerializeField] private float _playerRespawnPorcentage;
    public enum playerState
    {
        Healthy,
        LowHealth,
        Dead
    }

    private playerState _playerCurrentState;
    private playerState _playerNewState;

    private string _gameManagerKey = "PlayerState";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerCurrentState = playerState.Healthy;
        _playerNewState = playerState.Healthy;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerCurrentState != _playerNewState)
        {
            _playerCurrentState = _playerNewState;
            UpdateGameManager(_playerCurrentState);
        }
    }

    //si hay un cambio en la vida del jugador, se debe llamar esta funcion para validar si está por encima del umbral de "playerLowHealth".
    public int playerLifeUpdateCheck(int playerMaxLife, int playerCurrentLife)
    {
        if (playerCurrentLife <= 0)
        {
            _playerlifes--;
            if (_playerlifes > 0)
            {
                playerCurrentLife = (int)(playerMaxLife * _playerRespawnPorcentage);
            }
        }

        if (playerCurrentLife <= 0 && _playerlifes<= 0) _playerNewState = playerState.Dead;
        else if (playerCurrentLife / playerMaxLife <= _lowHealthTreshold) _playerNewState = playerState.LowHealth;
        else if (playerCurrentLife / playerMaxLife > _lowHealthTreshold) _playerNewState = playerState.Healthy;
        else Debug.Log("playerLifeUpdateCheck playerState Missmatch");

        return playerCurrentLife;
    }

    private void UpdateGameManager(playerState state)
    {
        switch (state)
        {
            case playerState.Healthy:
                GameManager._instance.getUpdate("Healthy", _gameManagerKey);
                break;
            case playerState.LowHealth:
                GameManager._instance.getUpdate("LowHealth", _gameManagerKey);
                break;
            case playerState.Dead:
                GameManager._instance.getUpdate("Dead", _gameManagerKey);
                break;
            default:
                Debug.Log("UpdateGameManager playerState Missmatch");
                break;
        }
    }

}
