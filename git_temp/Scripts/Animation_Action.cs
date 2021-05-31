using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Platform.Models;
using UnityEngine;

public class Animation_Action : MonoBehaviour
{
    [SerializeField]  int ActionIndex = 0;
    [SerializeField] Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (ActionIndex == 0)
        {
            Debug.Log("Action Index is Invalid!");
        }
        if (other.tag == "LeftHand" || other.tag == "RightHand")
        {
            animator.SetInteger("action", ActionIndex);
        }
    }
}
