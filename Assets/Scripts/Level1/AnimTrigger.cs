using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool alreadyTriggered = false;
    [SerializeField] private GameObject requiredItem;
    [SerializeField] private string animationStateName;

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyTriggered)
        {
            if (other.gameObject == requiredItem)
            {
                alreadyTriggered = true;
                animator.Play(animationStateName, 0, 0.0f);
            }
        }
    }
}
