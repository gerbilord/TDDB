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
}
