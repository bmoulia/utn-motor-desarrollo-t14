using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public GameObject gameOverScreen;

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
        }
    } 
private void ActivarPantallaYPausar()
{
    // 1. Mostramos la pantalla en la interfaz
    if (gameOverScreen != null)
    {
        gameOverScreen.SetActive(true);
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
