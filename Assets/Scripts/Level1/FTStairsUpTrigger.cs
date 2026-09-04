using UnityEngine;

public class FTStairsUpTrigger : MonoBehaviour
{
    [SerializeField] private Animator stairsAnimator;
    [SerializeField] private bool stairsUp = false;
    [SerializeField] private GameObject batery;

    private void OnTriggerEnter(Collider other)
    {
        if (!stairsUp)
        {
          if (other.gameObject == batery     )
             {
                stairsUp = true;
                stairsAnimator.Play("FireTruckStairsUp", 0, 0.0f);
             }
        }
    }
}
