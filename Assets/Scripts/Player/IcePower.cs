using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperMaxim.Messaging;

public class IcePower : Power
{
    public override void Use()
    {
        var message = new ElementChangeMessage(PowerType.Ice);
        Messenger.Default.Publish(message);
    }
}
