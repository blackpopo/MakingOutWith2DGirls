using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbleBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    [ContextMenu("Copy Self")]
    public void CopySelf()
    {
        var allChildren = GetAll(gameObject.gameObject);
        foreach (GameObject obj in allChildren)
        {
            if (obj.GetComponent<Rigidbody>())
            {
                obj.AddComponent<OVRGrabbable>();
            }
        }
    }
    
    [ContextMenu("Remove Self")]
    public void RemoveSelf()
    {
        var allChildren = GetAll(gameObject.gameObject);
        foreach (GameObject obj in allChildren)
        {
            if (obj.GetComponent<OVRGrabbable>())
            {
                DestroyImmediate(obj.GetComponent<OVRGrabbable>());
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
