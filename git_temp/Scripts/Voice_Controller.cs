using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice_Controller : MonoBehaviour
{
    [SerializeField]
    public AudioSource audioSource;

    [SerializeField] 
    public AudioClip[] lineClips;
    
    [SerializeField] 
    public AudioClip[] actionClips;

    // Update is called once per frame
    void LinePlayback(int animIndex)
    {
        audioSource.PlayOneShot(lineClips[animIndex-1], 1.0f);
    }

    void ActionPlayBack(int actionIndex)
    {
        audioSource.PlayOneShot(actionClips[actionIndex-1], 1.0f);
    }
}
