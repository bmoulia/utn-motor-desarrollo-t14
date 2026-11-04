using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _normalMusicClip;
    [SerializeField] private AudioClip _combatMusicClip;
    [SerializeField] private AudioClip _victoryMusicClip;
    [SerializeField] private AudioClip _gameOverMusicClip;

    [SerializeField] private AudioMixerSnapshot _normalSnapshot;
    [SerializeField] private AudioMixerSnapshot _combatSnapshot;
    [SerializeField] private AudioMixerSnapshot _lowHealthSnapshot;
    [SerializeField] private AudioMixerSnapshot _otherSnapshot;

    [SerializeField] private float _transitionTime = 1f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCombatMusic()
    {
        if (_audioSource.clip != _combatMusicClip)
        {
            _combatSnapshot.TransitionTo(_transitionTime);
            _audioSource.clip = _combatMusicClip;
            _audioSource.Play();
        }
    }
    public void PlayNormalMusic()
    {
        if (_audioSource.clip != _normalMusicClip)
        {
            _normalSnapshot.TransitionTo(_transitionTime);
            _audioSource.clip = _normalMusicClip;
            _audioSource.Play();
        }
    }
    
    public void PlayVictoryMusic()
    {
        if (_audioSource.clip != _victoryMusicClip)
        {
            _otherSnapshot.TransitionTo(_transitionTime);
            _audioSource.clip = _victoryMusicClip;
            _audioSource.Play();
        }
    }

    public void PlayGameOverMusic()
    {
        if (_audioSource.clip != _gameOverMusicClip)
        {
            _otherSnapshot.TransitionTo(_transitionTime);
            _audioSource.clip = _gameOverMusicClip;
            _audioSource.Play();
        }
    }

    public void PlayLowHealthSnapshot()
    {
        _lowHealthSnapshot.TransitionTo(_transitionTime);
    }  
}
