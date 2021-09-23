using System;
using UnityEngine;

public enum GameState
{
    Playing,
    Paused
}
public static class GameStatus 
{
    public static GameState CurrentState { get; private set; }
    public static void ChangeGameStatus(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                CurrentState = GameState.Playing;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                CurrentState = GameState.Paused;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                Time.timeScale = 0f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
