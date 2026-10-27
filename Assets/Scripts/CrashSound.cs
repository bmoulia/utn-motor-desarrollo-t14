using UnityEngine;

public class CrashSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _crashClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource.clip = _crashClip;
        _audioSource.Play();
        Destroy(gameObject, _crashClip.length);
    }

}
