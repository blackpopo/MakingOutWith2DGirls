using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Contoller : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.RawButton.LIndexTrigger) ||
            OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            if (animator.GetInteger("line") == 0 && animator.GetInteger("action") == 0)
            {
                int num = Random.Range(1, 4);
                animator.SetInteger("line", num);
            }
        }
    }
}
