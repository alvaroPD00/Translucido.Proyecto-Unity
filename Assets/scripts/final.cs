using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Importar UI para manejar la imagen de transición

public class final : MonoBehaviour
{
    [SerializeField] private AudioSource firstAudioSource;
    [SerializeField] private AudioSource secondAudioSource;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private Image fadeImage; // Imagen de transición a blanco
    [SerializeField] private float fadeDuration = 2f; // Duración del fade en segundos

    private bool isSecondAudioPlaying = false;

    private void Start()
    {
        if (firstAudioSource != null)
        {
            firstAudioSource.loop = true;
            firstAudioSource.Play();
        }

        if (secondAudioSource != null)
        {
            secondAudioSource.loop = false;
            secondAudioSource.Stop();
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false); // Asegurar que inicie desactivada
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador entró en el área del square.");

            if (objectToActivate != null)
            {
                Debug.Log("Activando objeto.");
                objectToActivate.SetActive(true);
            }

            if (firstAudioSource != null && firstAudioSource.isPlaying)
            {
                Debug.Log("Silenciando el primer audio.");
                firstAudioSource.Stop();
            }

            if (secondAudioSource != null && !isSecondAudioPlaying)
            {
                Debug.Log("Reproduciendo el segundo audio.");
                isSecondAudioPlaying = true;
                secondAudioSource.Play();
                StartCoroutine(WaitForAudioToFinish());
            }

            StartCoroutine(FadeToWhite()); // Iniciar la transición
        }
    }

    private System.Collections.IEnumerator WaitForAudioToFinish()
    {
        if (secondAudioSource != null)
        {
            while (secondAudioSource.isPlaying)
            {
                yield return null;
            }

            Debug.Log("Segundo audio terminado. Cambiando de escena.");
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No se ha especificado una escena para cargar.");
        }
    }

    private System.Collections.IEnumerator FadeToWhite()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color color = fadeImage.color;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                fadeImage.color = color;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            color.a = 1;
            fadeImage.color = color;
        }
    }
}
