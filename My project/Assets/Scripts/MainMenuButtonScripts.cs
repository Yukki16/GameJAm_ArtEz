using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonScripts : MonoBehaviour
{
    [SerializeField] GameObject SettingsUI;

    [SerializeField] EventReference ButtonPressed;
    public void StartGame(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        AudioManager.instance.PlayOneShot(ButtonPressed, this.transform.position);
    }

    public void OpenSettings()
    {
        AudioManager.instance.PlayOneShot(ButtonPressed, this.transform.position);
    }

    public void Quit()
    {
        AudioManager.instance.PlayOneShot(ButtonPressed, this.transform.position);
        Application.Quit();
        Debug.Log("Quited");
    }

    public void Resume()
    {

    }
}
