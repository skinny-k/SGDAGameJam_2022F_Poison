using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Clickable : MonoBehaviour
{
    public static event Action<GameObject> OnClick;

    protected virtual void OnMouseDown()
    {
        OnClick?.Invoke(this.gameObject);
    }
}
