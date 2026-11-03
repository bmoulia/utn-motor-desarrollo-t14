
using UnityEngine;

public class BridgeMovementDetector : MonoBehaviour
{
    public ObjectiveUI objectiveUI;
    public Transform carTrack;
    public float movementThreshold = 0.1f;

    private Vector3 initialPosition;
    private bool messageShown = false;

    private void Start()
    {
        if (carTrack != null)
            initialPosition = carTrack.position;
    }

    private void Update()
    {
        if (messageShown || carTrack == null || objectiveUI == null)
            return;

        float distanceMoved =
            Vector3.Distance(carTrack.position, initialPosition);

        if (distanceMoved >= movementThreshold)
        {
            messageShown = true;
            objectiveUI.ShowBridgeBlocksObjective();
        }
    }
}
