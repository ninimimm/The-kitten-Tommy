using UnityEngine.EventSystems;

public class CustomInputModule : StandaloneInputModule
{
    public override void Process()
    {
        bool usedEvent = SendUpdateEventToSelectedObject();

        if (eventSystem.sendNavigationEvents)
        {
            if (!usedEvent)
                usedEvent |= SendMoveEventToSelectedObject();
            if (!usedEvent)
                SendSubmitEventToSelectedObject();
        }

        // Если никакие события не были использованы, отправьте событие ввода от мыши.
        if (!usedEvent)
            ProcessMouseEvent();
    }
}