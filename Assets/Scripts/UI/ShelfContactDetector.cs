using UnityEngine;

public class ShelfContactDetector : MonoBehaviour
{
    public ObjectiveUI objectiveUI;

    private bool messageShown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (messageShown || objectiveUI == null)
            return;

        Transform playerTransform = other.transform.root;

        if (!playerTransform.CompareTag("Player") &&
            !other.CompareTag("Player"))
            return;

        messageShown = true;
        objectiveUI.CompleteObjective();
    }
}