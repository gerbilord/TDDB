public class EventManager
{
    public CellEventManager cellEventManager { get; }
    public CardEventManager cardEventManager { get; }
    
    public EventManager()
    {
        cellEventManager = new CellEventManager();
        cardEventManager = new CardEventManager();
    }
}
