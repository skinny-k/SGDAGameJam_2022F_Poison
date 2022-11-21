using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] Transform _holdLocation = null;
    
    Item _heldItem = null;
    public bool HasItem
    {
        get => !(_heldItem == null);
    }
    
    public bool PickUpItem(Item itemToPickUp)
    {
        if (_heldItem == null)
        {
            _heldItem = itemToPickUp;
            _heldItem.PickUpBy(this);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PutDownItem(Table tableToPlaceOn)
    {
        if (tableToPlaceOn.ReceiveItem(_heldItem))
        {
            _heldItem.PutDown();
            _heldItem = null;
        }
    }

    void Update()
    {
        if (_heldItem != null)
        {
            _heldItem.transform.position = _holdLocation.position - new Vector3(0, 0, 0.1f);
        }
    }
}
