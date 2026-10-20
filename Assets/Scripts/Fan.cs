using UnityEngine;

public class Fan : MonoBehaviour
{
    public float velocidad = 180f;

    void Update()
    {
        transform.Rotate(0f, velocidad * Time.deltaTime, 0f, Space.Self);
    }
}
