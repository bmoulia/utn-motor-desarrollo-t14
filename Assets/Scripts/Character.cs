using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    protected float _health;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log("Got " +  damage + " points of damage");
        _health -= damage;
        CheckLife();

    }

    protected virtual void CheckLife()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }

    }
}
