using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ticket : MonoBehaviour
{
    [SerializeField] Color[] _ingredientColors = new Color[OrderGenerator.NumIngredientTypes];
    [SerializeField] Image _ingredientDisplayPrefab = null;
    [SerializeField] RectTransform _safeDisplayBox = null;
    [SerializeField] RectTransform _poisonDisplayBox = null;
    
    public void DisplayOrder(Order orderToDisplay)
    {
        ShowSolution(_safeDisplayBox, orderToDisplay.SafeSolution);
        ShowSolution(_poisonDisplayBox, orderToDisplay.PoisonSolution);
    }
    
    void ShowSolution(RectTransform display, int[] solutionToDisplay)
    {
        int currentIngredientTypeCount = 0;

        for (int i = 0; i < solutionToDisplay.Length; i++)
        {
            if (solutionToDisplay[i] != 0)
            {
                while (display.childCount <= currentIngredientTypeCount)
                {
                    RectTransform ingredientSprite = Instantiate(_ingredientDisplayPrefab, display.transform).GetComponent<RectTransform>();
                    int xOffset = 25;
                    if (currentIngredientTypeCount > 0 && currentIngredientTypeCount % 2 == 1)
                    {
                        xOffset += 75;
                    }
                    int yOffset = -40 * (currentIngredientTypeCount / 2);
                    ingredientSprite.anchoredPosition3D += new Vector3(xOffset, yOffset, -10);
                }
                GameObject ingredient = display.GetChild(currentIngredientTypeCount).gameObject;
                // ingredient.GetComponent<SpriteRenderer>().sprite = _ingredientSprites[i];
                ingredient.GetComponent<Image>().color = _ingredientColors[i];
                ingredient.transform.GetChild(0).GetComponent<TMP_Text>().text = "x" + solutionToDisplay[i];
                currentIngredientTypeCount++;
            }
        }
    }
}
