using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public EventReference Jumpref;
    public EventInstance jumpinstance;
    [SerializeField] KeyCode keyjump = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        jumpinstance = RuntimeManager.CreateInstance(Jumpref);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(keyjump))
        {
            jumpinstance.start();
        }

    }
}