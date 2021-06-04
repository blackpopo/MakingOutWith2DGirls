using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColliderBuilder : MonoBehaviour
{
    [SerializeField]  Transform rootPosition;
    
    [SerializeField] private String tagname = "Maya";

    [ContextMenu("Copy Self")]
    public void CopySelf()
    {
        var allChildren = GetAll(gameObject.gameObject);
        foreach (GameObject obj in allChildren)
        {
            if (obj.GetComponent<Rigidbody>())
            {
                obj.AddComponent<HandCollider>();
                HandCollider handCollider = obj.GetComponent<HandCollider>();
                handCollider.rootPosition = rootPosition;
                handCollider.tagname = tagname;
            }
        }
    }
    
    [ContextMenu("Remove Self")]
    public void RemoveSelf()
    {
        var allChildren = GetAll(gameObject.gameObject);
        foreach (GameObject obj in allChildren)
        {
            if (obj.GetComponent<HandCollider>())
            {
                DestroyImmediate(obj.GetComponent<HandCollider>());
            }
        }
    }
    // Start is called before the first frame update

    private List<GameObject>  GetAll (GameObject gameObject)
    {
        List<GameObject> allChildren = new List<GameObject> ();
        GetChildren (gameObject, ref allChildren);
        return allChildren;
    }

    //子要素を取得してリストに追加
    private void GetChildren(GameObject obj, ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がいなければ終了
        if (children.childCount == 0)
        {
            return;
        }

        foreach (Transform ob in children)
        {
            allChildren.Add(ob.gameObject);
            GetChildren(ob.gameObject, ref allChildren);
        }
    }
}
