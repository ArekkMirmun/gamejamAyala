using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void CloseGame()
    {
        Application.Quit();
    }
    public void LoadGameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }
}
