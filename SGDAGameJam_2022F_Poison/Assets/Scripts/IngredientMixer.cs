using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientMixer : MonoBehaviour, IInteractable
{
    [SerializeField] Potion _basicPotionPrefab = null;
    [SerializeField] Transform _dropLocation = null;
    [SerializeField] SpriteRenderer _fill = null;
    [SerializeField] Color _fillPendingColor = Color.green;
    [SerializeField] Color _fillFinishedColor = Color.yellow;
    [SerializeField] float _pickupDistance = 2.75f;
    [SerializeField] float _brewTimeForOneIngredient = 3.5f;

    List<Ingredient> _ingredientsToBrew = new List<Ingredient>();
    Potion _myPotion = null;
    float _timer = 0;
    PlayerPickup _playerIncoming;
    bool _expectingDropoff;
    bool _expectingPickup;

    public event Action OnReady;
    public event Action OnCollect;
    
    public bool Interact(Player player)
    {
        return ClickMixer(player);
    }

    bool ClickMixer(Player player)
    {
        PlayerPickup checkPlayer = player.GetComponent<PlayerPickup>();
        
        if (checkPlayer != null && checkPlayer.HeldItem != null && 
            checkPlayer.HeldItem.GetComponent<Ingredient>() != null)
        {
            _expectingDropoff = true;
            _playerIncoming = checkPlayer;
            return true;
        }
        else if (checkPlayer != null && checkPlayer.HeldItem == null)
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

    void OnEnable()
    {
        PlayerMovement.OnNewMovement += ResetExpectations;
    }

    void CreateNewPotion()
    {
        _myPotion = Instantiate(_basicPotionPrefab, _dropLocation.position, Quaternion.identity);
        _myPotion.OnPickUp += Collect;
        _myPotion.gameObject.SetActive(false);
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
                Ingredient ingredientToAdd = _playerIncoming.HeldItem.GetComponent<Ingredient>();
                _ingredientsToBrew.Add(ingredientToAdd);
                _playerIncoming.ReleaseItem();
                ingredientToAdd.transform.position = transform.position;
                ingredientToAdd.gameObject.SetActive(false);
                UpdateFill();
                ResetExpectations();
            }
        }
        else if (_expectingPickup && _playerIncoming != null && _myPotion != null && _ingredientsToBrew.Count == 0)
        {
            Vector3 posWithoutZ = transform.position;
            Vector3 playerIncomingPosWithoutZ = _playerIncoming.transform.position;
            posWithoutZ.z = 0;
            playerIncomingPosWithoutZ.z = 0;

            if (Vector3.Distance(posWithoutZ, playerIncomingPosWithoutZ) <= _pickupDistance)
            {
                GivePotionToPlayer(_playerIncoming);
                ResetExpectations();
            }
        }

        if (_ingredientsToBrew.Count > 0)
        {
            _timer += Time.deltaTime;
        }
        if (_timer >= _brewTimeForOneIngredient)
        {
            if (_myPotion == null)
            {
                CreateNewPotion();
            }
            _myPotion.gameObject.SetActive(true);
            _ingredientsToBrew[0].gameObject.SetActive(true);
            _myPotion.AddIngredient(_ingredientsToBrew[0]);
            _myPotion.gameObject.SetActive(false);
            _ingredientsToBrew.Remove(_ingredientsToBrew[0]);
            _ingredientsToBrew.TrimExcess();
            _timer = 0;

            UpdateFill();

            if (_ingredientsToBrew.Count == 0)
            {
                OnReady?.Invoke();
            }
        }
    }

    void GivePotionToPlayer(PlayerPickup playerToGiveTo)
    {
        _myPotion.gameObject.SetActive(true);
        if (playerToGiveTo.PickUpItem(_myPotion))
        {
            UpdateFill();
        }
        else
        {
            _myPotion.gameObject.SetActive(false);
        }
    }

    void UpdateFill()
    {
        if (_ingredientsToBrew.Count == 0 && _myPotion != null)
        {
            _fill.gameObject.SetActive(true);
            _fill.color = _fillFinishedColor;
        }
        else if (_ingredientsToBrew.Count > 0 || _myPotion != null)
        {
            _fill.gameObject.SetActive(true);
            _fill.color = _fillPendingColor;
        }
        else
        {
            _fill.gameObject.SetActive(false);
        }
    }

    void ResetExpectations()
    {
        _expectingDropoff = false;
        _expectingPickup = false;
        _playerIncoming = null;
    }
    
    void Collect(PlayerPickup playerCollecting)
    {
        OnCollect?.Invoke();
        _myPotion.OnPickUp -= Collect;
        _myPotion.HideContents();
        _myPotion = null;
    }

    void OnMouseEnter()
    {
        if (_myPotion != null && _myPotion.Ingredients.Length > 0)
        {
            _myPotion.ShowContents();
        }
    }

    void OnMouseExit()
    {
        if (_myPotion != null)
        {
            _myPotion.HideContents();
        }
    }

    void OnDisable()
    {
        PlayerMovement.OnNewMovement -= ResetExpectations;
    }
}
