using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] float _pickupDistance = 2.75f;

    PlayerPickup _playerIncoming;
    PlayerPickup _playerHolding;
    ItemSprite _mySprite;
    bool _needsPickup = false;

    public event Action<PlayerPickup> OnPickUp;
    public event Action<PlayerPickup> OnPutDown;
    
    public bool Interact(Player player)
    {
        ClickItem(player);
        return true;
    }

    void OnEnable()
    {
        PlayerMovement.OnNewMovement += ResetExpectations;
    }

    void Awake()
    {
        _mySprite = GetComponent<ItemSprite>();
    }
    
    void ClickItem(Player player)
    {
        if (_playerIncoming == null)
        {
            _needsPickup = true;
            _playerIncoming = player.GetComponent<PlayerPickup>();
        }
        else
        {
            ResetExpectations();
        }
    }

    public void ResetExpectations()
    {
        _needsPickup = false;
        _playerIncoming = null;
    }

    void Update()
    {
        if (_needsPickup && _playerIncoming != null)
        {
            Vector3 posWithoutZ = transform.position;
            Vector3 playerIncomingPosWithoutZ = _playerIncoming.transform.position;
            posWithoutZ.z = 0;
            playerIncomingPosWithoutZ.z = 0;

            if (Vector3.Distance(posWithoutZ, playerIncomingPosWithoutZ) <= _pickupDistance)
            {
                _playerIncoming.PickUpItem(this);
                _needsPickup = false;
            }
        }
    }

    public void PickUpBy(PlayerPickup player)
    {
        ResetExpectations();
        _playerHolding = player;
        OnPickUp?.Invoke(_playerHolding);
        _mySprite.PickUpBy(player);
    }

    public void PutDown()
    {
        ResetExpectations();
        OnPutDown?.Invoke(_playerHolding);
        _playerHolding = null;
        _mySprite.PutDown();
    }

    void OnDisable()
    {
        PlayerMovement.OnNewMovement -= ResetExpectations;
    }
}
