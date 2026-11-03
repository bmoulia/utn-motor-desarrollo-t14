
using UnityEngine;

public class BookContactDetector : MonoBehaviour
{
    public ObjectiveUI objectiveUI;

    private bool messageShown = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("BOOK DETECTOR: contacto con " + other.name);
        if (messageShown || objectiveUI == null)
            return;

        Transform playerTransform = other.transform.root;

        if (!playerTransform.CompareTag("Player") &&
            !other.CompareTag("Player"))
            return;

        messageShown = true;
        objectiveUI.ShowDynamiteObjective();
    }
}
