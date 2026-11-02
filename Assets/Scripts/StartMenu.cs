using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPanel.SetActive(true);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
   public void Jugar()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
