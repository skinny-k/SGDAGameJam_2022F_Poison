using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] Transform _holdLocation = null;
    
    Item _heldItem = null;
    public Item HeldItem
    {
        get => _heldItem;
    }
    public bool HasItem
    {
        get => !(_heldItem == null);
    }

    public static event Action<Item> OnItemPickup;
    public static event Action OnItemRelease;
    
    public bool PickUpItem(Item itemToPickUp)
    {
        if (_heldItem == null)
        {
            _heldItem = itemToPickUp;
            _heldItem.PickUpBy(this);
            OnItemPickup?.Invoke(_heldItem);
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
            ReleaseItem();
        }
    }

    public void ReleaseItem()
    {
        _heldItem = null;
        OnItemRelease?.Invoke();
    }

    public void DestroyItem()
    {
        Destroy(_heldItem.gameObject);
        ReleaseItem();
    }

    void Update()
    {
        if (_heldItem != null)
        {
            _heldItem.transform.position = _holdLocation.position - new Vector3(0, 0, 0.1f);
        }
    }
}
