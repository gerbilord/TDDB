/**
 * Interface for effects that can be played.
 * Also used on the Card itself.
 *
 * So a card can implement IPlayEffects directly,
 * or have a list of IPlayEffects it calls.
 * 
 */
public interface IPlayEffects: IHasIGameEngine
{
    /**
     * Returns true if the effect was played successfully, false otherwise.
     */
    public CardPlayResult Play(ICell cell);


    public CardPlayResult UI_OnCellEntered(ICell cell)
    {
        // default no-op
        return CardPlayResult.IGNORE;
    }
    
    public CardPlayResult UI_OnCellExited(ICell cell)
    {
        // default no-op
        return CardPlayResult.IGNORE;
    }
    
    public CardPlayResult UI_OnCellClicked(ICell cell)
    {
        // default no-op
        return CardPlayResult.IGNORE;
    }

    public CardPlayResult UI_OnCardSelected()
    {
        // default no-op
        return CardPlayResult.IGNORE;
    }
    
    public CardPlayResult UI_OnCardDeselected()
    {
        // default no-op
        return CardPlayResult.IGNORE;
    }

}
