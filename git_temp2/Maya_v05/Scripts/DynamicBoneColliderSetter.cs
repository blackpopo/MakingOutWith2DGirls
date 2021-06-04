using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBoneColliderSetter : MonoBehaviour
{
    // [SerializeField] public GameObject baseObject;
    private CapsuleCollider fromCollider;
    private GameObject baseObject;
    [SerializeField] private GameObject hand;

    [ContextMenu("Copy Colliders in Chilren")]
    public void CopyColliderInChildren()
    {
        var allChildren = GetAll(hand);
            foreach (var obj in allChildren)
            {
                fromCollider = obj.GetComponent<CapsuleCollider>();
                if (fromCollider ){
                    obj.AddComponent<DynamicBoneCollider>();
                    DynamicBoneCollider toCollider = obj.GetComponent<DynamicBoneCollider>();
                    if (fromCollider.direction == 0) 
                    {
                        toCollider.m_Direction = DynamicBoneColliderBase.Direction.X;
                    }
                    else if (fromCollider.direction == 1)
                    {
                        toCollider.m_Direction = DynamicBoneColliderBase.Direction.Y;
                    }
                    else
                    {
                        toCollider.m_Direction = DynamicBoneColliderBase.Direction.Z;
                    }

                    toCollider.m_Center.x = fromCollider.center.x;
                    toCollider.m_Center.y = fromCollider.center.y;
                    toCollider.m_Center.z = fromCollider.center.z;

                    toCollider.m_Height = fromCollider.height;
                    toCollider.m_Radius = fromCollider.radius;
                }
            }
        
    }

    [ContextMenu("Remove Colliders in Children")]
    public void RemoveAllColliders()
    {
        var allChildren = GetAll(hand);
        foreach (var obj in allChildren)
        {
            var toCollider = obj.GetComponent<DynamicBoneCollider>();
            if (toCollider)
            {
                DestroyImmediate(toCollider);
            }

        }
    }

    [ContextMenu("Set Hand Colliders")]
    public void SetHandColliders()
    {
        var allHandColliders = new List<DynamicBoneCollider>();
        var allChildren = GetAll(hand);
        var db = GetComponent<DynamicBone>();
        foreach (var obj in allChildren)
        {
            DynamicBoneCollider dbc = obj.GetComponent<DynamicBoneCollider>();
            if (dbc)
            {
                allHandColliders.Add(dbc);
            }
        }
        foreach (var col in allHandColliders)
        {
            db.m_Colliders.Add(col);
        }
    }

    [ContextMenu("Remove Hand Colliders")]
    public void RemoveHandColliders()
    {
        var db = GetComponent<DynamicBone>();
        db.m_Colliders.Clear();
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
