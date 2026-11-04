using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private GameObject objectivePUPanel;
    [SerializeField] private GameObject healthPanel;


    private void Start()
    {
        gameOverScreen.SetActive(false);

    }
     void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Colisione");
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivarPantallaYPausar();
            GameManager._instance.GetUpdateGameStatus("GameStatus", "Victory");
        }
    } 
private void ActivarPantallaYPausar()
{
    // 1. Mostramos la pantalla en la interfaz
    if (gameOverScreen != null)
    {
        gameOverScreen.SetActive(true);
        objectivePanel.SetActive(false);
        objectivePUPanel.SetActive(false);
        healthPanel.SetActive(false);
        }

    // 2. Pausamos el juego por completo (frenamos el tiempo físico y de animaciones)
    Time.timeScale = 0f;

    // 3. Desbloqueamos el cursor por si necesitas hacer clic en algún botón de la pantalla
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//Regargo la misma escena donde se activa automaticamente el menu
    }

}
