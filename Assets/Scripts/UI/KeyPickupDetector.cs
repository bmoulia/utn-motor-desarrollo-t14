using UnityEngine;

public class KeyPickupDetector : MonoBehaviour
{
    public ObjectiveUI objectiveUI;
    public Rigidbody key;

    private Vector3 initialPosition;
    private bool pickedUp = false;

    private void Start()
    {
        if (key != null)
            initialPosition = key.position;
    }

    private void Update()
    {
        if (pickedUp || key == null || objectiveUI == null)
            return;

        float distanceMoved =
            Vector3.Distance(key.position, initialPosition);

        if (distanceMoved > 0.5f)
        {
            pickedUp = true;
            objectiveUI.CompleteObjective();
        }
    }
}