using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TicTacToeManager : MonoBehaviour
{
    public enum PlayerType { X, O }

    [Header("Prefabs")]
    public CharacterView playerXPrefab;
    public CharacterView playerOPrefab;

    [Header("Board Setup")]
    public BoardCell[] cells; 

    [Header("Scene Transition Settings")]
    public string winSceneName = "EndScene"; 
    public float winSceneDelay = 3.0f;       

    private PlayerType currentPlayer = PlayerType.X;
    private bool isGameActive = true;

    private Queue<CharacterView> playerXPieces = new Queue<CharacterView>();
    private Queue<CharacterView> playerOPieces = new Queue<CharacterView>();

    private readonly int[][] winPatterns = new int[][]
    {
        new int[] {0, 1, 2}, new int[] {3, 4, 5}, new int[] {6, 7, 8},
        new int[] {0, 3, 6}, new int[] {1, 4, 7}, new int[] {2, 5, 8},
        new int[] {0, 4, 8}, new int[] {2, 4, 6}
    };

    void Start()
    {
        Debug.Log($"<color=cyan>--- Game Started! Player Turn: {currentPlayer} ---</color>");
    }

    public void OnCellClicked(BoardCell clickedCell)
    {
        if (!isGameActive) return;

        if (!clickedCell.IsEmpty())
        {
            Debug.Log("<color=yellow>Cell is already occupied!</color>");
            return;
        }

        StartCoroutine(HandleTurn(clickedCell));
    }

    private IEnumerator HandleTurn(BoardCell cell)
    {
        Queue<CharacterView> currentQueue = (currentPlayer == PlayerType.X) ? playerXPieces : playerOPieces;
        CharacterView prefabToSpawn = (currentPlayer == PlayerType.X) ? playerXPrefab : playerOPrefab;

        // 1. Spawn the new character
        CharacterView newCharacter = Instantiate(prefabToSpawn, cell.transform.position, Quaternion.identity);
        newCharacter.owner = currentPlayer;
        cell.SetCharacter(newCharacter);
        currentQueue.Enqueue(newCharacter);

        Debug.Log($"Player {currentPlayer} placed a piece in cell {cell.cellIndex}");
        newCharacter.PlayEnter();

        // 2. Immediate Win Check
        if (CheckWin(currentPlayer))
        {
            isGameActive = false;
            Debug.Log($"<color=green>=== Player {currentPlayer} Won the Game! ===</color>");
            
            yield return new WaitForSeconds(0.5f);
            EndGame(currentPlayer);

            yield return new WaitForSeconds(winSceneDelay);
            SceneManager.LoadScene(winSceneName);
            yield break;
        }

        // 3. If no win and player placed their 3rd piece - Remove oldest piece
        if (currentQueue.Count == 3)
        {
            CharacterView oldestPiece = currentQueue.Dequeue();
            
            foreach (var c in cells)
            {
                if (c.GetCharacter() == oldestPiece)
                {
                    c.ClearCell();
                    break;
                }
            }

            Debug.Log($"<color=orange>Player {currentPlayer} placed 3rd piece (no win) - Oldest piece removed!</color>");
            oldestPiece.PlayExit();
        }

        // 4. Switch Turn
        currentPlayer = (currentPlayer == PlayerType.X) ? PlayerType.O : PlayerType.X;
        Debug.Log($"<color=cyan>--- Player Turn: {currentPlayer} ---</color>");
    }

    private bool CheckWin(PlayerType player)
    {
        foreach (var pattern in winPatterns)
        {
            bool hasWin = true;
            foreach (int index in pattern)
            {
                CharacterView charInCell = cells[index].GetCharacter();
                if (charInCell == null || charInCell.owner != player)
                {
                    hasWin = false;
                    break;
                }
            }
            if (hasWin) return true;
        }
        return false;
    }

   private void EndGame(PlayerType winner)
    {
        foreach (var cell in cells)
        {
            CharacterView character = cell.GetCharacter();
            if (character != null)
            {
                if (character.owner == winner)
                {
                    // שימוש בפונקציה החדשה עם האופסט
                    character.PlayWinWithOffset();
                }
                else
                {
                    // הכלים של המפסיד יוצאים מהלוח
                    character.PlayExit();
                }
            }
        }
    }
}
