using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// Create asset menu
[CreateAssetMenu(fileName = "BuildTowerAtEffect", menuName = "ScriptableObjects/CardEffects/BuildTowerAtEffect", order = 1)]
public class BuildTowerAtEffect : ScriptableObject, IPlayEffects
{
    [DoNotSerialize]
    public IGameEngine gameEngine { get; set; }

    public int x;
    public int y;

    [SerializeField]
    private TowerPreset towerPreset;
    
    public CardPlayResult Play()
    {
        ICell cellToBuildOn = gameEngine.board.GetMainBoardCellAt(x, y);
        if (cellToBuildOn.type == CellType.Tree)
        {
            cellToBuildOn.type = CellType.Grass;
            cellToBuildOn.UpdateCellPrefabToMatchType();

            cellToBuildOn = gameEngine.board.GetMainBoardCellAt(x, y); // re-get the new cell, since it changed.
        }

        // Create a BuildTowerEffect
        BuildTowerEffect buildTowerEffect = ScriptableObject.CreateInstance<BuildTowerEffect>();
        buildTowerEffect.gameEngine = gameEngine; // Always need to do this for AI cards, since the card only set out gameEngine, but not our inner effect.
        buildTowerEffect.InjectPlayData(cellToBuildOn, towerPreset);
        buildTowerEffect.Play();

        return CardPlayResult.SUCCESS;
    }
}