using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPower : Power
{
    [SerializeField] private GameObject earthBlock;
    [SerializeField] private Transform earthSpawn;

    public override void Use()
    {
        base.Use();
        GameObject newBlock = Instantiate(earthBlock, earthSpawn.position, earthSpawn.rotation);
    }
}
