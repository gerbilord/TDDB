using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// Create asset menu
[CreateAssetMenu(fileName = "AI_SendExtraCreepEffect", menuName = "ScriptableObjects/CardEffects/AI_SendExtraCreepEffect", order = 1)]
public class AI_SendExtraCreepEffect : ScriptableObject, IPlayEffects
{
    [DoNotSerialize]
    public IGameEngine gameEngine { get; set; }

    public bool sendImmediately = false;

    [SerializeField]
    private CreepPreset creepPreset;
    public CardPlayResult Play()
    {
        ICell cellToSendTo = gameEngine.board.GetCorralCellAt(0, 0);
        if (sendImmediately)
        {
            cellToSendTo = gameEngine.board.GetImmediateSendCellAt(0, 0);
        }

        // Create a SendCreepEffect
        AddCreepToCorralEffect sendCreepEffect = ScriptableObject.CreateInstance<AddCreepToCorralEffect>();
        sendCreepEffect.gameEngine = gameEngine; // Always need to do this for AI cards, since the card only set out gameEngine, but not our inner effect.
        sendCreepEffect.InjectPlayData(cellToSendTo, creepPreset);
        sendCreepEffect.Play();

        return CardPlayResult.SUCCESS;
    }
}