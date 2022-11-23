using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour, IInteractable
{
    [SerializeField] float _pickupDistance = 5.5f;
    [SerializeField] float _despawnDelay = 1.5f;
    [SerializeField] float _timeout = 25f;
    
    protected CustomerSprite _mySprite = null;
    protected Order _myOrder = null;
    PlayerPickup _playerIncoming;
    Potion _potionIncoming;
    bool _expectingDropoff = false;
    // bool _dead = false;
    bool _interactable = true;
    float _timer = 0f;

    public static event Action<Order> OnOrderComplete;
    public static event Action OnDeath;
    public static event Action OnTimeout;
    
    public bool Interact(Player player)
    {
        return ClickCustomer(player);
    }

    bool ClickCustomer(Player player)
    {
        if (_playerIncoming == null)
        {
            PlayerPickup checkPlayer = player.GetComponent<PlayerPickup>();
            
            if (checkPlayer.HasItem && checkPlayer.HeldItem.GetComponent<Potion>() != null)
            {
                _expectingDropoff = true;
                _playerIncoming = checkPlayer;
                _potionIncoming = checkPlayer.HeldItem.GetComponent<Potion>();
                return true;
            }
            else
            {
                ResetExpectations();
                return false;
            }
        }
        else
        {
            ResetExpectations();
            return false;
        }
    }

    public void ResetExpectations()
    {
        _expectingDropoff = false;
        _playerIncoming = null;
        _potionIncoming = null;
    }

    void OnEnable()
    {
        PlayerMovement.OnNewMovement += ResetExpectations;
        PlayerPickup.OnItemPickup += TurnOnHighlight;
        PlayerPickup.OnItemRelease += TurnOffHighlight;
    }
    
    void Awake()
    {
        _myOrder = OrderGenerator.GenerateOrder();
        _mySprite = GetComponent<CustomerSprite>();
    }

    void Update()
    {
        if (_interactable && _expectingDropoff && _playerIncoming != null)
        {
            Vector3 posWithoutZ = transform.position;
            Vector3 playerIncomingPosWithoutZ = _playerIncoming.transform.position;
            posWithoutZ.z = 0;
            playerIncomingPosWithoutZ.z = 0;
        
            if (_expectingDropoff && Vector3.Distance(posWithoutZ, playerIncomingPosWithoutZ) <= _pickupDistance)
            {
                switch (_myOrder.ComparePotion(_potionIncoming))
                {
                    case -1:
                        _playerIncoming.DestroyItem();
                        Debug.Log("It's poison!");
                        OnOrderComplete?.Invoke(_myOrder);
                        StartCoroutine(Die());
                        break;
                    case 0:
                        // Potion does not match any solutions
                        Debug.Log("That's the wrong order!");
                        break;
                    case 1:
                        _playerIncoming.DestroyItem();
                        Debug.Log("Tastes good!");
                        OnOrderComplete?.Invoke(_myOrder);
                        StartCoroutine(Leave());
                        break;
                }
                ResetExpectations();
            }
        }

        _timer += Time.deltaTime;
        if (_interactable && _timer >= _timeout)
        {
            OnTimeout?.Invoke();
            Debug.Log("The service here is terrible!");
            StartCoroutine(Leave());
        }
    }

    void TurnOnHighlight(Item itemToCheck)
    {
        Potion potionToCheck = itemToCheck.GetComponent<Potion>();
        if (potionToCheck != null)
        {
            switch (_myOrder.ComparePotion(potionToCheck))
            {
                case -1:
                    Debug.Log("That's my poison! Turning on skull icon.");
                    break;
                case 0:
                    break;
                case 1:
                    Debug.Log("That's my potion! Turning on smile icon.");
                    break;
            }
        }
    }

    void TurnOffHighlight()
    {
        // Debug.Log("Turning off highlight.");
    }

    IEnumerator Die()
    {
        _mySprite.Die();
        // _dead = true;
        _interactable = false;
        OnDeath?.Invoke();

        yield return new WaitForSeconds(_despawnDelay);
        
        _mySprite.FadeOut();
    }

    IEnumerator Leave()
    {
        _interactable = false;
        
        yield return new WaitForSeconds(_despawnDelay);

        _mySprite.FadeOut();
    }

    void OnDisable()
    {
        PlayerMovement.OnNewMovement -= ResetExpectations;
        PlayerPickup.OnItemPickup -= TurnOnHighlight;
        PlayerPickup.OnItemRelease -= TurnOffHighlight;
    }
}
