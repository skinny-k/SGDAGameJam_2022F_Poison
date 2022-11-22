using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    int totalIngredients = 0;
    int ingredientCount0 = 0;
    int ingredientCount1 = 0;
    int ingredientCount2 = 0;
    int ingredientCount3 = 0;
    
    public void AddIngredient(Ingredient ingredientToAdd)
    {
        switch (ingredientToAdd.IngredientType)
        {
            case 0:
                ingredientCount0++;
                totalIngredients++;
                break;
            case 1:
                ingredientCount1++;
                totalIngredients++;
                break;
            case 2:
                ingredientCount2++;
                totalIngredients++;
                break;
            case 3:
                ingredientCount3++;
                totalIngredients++;
                break;
        }

        Destroy(ingredientToAdd.gameObject);
    }
}
