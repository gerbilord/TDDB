using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariablesUtils 
{
    // for each cell, run a function
    public static void ForEachCellInGame(System.Action<ICell> function)
    {
        foreach (ICell allCell in GlobalVariables.playerGameEngine.board.GetAllCells())
        {
            function(allCell);
        }

        if (GlobalVariables.enemyGameEngine == null || GlobalVariables.enemyGameEngine.board == null)
        {
            Debug.Log("Enemy game engine or board is null"); // TODO: Remove once we have a proper enemy game engine.
            return;
        }

        foreach (ICell allCell in GlobalVariables.enemyGameEngine.board.GetAllCells())
        {
            function(allCell);
        }
    }

    public static IGameEngine GetEnemyGameEngine(IGameEngine gameEngine)
    {
        if(gameEngine == GlobalVariables.playerGameEngine)
        {
            return GlobalVariables.enemyGameEngine;
        }
        else if(gameEngine == GlobalVariables.enemyGameEngine)
        {
            return GlobalVariables.playerGameEngine;
        }
        else
        {
            Debug.LogError("Game engine not found");
            return null;
        }
    }
}
