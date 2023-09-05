using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public EventReference audioref;
    public EventInstance audioinstance;
  
    // Start is called before the first frame update
    void Start()
    {
        audioinstance = RuntimeManager.CreateInstance(audioref);
    
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlayButton(string state)
    {
        audioinstance.start();
        audioinstance.release();
    }
}