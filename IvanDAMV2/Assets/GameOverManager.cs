using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void VolverAlInicio()
    {
        SceneManager.LoadScene("MenuPrincipal"); // usa el nombre real de tu escena inicial
    }
}
