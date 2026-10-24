using JetBrains.Annotations;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool alreadyTriggered = false;
    [SerializeField] private GameObject requiredItem;
    [SerializeField] private string animationStateName;
    [SerializeField] private AudioSource thisAudio;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private GameObject keyPlaced;

        

     void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyTriggered)
        {
            if (other.gameObject == requiredItem)
            {
                MeshRenderer thisRender = GetComponent<MeshRenderer>();
                alreadyTriggered = true;
                animator.Play(animationStateName, 0, 0.0f);
                other.gameObject.SetActive(false);
                particle.gameObject.SetActive(true);
                keyPlaced.SetActive(true);
                thisAudio.Play();
                thisRender.enabled = false;
            }
        }
    }
}
