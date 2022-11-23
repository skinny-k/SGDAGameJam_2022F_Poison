using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item
{
    [SerializeField] float _startingPickupDistance = 5.5f;
    [SerializeField] int _ingredientType = 0;
    public int IngredientType
    {
        get => _ingredientType;
    }

    float _basePickupDistance;
    
    protected override void Awake()
    {
        base.Awake();

        _basePickupDistance = _pickupDistance;
        _pickupDistance = _startingPickupDistance;
    }

    public override void PickUpBy(PlayerPickup player)
    {
        base.PickUpBy(player);
        if (_pickupDistance == _startingPickupDistance)
        {
            _pickupDistance = _basePickupDistance;
        }
    }
}
