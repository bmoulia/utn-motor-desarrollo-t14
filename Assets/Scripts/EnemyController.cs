using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private string _attackType;
    [SerializeField] private EnemyMovement _movement;

    [SerializeField] private float _radious;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*rivate void DetectPlayer()
    {
        Collider[] collitions = Physics.OverlapSphere(transform.position, _ra)
    }/*/

}
