using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSprite : MonoBehaviour
{
    protected virtual void Start()
    {
        SetPositionZ();
    }

    public virtual void SetPositionZ()
    {
        transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.y - 20);
    }
}
