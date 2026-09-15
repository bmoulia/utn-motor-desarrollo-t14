using System;
using TMPro;
using UnityEngine;

public class PatrollPoint : MonoBehaviour
{
    [SerializeField]private GameObject _enemy;
    public GameObject enemy { get { return _enemy; } set { _enemy = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == _enemy) {
            _enemy.GetComponent<EnemyMovement>().TurnArround(false);
            Debug.Log("regresa");
        }
        else
        {
            return;
        }
    }
}
