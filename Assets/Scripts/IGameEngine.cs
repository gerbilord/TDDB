using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEngine 
{
    public Board board { get; set; }
    public Player player { get; set; }
    public CardManager cardManager { get; set; }
    public WaveManager waveManager { get; set; }
    public Config config { get; set; }

    public void FinishCardTurn_StartWave();

    public void OnWaveEnd_StartCardTurn(); // Current turn increments here.

    public int currentTurnNumber { get; set; }
    
    public void SendCreepToEnemyCorral(CreepPreset creepToSend);
    public void SendCreepToEnemyImmediateSend(CreepPreset creepToSend);
}
