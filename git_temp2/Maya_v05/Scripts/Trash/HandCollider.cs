using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    [SerializeField] public Transform rootPosition;

    [SerializeField] public String tagname = "Maya";

    // private CapsuleCollider col;
    private SphereCollider col;

    private Collider[] neighbors;

    private Vector3 totalTransform;
    // Start is called before the first frame update
    void Start()
    {
        // col = GetComponent<CapsuleCollider>();
        col = GetComponent<SphereCollider>();
        neighbors = new Collider[16];
        
    }

    void OnTriggerStay(Collider other)
        {
            if (other.tag == tagname) // Mayaと衝突したときのみに処理
            {

                // var size = transform.TransformVector(col.radius, col.height, col.radius);
                // var radius = size.x;
                // var height = size.y;
                // var center = transform.TransformPoint(col.center);
                // var bottom = new Vector3(center.x, center.y - height / 2 + radius, center.z);
                // var top = new Vector3(center.x, center.y + height / 2 - radius, center.z);
                // int count = Physics.OverlapCapsuleNonAlloc(top, bottom, radius,
                //     neighbors); //アタッチしているコライダーに接触しているcolliderをすべて取り出す。

                int count = Physics.OverlapSphereNonAlloc(transform.position, col.radius, neighbors);
                float min_distance = 100.0f;
                Vector3 min_direction = Vector3.zero;
                for (int i = 0; i < count; ++i)
                {
                    var collider = neighbors[i];
    
                    //
                    if (collider == col || collider.tag != tagname) //自分自身やMaya以外のColliderなら無視
                    {
                        continue;
                    }
                    Debug.Log(collider.tag);
                    Vector3 otherPosition = other.gameObject.transform.position;
                    Quaternion otherRotation = other.gameObject.transform.rotation;
                    Vector3 direction;
                    float distance;
                    bool overlapped = Physics.ComputePenetration(
                        col, transform.position, transform.rotation,
                        collider, otherPosition, otherRotation,
                        out direction, out distance
                    );
                    if (overlapped)
                    {
                        if (min_distance > distance)
                        {
                            min_distance = distance;
                            // min_direction = direction;
                            min_direction = (-transform.position + other.transform.position).normalized;
                        }
                    }
                }
    
                if (min_distance < 10.0f)
                {
                    rootPosition.transform.Translate(min_direction * min_distance);
                    totalTransform += min_direction * min_distance;
                    Debug.Log("total transform: " + totalTransform);
                }
            }
        }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == tagname)
        {
            totalTransform = Vector3.zero;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == tagname) // Mayaと衝突したときのみに処理
        {
            rootPosition.transform.Translate(-totalTransform);
        }
    }
    
}
