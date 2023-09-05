using FMODUnity;
using SuperMaxim.Messaging;
using UnityEngine;

public class EarthPower : Power
{
    [SerializeField] private GameObject earthBlock;
    [SerializeField] private Transform earthSpawn;

    //instancia fmod
    FMOD.Studio.EventInstance EarthPowerSound;
    
    public override void Use()
    {
        GameObject newBlock = Instantiate(earthBlock, earthSpawn.position, earthSpawn.rotation);

        //Crea Instancia de evento de FMOD y Reproduce el sonido de Poder de Tierra
        //EarthPowerSound = RuntimeManager.CreateInstance("event:/SFX/Player/Powers/UI_Power_Earth_Use_(FX_015)");
       // EarthPowerSound.start();
        //EarthPowerSound.release();
    }
}
