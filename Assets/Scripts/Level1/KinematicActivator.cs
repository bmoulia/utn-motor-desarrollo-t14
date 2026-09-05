using UnityEngine;

public class KinematicActivator : MonoBehaviour
{
    
    [SerializeField] private bool alreadyActivated = false;
    [SerializeField] private GameObject requiredItem;
    [SerializeField] private GameObject activateItem;
    private Rigidbody rb;


    private void OnTriggerExit(Collider other)
    {
        if (!alreadyActivated)
        {
            if (other.gameObject == requiredItem)
            {
                rb = activateItem.GetComponent<Rigidbody>();
                alreadyActivated = true;
                rb.isKinematic = false;
            }
        }
    }
}
