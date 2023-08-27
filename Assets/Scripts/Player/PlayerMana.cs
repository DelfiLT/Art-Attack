using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] protected int manaQuantity;
    [SerializeField] GameObject[] manaSpheres;

    public int ManaQuantity { get { return manaQuantity; } set { manaQuantity = value; } }

    private void Start()
    {
        manaQuantity = 3;
    }

    private void Update()
    {
        if(manaQuantity == 2)
        {
            manaSpheres[2].SetActive(false);
        }

        if (manaQuantity == 1)
        {
            manaSpheres[1].SetActive(false);
        }

        if (manaQuantity <= 0)
        {
            manaSpheres[0].SetActive(false);
        }
    }
}
