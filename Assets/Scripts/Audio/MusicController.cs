using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public EventReference musicref;
    public EventInstance musicinstance;
  
    // Start is called before the first frame update
    void Start()
    {
        musicinstance = RuntimeManager.CreateInstance(musicref);
        musicinstance.start();
        musicinstance.release();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //public void ParameterChange(string state)
    //{
       // RuntimeManager.StudioSystem.setParameterByNameWithLabel("MenuGameplay", state);
    //}
}