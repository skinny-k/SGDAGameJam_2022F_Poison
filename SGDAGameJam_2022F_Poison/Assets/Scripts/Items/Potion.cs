using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    int[] _ingredients = new int[OrderGenerator.NumIngredientTypes];
    public int[] Ingredients
    {
        get => _ingredients;
    }
    int _totalIngredients = 0;
    
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < _ingredients.Length; i++)
        {
            _ingredients[i] = 0;
        }
    }
    
    public void AddIngredient(Ingredient ingredientToAdd)
    {
        _ingredients[ingredientToAdd.IngredientType]++;
        _totalIngredients++;

        Destroy(ingredientToAdd.gameObject);
    }

    void OnMouseEnter()
    {
        ShowContents();
    }

    void OnMouseExit()
    {
        HideContents();
    }

    void ShowContents()
    {
        /*
        Debug.Log("Ingredient List:");
        for (int i = 0; i < _ingredients.Length; i++)
        {
            Debug.Log("Ingredient " + i + " x" + _ingredients[i]);
        }
        */
    }

    void HideContents()
    {
        //
    }
}
