using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;
    protected float _health;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        _health = _maxHealth;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        
        _health = _health -damage;
        CheckLife();

    }

    protected virtual void CheckLife()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }

    }

    public virtual void AplicarKnockback(Vector3 direccion, float fuerza)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null)
            return;

        direccion.y = 0f;
        direccion.Normalize();

        rb.AddForce(direccion * fuerza, ForceMode.Impulse);
    }
}
