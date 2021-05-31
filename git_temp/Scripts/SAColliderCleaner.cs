using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAColliderCleaner : MonoBehaviour
{
    [ContextMenu("Clean All Children")]
    void CleanAllChildren()
    {
        var allChildren = GetAll(gameObject.gameObject);
        foreach (var obj in allChildren)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb)
            {
                DestroyImmediate(obj);
            }
        }
    }

    // Start is called before the first frame update
    private List<GameObject> GetAll(GameObject gameObject)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(gameObject, ref allChildren);
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
