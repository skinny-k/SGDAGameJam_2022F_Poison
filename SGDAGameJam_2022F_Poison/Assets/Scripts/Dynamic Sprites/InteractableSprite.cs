using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSprite : DynamicSprite
{
    [SerializeField] float _zOffset = 0.02f;
    
    PlayerPickup _playerHolding = null;
    
    public override void SetPositionZ()
    {
        if (_playerHolding == null)
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.y - (20 + _zOffset));
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
