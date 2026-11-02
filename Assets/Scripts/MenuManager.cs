using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private string gameScene;


    public void IniciarJuego()
    {
        SceneManager.LoadScene(gameScene);
    }


    public void SalirDelJuego()
    {
        Application.Quit();
    }


}
