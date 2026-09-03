using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensibility = 200f;
    public float rotationX = 0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensibility * Time.deltaTime;
        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, -25f, 25f);

        //segun la docu Quaternion hace que rote rotationX grados en el eje X, y nada en Y ni Z
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}
