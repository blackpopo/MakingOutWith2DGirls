using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField] 
    private GameObject gameobject;

    private Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        _transform = gameobject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = transform.position.ToString();
    }
}
