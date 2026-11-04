using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private GameObject healthPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPanel.SetActive(true);
        objectivePanel.SetActive(false);
        healthPanel.SetActive(false);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
   public void Jugar()
    {
        startPanel.SetActive(false);
        objectivePanel.SetActive(true);
        healthPanel.SetActive(true);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
