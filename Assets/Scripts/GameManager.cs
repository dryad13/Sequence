using System.Collections.Generic;
using UnityEngine;
using TMPro;  // For displaying the current player's turn and game state
public class PlayerPiece : MonoBehaviour
{
    public int playerID;  // This will link the piece to a specific player
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // UI references
    public TextMeshProUGUI turnIndicator;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    // Player data
    public int numberOfPlayers = 2;
    private List<Player> players;
    private int currentPlayerIndex = 0;
    private int currentPlayerScore = 0;

    // Board data
    public GameObject[] playerPrefabs;  // The peg prefabs for each player (assign in Inspector)
    public Transform board;  // Reference to the board in the scene
    private List<List<CellHandler>> gridCells;

    // Game state
    private bool gameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep the GameManager object between scenes
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicates of GameManager
        }
    }
    public void StartGame(int numberOfPlayers)
    {
        // Set the number of players and initialize the game
        this.numberOfPlayers = numberOfPlayers;
        InitializeGame();
    }

    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        players = new List<Player>();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players.Add(new Player($"Player {i + 1}"));
        }

        // Set up the grid based on your 3D board
        SetUpBoard();

        UpdateTurnIndicator();
    }

    // Set up the board, link all the cells to the game
    private void SetUpBoard()
    {
        gridCells = new List<List<CellHandler>>();

        // Assuming the board is a grid of GameObjects with attached CellHandler components
        foreach (Transform child in board)
        {
            if (child.TryGetComponent<CellHandler>(out CellHandler cellHandler))
            {
                int x = Mathf.FloorToInt(child.position.x);  // Set grid coordinates
                int y = Mathf.FloorToInt(child.position.z);

                cellHandler.x = x;
                cellHandler.y = y;
                gridCells.Add(new List<CellHandler> { cellHandler });
            }
        }
    }

    // Update turn indicator UI
    private void UpdateTurnIndicator()
    {
        turnIndicator.text = $"Player {currentPlayerIndex + 1}'s Turn";
    }

    // Place a player's peg in a cell
    public void PlacePeg(Vector3 cellPosition, int playerId)
    {
        if (gameOver) return;

        GameObject peg = Instantiate(playerPrefabs[playerId], cellPosition, Quaternion.identity);
        peg.GetComponent<PlayerPiece>().playerID = playerId;
    }

    // Check for a winning sequence (horizontal, vertical, diagonal)
    public bool CheckSequence(Vector3 cellPosition, int playerId)
    {
        // Check horizontally, vertically, and diagonally for a sequence of 5 pegs

        // Horizontal check
        int sequenceCount = 1;
        for (int x = -1; x <= 1; x += 2)
        {
            sequenceCount += CountSequence(cellPosition, x, 0, playerId);
        }

        if (sequenceCount >= 5) return true;

        // Vertical check
        sequenceCount = 1;
        for (int y = -1; y <= 1; y += 2)
        {
            sequenceCount += CountSequence(cellPosition, 0, y, playerId);
        }

        if (sequenceCount >= 5) return true;

        // Diagonal check (positive slope)
        sequenceCount = 1;
        for (int d = -1; d <= 1; d += 2)
        {
            sequenceCount += CountSequence(cellPosition, d, d, playerId);
        }

        if (sequenceCount >= 5) return true;

        // Diagonal check (negative slope)
        sequenceCount = 1;
        for (int d = -1; d <= 1; d += 2)
        {
            sequenceCount += CountSequence(cellPosition, d, -d, playerId);
        }

        if (sequenceCount >= 5) return true;

        return false;
    }

    // Count the number of consecutive pegs in the direction specified (dx, dy)
    private int CountSequence(Vector3 startPos, int dx, int dy, int playerId)
    {
        int count = 0;
        Vector3 direction = new Vector3(dx, 0, dy);

        for (int i = 1; i <= 4; i++)
        {
            Vector3 checkPos = startPos + (direction * i);
            if (CheckCellForPlayer(checkPos, playerId))
            {
                count++;
            }
            else
            {
                break;
            }
        }

        return count;
    }

    // Check if a given position contains the player's peg
    private bool CheckCellForPlayer(Vector3 position, int playerId)
    {
        foreach (var cell in gridCells)
        {
            if (cell.position == position)
            {
                if (cell.peg != null && cell.peg.playerID == playerId)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }

    // Update the score of the player
    public void UpdateScore(int playerId)
    {
        if (playerId == 0)
        {
            players[0].Score++;
        }
        else
        {
            players[1].Score++;
        }

        scoreText.text = $"Player 1: {players[0].Score} - Player 2: {players[1].Score}";
    }

    // Check if a player has won the game (2 points to win)
    public bool CheckWinner(int playerId)
    {
        if (playerId == 0 && players[0].Score >= 2)
        {
            EndGame(playerId);
            return true;
        }
        else if (playerId == 1 && players[1].Score >= 2)
        {
            EndGame(playerId);
            return true;
        }

        return false;
    }

    // End the game and display the winner
    private void EndGame(int playerId)
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
        gameOverText.text = $"Player {playerId + 1} Wins!";
    }

    // Switch to the next player's turn
    public void NextPlayerTurn()
    {
        if (gameOver) return;

        currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
        UpdateTurnIndicator();
    }

}

// Player class to hold player-specific data
public class Player
{
    public string Name { get; private set; }
    public int Score { get; set; }

    public Player(string name)
    {
        Name = name;
        Score = 0;
    }
}
