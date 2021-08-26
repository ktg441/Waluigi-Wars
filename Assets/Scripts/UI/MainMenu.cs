using UnityEngine;

public class MainMenu : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            TryQuit();
        }
    }

    public void StartGame()
    {

    }

    public void GoToSettings()
    {

    }

    public void ExitGame()
    {
        TryQuit();
    }

    private void TryQuit()
    {
        // Pop up confirmation window
        Application.Quit();
    }

}
