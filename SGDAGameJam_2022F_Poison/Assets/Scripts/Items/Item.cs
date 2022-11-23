using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] protected float _pickupDistance = 2.75f;

    protected PlayerPickup _playerIncoming;
    protected PlayerPickup _playerHolding;
    protected InteractableSprite _mySprite;
    protected bool _expectingPickup = false;

    public event Action<PlayerPickup> OnPickUp;
    public event Action<PlayerPickup> OnPutDown;
    
    public virtual bool Interact(Player player)
    {
        return ClickItem(player);
    }

    protected virtual void OnEnable()
    {
        PlayerMovement.OnNewMovement += ResetExpectations;
    }

    protected virtual void Awake()
    {
        _mySprite = GetComponent<InteractableSprite>();
    }
    
    protected virtual bool ClickItem(Player player)
    {
        if (_playerIncoming == null)
        {
            _expectingPickup = true;
            _playerIncoming = player.GetComponent<PlayerPickup>();
            return true;
        }
        else
        {
            ResetExpectations();
            return false;
        }
    }

    public virtual void ResetExpectations()
    {
        _expectingPickup = false;
        _playerIncoming = null;
    }

    protected virtual void Update()
    {
        if (_expectingPickup && _playerIncoming != null)
        {
            Vector3 posWithoutZ = transform.position;
            Vector3 playerIncomingPosWithoutZ = _playerIncoming.transform.position;
            posWithoutZ.z = 0;
            playerIncomingPosWithoutZ.z = 0;

            if (Vector3.Distance(posWithoutZ, playerIncomingPosWithoutZ) <= _pickupDistance)
            {
                _playerIncoming.PickUpItem(this);
                _expectingPickup = false;
            }
        }
    }

    public virtual void PickUpBy(PlayerPickup player)
    {
        ResetExpectations();
        _playerHolding = player;
        OnPickUp?.Invoke(_playerHolding);
        _mySprite.PickUpBy(player);
    }

    public virtual void PutDown()
    {
        ResetExpectations();
        OnPutDown?.Invoke(_playerHolding);
        _playerHolding = null;
        _mySprite.PutDown();
    }

    protected virtual void OnDisable()
    {
        PlayerMovement.OnNewMovement -= ResetExpectations;
    }
}
