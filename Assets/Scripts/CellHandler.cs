using UnityEngine;

public class CellHandler : MonoBehaviour
{
    private GameManager gameManager;   // Reference to the GameManager
    public int x; // X coordinate of this cell
    public int y; // Y coordinate of this cell
    private bool isOccupied = false;  // To track whether the cell is occupied by a peg
    public int GetCurrentPlayerId()
    {
        return currentPlayerIndex;
    }
    void Start()
    {
        peg = null;
        // Find and assign the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not found in the scene!");
        }
    }

    void OnMouseDown()
    {
        // Ensure the GameManager reference is valid
        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing in CellHandler!");
            return;
        }

        // Check if the cell is already occupied by a peg
        if (isOccupied)
        {
            Debug.Log("Cell is already occupied! Please choose an empty cell.");
            return;
        }

        // Get the current player's ID
        int playerId = gameManager.GetCurrentPlayerId();

        // Place the player's peg in this cell (you could define how pegs are placed in GameManager)
        Vector3 cellPosition = transform.position; // Get the 3D position of the cell
        gameManager.PlacePeg(cellPosition, playerId); // Call GameManager's PlacePeg method to spawn the peg

        // Mark the cell as occupied
        isOccupied = true;

        // Check for a sequence (horizontal, vertical, diagonal)
        if (gameManager.CheckSequence(cellPosition, playerId))
        {
            // If the player makes a sequence, update the score and check if they win
            gameManager.UpdateScore(playerId);
            if (gameManager.CheckWinner(playerId))
            {
                gameManager.EndGame(playerId);  // End the game if the player reaches 2 points
            }
        }

        // Proceed to the next player's turn
        gameManager.NextPlayerTurn();

        // Log cell click (optional)
        Debug.Log($"Cell clicked: {gameObject.name} at position ({x}, {y}) by Player {playerId}");
    }

    // Reset this cell when the game is reset or restarted
    public void ResetCell()
    {
        isOccupied = false;
    }
    public void SetPeg(PlayerPiece newPeg)
    {
        peg = newPeg;
    }

    // Check if this cell has a peg
    public bool HasPeg()
    {
        return peg != null;
    }
}
