using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStates
{
    public static State state = State.None;
    public enum State
    {
        None,
        GameStart
    }

    public static void ChangeState(State _newState)
    {
        state = _newState;
        Debug.Log("state chagned");
    }
}
