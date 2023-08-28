using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Runtime.CompilerServices;
using SuperMaxim.Messaging;

public class AudioPowerController : MonoBehaviour
{
    public EventReference PowerSfx;
    public EventInstance powerinstance;
    [SerializeField] KeyCode keypowerstart = KeyCode.LeftShift;

    // Start is called before the first frame update
    void Start()
    {
        powerinstance = RuntimeManager.CreateInstance(PowerSfx);
   
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            RuntimeManager.StudioSystem.setParameterByName("Power", 0);

        }

        if (Input.GetKeyDown("2"))
        {
            RuntimeManager.StudioSystem.setParameterByName("Power", 1);
           

        }

        if (Input.GetKeyDown(keypowerstart)) 
        {
            powerinstance.start();
        }
       
          
            
          
        
    }





}
    //public void ParameterChange(string state)
    //{
       // RuntimeManager.StudioSystem.setParameterByNameWithLabel("MenuGameplay", state);
    //}
