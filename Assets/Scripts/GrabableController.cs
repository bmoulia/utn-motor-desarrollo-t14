using UnityEngine;

public class GrabableController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] private GameObject _crashSound;
    
    private float _crashSoundCooldown = 0.5f;
    private float _crashSoundCurrentCooldown = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.tag = "Agarrable";
        _rb = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_crashSoundCurrentCooldown > 0f)
        {
            _crashSoundCurrentCooldown -= Time.deltaTime;
        }else if (_crashSoundCurrentCooldown < 0f)
        {
            _crashSoundCurrentCooldown = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;

        Character character = collision.gameObject.GetComponent<Character>();

        if (character != null)
        {
            character.TakeDamage(Mathf.Round(speed));
        }

        if (_crashSoundCurrentCooldown <= 0f)
        {
            _crashSoundCurrentCooldown = _crashSoundCooldown;
            Instantiate(_crashSound, transform.position, Quaternion.identity);
        }


    }
        
}
