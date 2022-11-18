using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : DynamicSprite
{
    protected override void Start()
    {
        //
    }

    void Update()
    {
        SetPositionZ();
    }
}
