using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard : IPlayEffects
{
    public GameObject GetGameObject();
    
    public List<IPlayEffects> playEffects { get; set; }

}
