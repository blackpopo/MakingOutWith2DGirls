using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAllTag : MonoBehaviour
{
    [SerializeField] String tagname;
    [SerializeField] GameObject gameobject;

    // Start is called before the first frame update
    [ContextMenu("Change Tag Name")]
    void ChangeName()
    {
        var allChildren = GetAll(gameObject.gameObject);
        foreach (GameObject obj in allChildren) {
            obj.tag = tagname;
        }
    }
    
    private List<GameObject>  GetAll (GameObject gameObject)
    {
        List<GameObject> allChildren = new List<GameObject> ();
        GetChildren (gameObject, ref allChildren);
        return allChildren;
    }

//子要素を取得してリストに追加
    private void GetChildren (GameObject obj, ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform> ();
        //子要素がいなければ終了
        if (children.childCount == 0) {
            return;
        }
        foreach (Transform ob in children) {
            allChildren.Add (ob.gameObject);
            GetChildren (ob.gameObject, ref allChildren);
        }
    }
}
