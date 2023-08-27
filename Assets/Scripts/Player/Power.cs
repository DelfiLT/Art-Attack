using SuperMaxim.Messaging;
using System.Collections;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    //[SerializeField] private PowerType power;
    private PlayerMana playerMana;

    private void Start()
    {
        playerMana = GetComponent<PlayerMana>();
    }

    public virtual void Use() 
    {
        playerMana.ManaQuantity--;
    }

    protected void Update()
    {
        if (Input.GetKeyDown(key) && playerMana.ManaQuantity > 0)
        {
            Use();
        }
    }
}
