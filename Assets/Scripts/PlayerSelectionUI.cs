using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionUI : MonoBehaviour
{
    public Button twoPlayerButton;
    public Button threePlayerButton;
    public Button fourPlayerButton;
    public GameManager gameManager;

    void Start()
    {
        twoPlayerButton.onClick.AddListener(() => StartGame(2));
        threePlayerButton.onClick.AddListener(() => StartGame(3));
        fourPlayerButton.onClick.AddListener(() => StartGame(4));
    }

    void StartGame(int playerCount)
    {
        gameManager.StartGame(playerCount);
        gameObject.SetActive(false); // Hide the player selection UI
    }
}
