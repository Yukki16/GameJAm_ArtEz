using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonScripts : MonoBehaviour
{
    [SerializeField] GameObject SettingsUI;
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings()
    {

    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quited");
    }

    public void Resume()
    {

    }
}
