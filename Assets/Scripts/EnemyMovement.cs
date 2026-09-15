using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private string _movementType;
    [SerializeField] private float _speed;
    [SerializeField]  private GameObject _patrollPoint;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private GameObject _player;
    private bool _playerLocated;
    private bool _ida;

    public GameObject PatrollPoint {  get { return _patrollPoint; } set { _patrollPoint = value; } }
    public GameObject SpawnPoint {  get { return _spawnPoint; } set {_spawnPoint = value; }  }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerLocated = false;
        transform.position = _spawnPoint.transform.position;
        _ida = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction;

        if (_playerLocated)
        {
            direction = _player.transform.position - transform.position;
        }
        else if (_ida)
        {
            direction =_patrollPoint.transform.position - transform.position;
        }
        else
        {
            direction = _spawnPoint.transform.position - transform.position;
        }

        transform.position += direction * _speed * Time.deltaTime;

    }

    public void LocatedPlayer(GameObject player)
    {
        if (player != null)
        {
            _playerLocated = true;
            _player = player;
        }
    }

    public void TurnArround(bool value)
    {
        _ida = value;
    }

}
