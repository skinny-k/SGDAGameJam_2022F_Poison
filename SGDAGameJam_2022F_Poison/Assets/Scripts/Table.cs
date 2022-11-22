using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField] Transform _holdLocation = null;
    [SerializeField] float _pickupDistance = 2.75f;
    [SerializeField] LayerMask _itemLayers = 0;
    [SerializeField] LayerMask _toolLayers = 0;
    
    PlayerPickup _playerIncoming;
    protected Item _itemHolding = null;
    bool _expectingDropoff = false;
    
    public bool Interact(Player player)
    {
        return ClickTable(player);
    }

    void OnEnable()
    {
        PlayerMovement.OnNewMovement += ResetExpectations;
    }

    void Start()
    {
        ContactFilter2D itemFilter = new ContactFilter2D();
        itemFilter.NoFilter();
        itemFilter.SetLayerMask(_itemLayers);
        List<Collider2D> itemColliders = new List<Collider2D>();

        GetComponent<Collider2D>().OverlapCollider(itemFilter, itemColliders);
        if (itemColliders.Count == 1)
        {
            ReceiveItem(itemColliders[0].GetComponent<Item>());
        }
        else
        {
            ContactFilter2D toolFilter = new ContactFilter2D();
            toolFilter.NoFilter();
            toolFilter.SetLayerMask(_toolLayers);
            List<Collider2D> toolColliders = new List<Collider2D>();

            GetComponent<Collider2D>().OverlapCollider(toolFilter, toolColliders);
            if (toolColliders.Count == 1)
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
                this.enabled = false;
            }
        }
    }
    
    bool ClickTable(Player player)
    {
        if (_itemHolding == null && _playerIncoming == null)
        {
            PlayerPickup checkPlayer = player.GetComponent<PlayerPickup>();
            
            if (checkPlayer.HasItem)
            {
                _expectingDropoff = true;
                _playerIncoming = checkPlayer;
                return true;
            }
            else
            {
                ResetExpectations();
                return false;
            }
        }
        else if (_itemHolding != null && _playerIncoming == null)
        {
            _playerIncoming = player.GetComponent<PlayerPickup>();
            return true;
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
    }

    public virtual bool ReceiveItem(Item itemToReceive)
    {
        ResetExpectations();
        if (_itemHolding == null)
        {
            _itemHolding = itemToReceive;
            _itemHolding.OnPickUp += ReleaseItem;
            _itemHolding.transform.position = _holdLocation.position - new Vector3(0, 0, 0.01f);
            _itemHolding.GetComponent<InteractableSprite>().SetPositionZ();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReleaseItem(PlayerPickup releaseToPlayer)
    {
        ResetExpectations();
        _itemHolding.OnPickUp -= ReleaseItem;
        _itemHolding = null;
    }

    void Update()
    {
        // Only checks dropoffs b/c pickups are handled thru events
        if (_expectingDropoff && _playerIncoming != null)
        {
            Vector3 posWithoutZ = transform.position;
            Vector3 playerIncomingPosWithoutZ = _playerIncoming.transform.position;
            posWithoutZ.z = 0;
            playerIncomingPosWithoutZ.z = 0;
        
            if (_expectingDropoff && Vector3.Distance(posWithoutZ, playerIncomingPosWithoutZ) <= _pickupDistance)
            {
                _playerIncoming.PutDownItem(this);
                ResetExpectations();
            }
        }
    }

    void OnDisable()
    {
        PlayerMovement.OnNewMovement -= ResetExpectations;
    }
}
