using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item
{
    [SerializeField] int _ingredientType = 0;
    public int IngredientType
    {
        get => _ingredientType;
    }
    
    /*
    [SerializeField] string _ingredientName;
    public string IngredientName
    {
        get => _ingredientName;
    }
    */
}
