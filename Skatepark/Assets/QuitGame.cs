using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        // Exits the game
        Application.Quit();
        Debug.Log("Game Quit"); // This log is only visible in the editor
    }
}
