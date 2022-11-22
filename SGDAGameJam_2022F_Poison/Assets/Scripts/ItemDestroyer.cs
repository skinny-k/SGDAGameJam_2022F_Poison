using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroyer : Table
{
    public override bool ReceiveItem(Item itemToDestroy)
    {
        ResetExpectations();
        if (_itemHolding == null)
        {
            _itemHolding = itemToDestroy;
            Destroy(_itemHolding.gameObject);
            _itemHolding = null;
            return true;
        }
        else
        {
            return false;
        }
    }
}
