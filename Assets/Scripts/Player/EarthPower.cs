using FMODUnity;
using SuperMaxim.Messaging;
using UnityEngine;

public class EarthPower : Power
{
    [SerializeField] private GameObject earthBlock;
    [SerializeField] private Transform earthSpawn;
    FMOD.Studio.EventInstance EarthPowerSound;
    
    public override void Use()
    {
        GameObject newBlock = Instantiate(earthBlock, earthSpawn.position, earthSpawn.rotation);
        EarthPowerSound = RuntimeManager.CreateInstance("event:/SFX/Player/Powers/UI_Power_Earth_Use_(FX_015)");
        EarthPowerSound.start();
        EarthPowerSound.release();
    }
}
