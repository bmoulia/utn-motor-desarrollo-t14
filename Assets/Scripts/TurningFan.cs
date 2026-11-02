using UnityEngine;
using UnityEngine.UIElements;

public class TurningFan : MonoBehaviour
{
    [SerializeField] private float velocidadRotacion = 50f;
    [SerializeField] private Vector3 ejesDeRotacion = new Vector3(0f, 1f, 0f);

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(ejesDeRotacion * velocidadRotacion * Time.deltaTime, Space.World);
    }
}
