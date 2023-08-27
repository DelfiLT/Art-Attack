class PowerEnableMessage
{
    //public float CooldownTime { get; private set; }
    public PowerType Power { get; private set; }

    public bool Enable { get; private set; }

    public PowerEnableMessage(/*float time, */PowerType power, bool enable)
    {
        //CooldownTime = time;
        Power = power;
        Enable = enable;
    }
}
