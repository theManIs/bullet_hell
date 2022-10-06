using System;

public class HealthSystemEventArguments : EventArgs
{
    public EventTypeSet EventType = EventTypeSet.Damage;
    public int DamageAmount = 0;
}
