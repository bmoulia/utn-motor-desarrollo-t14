using UnityEngine;

public class GrabableController : MonoBehaviour
{
    Rigidbody _rb; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.tag = "Agarrable";
        _rb = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;

        Character character = collision.gameObject.GetComponent<Character>();

        if (character != null)
        {
            character.TakeDamage(Mathf.Round(speed));
        }
    }
        
}
