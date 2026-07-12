using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{
    public int cellIndex; // מספר המשבצת (0 עד 8)
    private TicTacToeManager gameManager;
    private CharacterView currentCharacter;

    void Start()
    {
        gameManager = FindFirstObjectByType<TicTacToeManager>();
    }

    void OnMouseDown()
    {
        // קריאה למנהל המשחק כשלוחצים על המשבצת
        if (gameManager != null)
        {
            gameManager.OnCellClicked(this);
        }
    }

    public bool IsEmpty()
    {
        return currentCharacter == null;
    }

    public void SetCharacter(CharacterView character)
    {
        currentCharacter = character;
    }

    public CharacterView GetCharacter()
    {
        return currentCharacter;
    }

    public void ClearCell()
    {
        currentCharacter = null;
    }
}