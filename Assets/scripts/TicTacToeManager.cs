using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeAutoBind : MonoBehaviour
{
    public enum Player { None, X, O }

    public Button[] cells;   // 9 כפתורים

    public Color xColor = Color.red;
    public Color oColor = Color.blue;
    public Color emptyColor = Color.white;

    private Player[,] board = new Player[3, 3];

    private Queue<Vector2Int> xPieces = new Queue<Vector2Int>();
    private Queue<Vector2Int> oPieces = new Queue<Vector2Int>();

    private Player currentPlayer = Player.X;
    private bool gameOver = false;

    void Start()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            int index = i;
            board[i / 3, i % 3] = Player.None;

            SetCellColor(i, emptyColor);

            cells[i].onClick.AddListener(() => OnCellClicked(index));
        }

        Debug.Log("Game Started - X begins");
    }

    void OnCellClicked(int index)
    {
        if (gameOver) return;

        int row = index / 3;
        int col = index % 3;

        if (board[row, col] != Player.None)
            return;

        Queue<Vector2Int> queue =
            currentPlayer == Player.X ? xPieces : oPieces;

        // הצבה לוגית
        board[row, col] = currentPlayer;
        queue.Enqueue(new Vector2Int(row, col));

        // צבע על הכפתור
        SetCellColor(index, GetColor(currentPlayer));

        // בדיקת ניצחון
        if (CheckWin(currentPlayer))
        {
            gameOver = true;
            Debug.Log($"🎉 {currentPlayer} WINS!");
            return;
        }

        // אם יש 3 כלים → מוחקים את הישן ביותר
        if (queue.Count == 3)
        {
            Vector2Int old = queue.Dequeue();

            board[old.x, old.y] = Player.None;

            int oldIndex = old.x * 3 + old.y;
            SetCellColor(oldIndex, emptyColor);
        }

        currentPlayer = currentPlayer == Player.X ? Player.O : Player.X;
        Debug.Log("Turn: " + currentPlayer);
    }

    void SetCellColor(int index, Color color)
    {
        Image img = cells[index].GetComponent<Image>();
        img.color = color;
    }

    Color GetColor(Player p)
    {
        return p == Player.X ? xColor : oColor;
    }

    bool CheckWin(Player p)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i,0]==p && board[i,1]==p && board[i,2]==p) return true;
            if (board[0,i]==p && board[1,i]==p && board[2,i]==p) return true;
        }

        if (board[0,0]==p && board[1,1]==p && board[2,2]==p) return true;
        if (board[0,2]==p && board[1,1]==p && board[2,0]==p) return true;

        return false;
    }
}