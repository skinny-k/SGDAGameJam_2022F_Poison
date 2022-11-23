using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable
{
    [SerializeField] Ingredient _dropItem = null;
    [SerializeField] Transform _dropLocation = null;
    [SerializeField] float _respawnRate = 10f;
    [SerializeField] float _pickupDistance = 2.75f;

    Item _myItem = null;
    float _timer = 0;
    PlayerPickup _playerIncoming;
    bool _expectingDropoff;

    public event Action OnReady;
    public event Action OnPick;

    public bool Interact(Player player)
    {
        return ClickPlant(player);
    }

    bool ClickPlant(Player player)
    {
        if (player.GetComponent<PlayerPickup>() != null && player.GetComponent<PlayerPickup>().HeldItem != null && player.GetComponent<PlayerPickup>().HeldItem.GetComponent<Fertilizer>() != null)
        {
            _expectingDropoff = true;
            _playerIncoming = player.GetComponent<PlayerPickup>();
            return true;
        }
        else
        {
            ResetExpectations();
            return false;
        }
    }

    void OnEnable()
    {
        PlayerMovement.OnNewMovement += ResetExpectations;
    }
    
    void Update()
    {
        if (_expectingDropoff && _playerIncoming != null)
        {
            Vector3 posWithoutZ = transform.position;
            Vector3 playerIncomingPosWithoutZ = _playerIncoming.transform.position;
            posWithoutZ.z = 0;
            playerIncomingPosWithoutZ.z = 0;

            if (Vector3.Distance(posWithoutZ, playerIncomingPosWithoutZ) <= _pickupDistance)
            {
                _playerIncoming.DestroyItem();
                Ready();
                ResetExpectations();
            }
        }
        
        if (_myItem == null)
        {
            _timer += Time.deltaTime;
        }

        if (_timer >= _respawnRate)
        {
            Ready();
        }
    }

    void ResetExpectations()
    {
        _expectingDropoff = false;
        _playerIncoming = null;
    }
    
    void Ready()
    {
        OnReady?.Invoke();
        _myItem = Instantiate(_dropItem, _dropLocation.position, Quaternion.identity);
        _myItem.OnPickUp += Pick;
        _timer = 0;
    }
    
    void Pick(PlayerPickup playerPicking)
    {
        OnPick?.Invoke();
        _myItem.OnPickUp -= Pick;
        _myItem = null;
    }

    void OnDisable()
    {
        PlayerMovement.OnNewMovement -= ResetExpectations;
    }
}
