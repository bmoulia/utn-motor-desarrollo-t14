using UnityEngine;

public class AudioAnim : MonoBehaviour
{
    public AudioSource miAudio;

    public void PlayTrenAudio()
    {
        if (miAudio != null) miAudio.Play();
    }
}
