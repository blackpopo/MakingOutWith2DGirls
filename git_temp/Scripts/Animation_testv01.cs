using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 
public class Animation_testv01 : MonoBehaviour   
{
    private Animator animator;
    [SerializeField]
    public AudioSource audioSource;

    [SerializeField] 
    public AudioClip[] lineClips;

    [SerializeField] public AudioClip[] actionClips;
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

    void LinePlayback(int animIndex)
    {
        audioSource.PlayOneShot(lineClips[animIndex-1], 1.0f);
    }

    void ActionPlayBack(int actionIndex)
    {
        audioSource.PlayOneShot(actionClips[actionIndex-1], 1.0f);
    }
    
}
        