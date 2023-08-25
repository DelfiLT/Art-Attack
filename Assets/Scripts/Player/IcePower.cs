using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperMaxim.Messaging;

public class IcePower : Power
{
    public override void Use()
    {
        base.Use();
        var message = new ElementChange(Element.Ice);
        Messenger.Default.Publish(message);
    }
}
