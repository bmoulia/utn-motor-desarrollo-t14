
using UnityEngine;

public class BatteryPickupDetector : MonoBehaviour
{
    public ObjectiveUI objectiveUI;
    public Rigidbody battery;

    private Vector3 initialPosition;
    private bool pickedUp = false;

    private void Start()
    {
        if (battery != null)
            initialPosition = battery.position;
    }

    private void Update()
    {
        if (pickedUp || battery == null || objectiveUI == null)
            return;

        float distanceMoved =
            Vector3.Distance(battery.position, initialPosition);

        if (distanceMoved > 0.5f)
        {
            pickedUp = true;
            objectiveUI.CompleteObjective();
        }
    }
}
