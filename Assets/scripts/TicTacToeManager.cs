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

    private bool nextXFlipped = false;
    private bool nextOFlipped = false;

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
    // 1. הגדרת התור והפריפאב הנכון לפי השחקן הנוכחי
    Queue<CharacterView> currentQueue = (currentPlayer == PlayerType.X) ? playerXPieces : playerOPieces;
    CharacterView prefabToSpawn = (currentPlayer == PlayerType.X) ? playerXPrefab : playerOPrefab;

    // 2. יצירת הדמות החדשה במשבצת שנלחצה
    CharacterView newCharacter = Instantiate(prefabToSpawn, cell.transform.position, Quaternion.identity);
    newCharacter.owner = currentPlayer;
    cell.SetCharacter(newCharacter);
    currentQueue.Enqueue(newCharacter);

    // 3. לוגיקת הפליפ (Flipping Logic) - אחת רגילה, אחת הפוכה
    bool shouldFlip = (currentPlayer == PlayerType.X) ? nextXFlipped : nextOFlipped;
    if (shouldFlip)
    {
        newCharacter.transform.localScale = new Vector3(-1, 1, 1);
    }
    
    // עדכון המצב לפעם הבאה של השחקן (מ-True ל-False ולהיפך)
    if (currentPlayer == PlayerType.X) nextXFlipped = !nextXFlipped;
    else nextOFlipped = !nextOFlipped;

    Debug.Log($"Player {currentPlayer} placed a piece. Flipped: {shouldFlip}");
    newCharacter.PlayEnter();

    // 4. בדיקת ניצחון מיידית (לפני שמשהו נעלם)
    if (CheckWin(currentPlayer))
    {
        isGameActive = false;
        Debug.Log($"<color=green>=== Player {currentPlayer} Won! ===</color>");
        
        yield return new WaitForSeconds(0.5f); // המתנה קלה לסיום אנימציית הכניסה
        EndGame(currentPlayer);

        yield return new WaitForSeconds(winSceneDelay); // המתנה לפני מעבר סצנה
        SceneManager.LoadScene(winSceneName);
        yield break; // עוצרים כאן - המשחק נגמר
    }

    // 5. אם אין ניצחון והשחקן הציב את הכלי השלישי שלו - הכלי הישן ביותר נעלם
    if (currentQueue.Count == 3)
    {
        CharacterView oldestPiece = currentQueue.Dequeue();
        
        // מציאת המשבצת שהכלי הישן ישב עליה וריקונה כדי שנוכל ללחוץ עליה שוב
        foreach (var c in cells)
        {
            if (c.GetCharacter() == oldestPiece)
            {
                c.ClearCell();
                break;
            }
        }

        Debug.Log($"<color=orange>Player {currentPlayer} placed 3rd piece without winning. Removing oldest piece.</color>");
        oldestPiece.PlayExit(); // הכלי עושה אנימציית יציאה ומשמיד את עצמו
    }

    // 6. החלפת תור לשחקן הבא
    currentPlayer = (currentPlayer == PlayerType.X) ? PlayerType.O : PlayerType.X;
    Debug.Log($"<color=cyan>--- Current Turn: {currentPlayer} ---</color>");
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
