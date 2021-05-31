using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] 
    public Transform Head = null;
    
    private float rotateSpeed = 1.0f;

    [SerializeField] 
    public GameObject targetObject;

    [SerializeField] public OVRHand leftHand;
    [SerializeField] public OVRHand rightHand;
    
    //MonoBehavor Reset() 関数はコンポーネントの初期化で呼ばれる
    void Reset()
    {
        Head = GetComponent<OVRCameraRig>().transform.Find("TrackingSpace/CenterEyeAnchor");
        
    }
    
    

    void Update()
    {
        // Forward move
        if (Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.RawButton.LThumbstickUp) || OVRInput.Get(OVRInput.RawButton.RThumbstickUp))
        {
            var forward = Head.forward;
            forward.y = 0;
            transform.position += forward.normalized * Time.deltaTime;
        }
        // Back move
        if (Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.RawButton.LThumbstickDown) || OVRInput.Get(OVRInput.RawButton.RThumbstickDown))
        {
            var forward = Head.forward;
            forward.y = 0;
            transform.position -= forward.normalized * Time.deltaTime;
        }
        // Left rotate
        if (Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.RawButton.LThumbstickLeft) || OVRInput.Get(OVRInput.RawButton.RThumbstickLeft))
        {
            transform.RotateAround(targetObject.transform.position, Vector3.up, rotateSpeed);
        }
        // Right rotate
        if (Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.RawButton.LThumbstickRight) || OVRInput.Get(OVRInput.RawButton.RThumbstickRight))
        {
            transform.RotateAround(targetObject.transform.position, Vector3.up, -rotateSpeed);
        }
        // Up move
        if (Input.GetKey(KeyCode.Q) || OVRInput.Get(OVRInput.RawButton.Y) || OVRInput.Get(OVRInput.RawButton.B))
        {
            transform.position += Vector3.up / 1000.0f;
        }
        // Down move
        if (Input.GetKey(KeyCode.E) || OVRInput.Get(OVRInput.RawButton.X) || OVRInput.Get(OVRInput.RawButton.A))
        {
            transform.position -= Vector3.up / 1000.0f;
        }

        if (leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
            rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            ;
        }
    }
}
