using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassCell : Cell
{
    public override bool IsBuildable()
    {
        return occupyingGameObjects.Count == 0;
    }
}
