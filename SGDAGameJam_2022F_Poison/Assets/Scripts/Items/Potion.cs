using System;
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

    public static event Action<int[]> OnPotionContentsShow;
    public static event Action OnPotionContentsHide;
    
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

    public void ShowContents()
    {
        OnPotionContentsShow?.Invoke(_ingredients);
    }

    public void HideContents()
    {
        OnPotionContentsHide?.Invoke();
    }
}
