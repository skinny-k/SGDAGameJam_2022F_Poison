using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionDisplayTool : MonoBehaviour
{
    // [SerializeField] Sprite[] _ingredientSprites = new Sprite[OrderGenerator.NumIngredientTypes];
    [SerializeField] Color[] _ingredientColors = new Color[OrderGenerator.NumIngredientTypes];
    [SerializeField] Image _ingredientDisplayPrefab = null;
    [SerializeField] RectTransform _displayBox = null;
    
    void OnEnable()
    {
        Potion.OnPotionContentsShow += ShowContents;
        Potion.OnPotionContentsHide += HideContents;
    }

    void Update()
    {
        if (_displayBox.gameObject.activeSelf)
        {
            _displayBox.anchoredPosition = new Vector2(Input.mousePosition.x - (Screen.width / 2) + 75, Input.mousePosition.y - (Screen.height / 2) + 25);
        }
    }

    void ShowContents(int[] potionToDisplay)
    {
        int currentIngredientTypeCount = 0;

        for (int i = 0; i < potionToDisplay.Length; i++)
        {
            if (potionToDisplay[i] != 0)
            {
                while (_displayBox.childCount <= currentIngredientTypeCount + 1)
                {
                    RectTransform ingredientSprite = Instantiate(_ingredientDisplayPrefab, _displayBox.transform).GetComponent<RectTransform>();
                    ingredientSprite.anchoredPosition3D += new Vector3(25 + (100 * currentIngredientTypeCount), 0, -10);
                }
                GameObject ingredient = _displayBox.GetChild(currentIngredientTypeCount + 1).gameObject;
                // ingredient.GetComponent<SpriteRenderer>().sprite = _ingredientSprites[i];
                ingredient.GetComponent<Image>().color = _ingredientColors[i];
                ingredient.transform.GetChild(0).GetComponent<TMP_Text>().text = "x" + potionToDisplay[i];
                currentIngredientTypeCount++;
            }
        }

        _displayBox.gameObject.SetActive(true);
        _displayBox.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100 * currentIngredientTypeCount, 50);
        _displayBox.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(50 * currentIngredientTypeCount, 0, -20);
    }

    void HideContents()
    {
        _displayBox.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Potion.OnPotionContentsShow -= ShowContents;
        Potion.OnPotionContentsHide -= HideContents;
    }
}
