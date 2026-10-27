using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _normalMusicClip;
    [SerializeField] private AudioClip _combatMusicClip;
    void Start()
    {
        _audioSource.clip = _normalMusicClip;
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
