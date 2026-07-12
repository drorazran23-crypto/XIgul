using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    private Animator animator;

    // הגדרה למי שייכת הדמות הזו
    public TicTacToeManager.PlayerType owner;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayEnter()
    {
        if (animator != null) animator.Play("Enter");
    }

    public void PlayIdle()
    {
        if (animator != null) animator.Play("Idle");
    }

    public void PlayExit()
    {
        if (animator != null) animator.Play("Exit");
    }

    public void PlayWin()
    {
        if (animator != null) animator.Play("Win");
    }

    public void PlayLose()
    {
        if (animator != null) animator.Play("Lose");
    }

    // נקרא בסוף אנימציית Exit
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    // נקרא בסוף אנימציית Enter
    public void GoToIdle()
    {
        PlayIdle();
    }
}