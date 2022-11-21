using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSprite : DynamicSprite
{
    PlayerPickup _playerHolding = null;
    
    protected override void SetPositionZ()
    {
        if (_playerHolding == null)
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.y - 20.01f);
        }
    }

    public void PickUpBy(PlayerPickup player)
    {
        _playerHolding = player;
    }

    public void PutDown()
    {
        _playerHolding = null;
    }
}
