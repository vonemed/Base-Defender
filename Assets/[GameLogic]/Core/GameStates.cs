using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Everything regarding game status and state
public static class GameStates
{
    
}

public interface IStartGameListener
{
    void OnStartGame();
}

public interface IPauseGameListener
{
    void OnPauseGame();
}

public interface IResumeGameListener
{
    void OnResumeGame();
}

public interface IFinishGameListener
{
    void OnFinishGame();
}