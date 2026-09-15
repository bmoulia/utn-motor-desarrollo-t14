using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        // Si el juego está en pausa, no toco el cursor: manda el PauseMenu.
        if (Time.timeScale == 0f)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}