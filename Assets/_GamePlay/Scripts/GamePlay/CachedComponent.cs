using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachedComponent<T> where T : Component
{
    RaycastHit hit_;
    T component;

    public RaycastHit hit
    {
        get { return hit_; }
        set { hit_ = value; component = null; }
    }

    public CachedComponent(RaycastHit gameObject)
    {
        hit_ = gameObject;
    }

    public T get()
    {
        if (component == null)
        {
            if (hit.collider != null)
            {
                component = hit.collider.GetComponent<T>();
            }
        }
        return component;
    }
}
