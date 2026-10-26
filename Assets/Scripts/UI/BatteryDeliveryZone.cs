
using UnityEngine;

public class BatteryDeliveryZone : MonoBehaviour
{
    public ObjectiveUI objectiveUI;
    public Rigidbody targetBattery;

    private bool completed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (completed || targetBattery == null || objectiveUI == null)
            return;

        if (other.attachedRigidbody != targetBattery)
            return;

        completed = true;
        objectiveUI.CompleteObjective();
    }
}
