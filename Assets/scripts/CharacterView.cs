using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    private Animator animator;
    public TicTacToeManager.PlayerType owner;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // הפונקציה החדשה לניצחון "חי" יותר
    public void PlayWinWithOffset()
    {
        StartCoroutine(WinSequence());
    }

    private IEnumerator WinSequence() 
    {
    float randomDelay = Random.Range(0f, 0.4f);
    yield return new WaitForSeconds(randomDelay);
    if (animator != null) animator.Play("Win");
    } 

    // שאר הפונקציות נשארות אותו דבר...
    public void PlayEnter() => animator?.Play("Enter");
    public void PlayIdle() => animator?.Play("Idle");
    public void PlayExit() => animator?.Play("Exit");
    public void PlayLose() => animator?.Play("Lose");
    public void DestroySelf() => Destroy(gameObject);
    public void ExitFinished() => DestroySelf();
    public void GoToIdle() => PlayIdle();
}