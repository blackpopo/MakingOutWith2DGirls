using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderEnabler : MonoBehaviour
{
    protected Collider[] m_colliders = null;
    // Start is called before the first frame update
    void Awake ()
    {
        m_colliders = this.GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();
        foreach(var c in m_colliders)
        {
            c.isTrigger = true;
        }
    }
}
