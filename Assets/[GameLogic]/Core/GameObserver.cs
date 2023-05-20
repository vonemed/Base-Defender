using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameObserver : MonoBehaviour
{
    private readonly List<object> listeners = new();

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        foreach (var listener in listeners)
        {
            if (listener is IStartGameListener startListener)
            {
                startListener.OnStartGame();
            }
        }
    }

    [ContextMenu("Pause Game")]
    public void PauseGame()
    {
        foreach (var listener in listeners)
        {
            if (listener is IPauseGameListener pauseListener)
            {
                pauseListener.OnPauseGame();
            }
        }
    }

    [ContextMenu("Resume Game")]
    public void ResumeGame()
    {
        foreach (var listener in listeners)
        {
            if (listener is IResumeGameListener resumeListener)
            {
                resumeListener.OnResumeGame();
            }
        }
    }

    [ContextMenu("Finish Game")]
    public void FinishGame()
    {
        foreach (var listener in listeners)
        {
            if (listener is IFinishGameListener finishListener)
            {
                finishListener.OnFinishGame();
            }
        }
    }

    public void AddListener(object listener)
    {
        this.listeners.Add(listener);
    }

    public void RemoveListener(object listener)
    {
        this.listeners.Remove(listener);
    }
}
