using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
using Oculus.Interaction;
public class GunController : MonoBehaviour
{
    public SimpleShoot simpleShoot;
    public OVRInput.Button shootButton;
    public OVRInput.Controller shootingController;

    private Grabbable _grabbable;
    private AudioSource _fireSound;

    void Start()
    {
        _grabbable = GetComponent<Grabbable>();
        _fireSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(shootButton)) {
            // print which hand's button is pressed
            print("Button pressed: " + OVRInput.GetActiveController());
        }
        if (OVRInput.GetDown(shootButton, shootingController) && _grabbable != null && _grabbable.IsGrabbed()) {
            simpleShoot.StartShoot();
            _fireSound.Play();
        }
    }
}
