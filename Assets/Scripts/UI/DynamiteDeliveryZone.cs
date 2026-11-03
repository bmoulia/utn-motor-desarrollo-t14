using UnityEngine;

public class DynamiteDeliveryZone : MonoBehaviour
{
    public ObjectiveUI objectiveUI;
    public Rigidbody targetDynamite;

    private bool completed = false;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody detected = other.GetComponent<Rigidbody>();

        if (completed || targetDynamite == null || objectiveUI == null)
            return;

        if (detected != targetDynamite)
            return;

        completed = true;
        objectiveUI.CompleteObjective();
    }
}