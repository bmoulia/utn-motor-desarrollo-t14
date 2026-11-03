using UnityEngine;
using TMPro;
using System.Collections;

public class BatteryDeliveryZone : MonoBehaviour
{
    public ObjectiveUI objectiveUI;
    public Rigidbody targetBattery;

    private bool completed = false;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody detected = other.GetComponent<Rigidbody>();
        
        if (completed || targetBattery == null || objectiveUI == null)
            return;

        if (detected != targetBattery)
            return;

        completed = true;
        objectiveUI.ShowBridgeObjective();
    }
}