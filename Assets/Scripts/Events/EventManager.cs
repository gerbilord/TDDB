public class EventManager
{
    public CellEventManager cellEventManager { get; }
    public CardEventManager cardEventManager { get; }
    
    public CreepEventManager creepEventManager { get; }
    
    public TowerEventManager towerEventManager { get; }
    public BulletEventManager bulletEventManager { get; }
    
    public EventManager()
    {
        cellEventManager = new CellEventManager();
        cardEventManager = new CardEventManager();
        creepEventManager = new CreepEventManager();
        towerEventManager = new TowerEventManager();
        bulletEventManager = new BulletEventManager();
    }
}
