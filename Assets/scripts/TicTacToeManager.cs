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
    public BoardCell[] cells; // 9 המשבצות בלוח

    [Header("Scene Transition Settings")]
    public string winSceneName = "EndScene"; // שם סצינת הסיום
    public float winSceneDelay = 3.0f;       // ה-Offset בשניות עד המעבר לסצינה

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
        Debug.Log($"<color=cyan>--- המשחק התחיל! תור שחקן: {currentPlayer} ---</color>");
    }

    public void OnCellClicked(BoardCell clickedCell)
    {
        if (!isGameActive) return;

        if (!clickedCell.IsEmpty())
        {
            Debug.Log("<color=yellow>המשבצת הזו כבר תפוסה!</color>");
            return;
        }

        StartCoroutine(HandleTurn(clickedCell));
    }

    private IEnumerator HandleTurn(BoardCell cell)
    {
        Queue<CharacterView> currentQueue = (currentPlayer == PlayerType.X) ? playerXPieces : playerOPieces;
        CharacterView prefabToSpawn = (currentPlayer == PlayerType.X) ? playerXPrefab : playerOPrefab;

        // 1. יצירת הדמות החדשה
        CharacterView newCharacter = Instantiate(prefabToSpawn, cell.transform.position, Quaternion.identity);
        newCharacter.owner = currentPlayer;
        cell.SetCharacter(newCharacter);
        currentQueue.Enqueue(newCharacter);

        Debug.Log($"שחקן {currentPlayer} שים כלי במשבצת מספר {cell.cellIndex}");
        newCharacter.PlayEnter();

        // 2. בדיקת ניצחון מיידית
        if (CheckWin(currentPlayer))
        {
            isGameActive = false;
            Debug.Log($"<color=green>=== שחקן {currentPlayer} ניצח במשחק! ===</color>");
            
            yield return new WaitForSeconds(0.5f);
            EndGame(currentPlayer);

            // 3. השהיה לפי ה-Offset שהוגדר, ולאחר מכן מעבר לסצינת הסיום
            yield return new WaitForSeconds(winSceneDelay);
            SceneManager.LoadScene(winSceneName);
            yield break;
        }

        // 4. אם אין ניצחון והשחקן הציב כלי שלישי - העלמת הכלי הישן
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

            Debug.Log($"<color=orange>שחקן {currentPlayer} הציב כלי שלישי ללא ניצחון - הכלי הישן שלו נעלם מהלוח!</color>");
            oldestPiece.PlayExit();
        }

        // 5. החלפת תור
        currentPlayer = (currentPlayer == PlayerType.X) ? PlayerType.O : PlayerType.X;
        Debug.Log($"<color=cyan>--- תור שחקן: {currentPlayer} ---</color>");
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
                    character.PlayWin();
                }
                else
                {
                    character.PlayExit();
                }
            }
        }
    }
}