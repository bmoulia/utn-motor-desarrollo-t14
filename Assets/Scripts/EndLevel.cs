using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public GameObject pantallaGameOver;
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            ActivarPantallaYPausar();
        }
    } 
private void ActivarPantallaYPausar()
{
    // 1. Mostramos la pantalla en la interfaz
    if (pantallaGameOver != null)
    {
        pantallaGameOver.SetActive(true);
    }

    // 2. Pausamos el juego por completo (frenamos el tiempo físico y de animaciones)
    Time.timeScale = 0f;

    // 3. Desbloqueamos el cursor por si necesitas hacer clic en algún botón de la pantalla
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}

}
