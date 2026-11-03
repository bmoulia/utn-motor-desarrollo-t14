using UnityEngine;

public class DynamitePickupDetector : MonoBehaviour
{
    public ObjectiveUI objectiveUI;
    public Rigidbody dynamite;

    private Vector3 initialPosition;
    private bool pickedUp = false;

    private void Start()
    {
        if (dynamite != null)
            initialPosition = dynamite.position;
    }

    private void Update()
    {
        if (pickedUp || dynamite == null || objectiveUI == null)
            return;

        float distanceMoved =
            Vector3.Distance(dynamite.position, initialPosition);

        if (distanceMoved > 0.5f)
        {
            pickedUp = true;
            objectiveUI.CompleteObjective();
        }
    }
}