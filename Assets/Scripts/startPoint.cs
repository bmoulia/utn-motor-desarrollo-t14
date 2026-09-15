using UnityEngine;

public class startPoint : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _patrollPoint;
    private GameObject _enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemy = Instantiate(_enemyPrefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
        _enemy.GetComponent<EnemyMovement>().SpawnPoint = gameObject;
        _enemy.GetComponent<EnemyMovement>().PatrollPoint = _patrollPoint;
        _patrollPoint.GetComponent<PatrollPoint>().enemy = _enemy;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _enemy)
        {
            _enemy.GetComponent<EnemyMovement>().TurnArround(true);
        }
        else
        {
            return;
        }
    }
}
