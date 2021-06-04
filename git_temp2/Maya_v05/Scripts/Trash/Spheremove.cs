using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spheremove : MonoBehaviour
{
    [SerializeField] 
    public Transform Head = null;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // Forward move
        if (Input.GetKey(KeyCode.I) || OVRInput.Get(OVRInput.RawButton.LThumbstickUp) || OVRInput.Get(OVRInput.RawButton.RThumbstickUp))
        {
            var forward = Head.forward;
            forward.y = 0;
            transform.position += forward.normalized * Time.deltaTime;
        }
        // Back move
        if (Input.GetKey(KeyCode.K) || OVRInput.Get(OVRInput.RawButton.LThumbstickDown) || OVRInput.Get(OVRInput.RawButton.RThumbstickDown))
        {
            var forward = Head.forward;
            forward.y = 0;
            transform.position -= forward.normalized * Time.deltaTime;
        }
        // Up move
        if (Input.GetKey(KeyCode.L) || OVRInput.Get(OVRInput.Button.Four) || OVRInput.Get(OVRInput.Button.Two))
        {
            transform.position += Vector3.left * Time.deltaTime;
        }
        // Down move
        if (Input.GetKey(KeyCode.J) || OVRInput.Get(OVRInput.Button.Three) || OVRInput.Get(OVRInput.Button.One))
        {
            transform.position += Vector3.right * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.U) || OVRInput.Get(OVRInput.Button.Four) || OVRInput.Get(OVRInput.Button.Two))
        {
            transform.position += Vector3.up / 1000.0f;
        }
        // Down move
        if (Input.GetKey(KeyCode.O) || OVRInput.Get(OVRInput.Button.Three) || OVRInput.Get(OVRInput.Button.One))
        {
            transform.position -= Vector3.up / 1000.0f;
        }
    }
}
