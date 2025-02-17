using UnityEngine;
using UnityEngine.SceneManagement;

public class Reiniciar : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "NombreDeLaEscena"; // Nombre de la escena a cargar
    [SerializeField] private float holdTimeToQuit = 3f; // Tiempo necesario para salir del juego

    private float escHoldTime = 0f;
    private bool isHoldingEsc = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isHoldingEsc = true;
            escHoldTime = 0f; // Reiniciar el contador
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            escHoldTime += Time.deltaTime;

            if (escHoldTime >= holdTimeToQuit)
            {
                Debug.Log("Cerrando juego...");
                QuitGame();
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isHoldingEsc = false;

            if (escHoldTime < holdTimeToQuit)
            {
                Debug.Log($"Cambiando a la escena: {sceneToLoad}");
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detiene el juego en el editor
#else
        Application.Quit(); // Cierra la aplicación en una build
#endif
    }
}
