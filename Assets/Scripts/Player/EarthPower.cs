using SuperMaxim.Messaging;
using UnityEngine;

public class EarthPower : Power
{
    [SerializeField] private GameObject earthBlock;
    [SerializeField] private Transform earthSpawn;
    
    public override void Use()
    {
        GameObject newBlock = Instantiate(earthBlock, earthSpawn.position, earthSpawn.rotation);
    }
}
