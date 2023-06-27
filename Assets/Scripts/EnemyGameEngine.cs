using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EnemyGameEngine : MonoBehaviour, IGameEngine
{
    public Board board { get; set; }
    public Player player { get; set; }
    public CardManager cardManager { get; set; }
    public WaveManager waveManager { get; set; }
    public Config config { get; set; }

    public void Setup()
    {
        GlobalVariables.enemyGameEngine = this;
        config = GameObject.Find("EnemyConfig").GetComponent<Config>();
    }
}