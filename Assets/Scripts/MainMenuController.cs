using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;
    public GameObject playerSelectionUI;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void OnPlayButtonClicked()
    {
        playerSelectionUI.SetActive(true); // Enable the player selection UI
    }

    void OnExitButtonClicked()
    {
        Application.Quit();  // Close the game
    }
}
