
using FMODUnity;

class FirePower : Power
{
    //instancia fmod
    FMOD.Studio.EventInstance FirePowerSound;
    public override void Use() 
    {
        //Crea Instancia de evento de FMOD y Reproduce el sonido de Poder de Fuego
        FirePowerSound = RuntimeManager.CreateInstance("event:/SFX/Player/Powers/UI_Power_Fire_Use");
        FirePowerSound.start();
        FirePowerSound.release();

    }
}
