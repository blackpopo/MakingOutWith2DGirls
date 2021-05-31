using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBoneCleaner : MonoBehaviour
{
    private DynamicBone db;

    [ContextMenu("Clean Dynamic Bone")]
    void RemoveDynamicBone()
    {
        var allChildren = GetAll(gameObject);
        foreach (var obj in  allChildren)
        {
            var db = obj.GetComponent<DynamicBone>();
            if (db)
            {
                DestroyImmediate(db);
            }
        }
    }

    private List<GameObject>  GetAll (GameObject gameObject)
    {
        List<GameObject> allChildren = new List<GameObject> ();
        allChildren.Add(gameObject);
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
