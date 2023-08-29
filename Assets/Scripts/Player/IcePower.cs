using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperMaxim.Messaging;
using FMODUnity;

public class IcePower : Power
{
    FMOD.Studio.EventInstance IcePowerSound;
    public override void Use()
    {
        var message = new ElementChangeMessage(PowerType.Ice);
        Messenger.Default.Publish(message);
        IcePowerSound = RuntimeManager.CreateInstance("event:/SFX/Player/Powers/UI_Power_Ice_Use_(FX_036)");
        IcePowerSound.start();
        IcePowerSound.release();
    }
}
