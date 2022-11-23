using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OrderGenerator
{
    static int _numIngredientTypes = 4;
    public static int NumIngredientTypes
    {
        get => _numIngredientTypes;
    }
    static int _maxIngredients = 4;
    public static int MaxIngredients
    {
        get => _maxIngredients;
    }
    static int _maxRepetition = 3;
    public static int MaxRepetition
    {
        get => _maxRepetition;
    }
    
    public static Order GenerateOrder()
    {
        int[] safeSolution = GenerateSolution();
        int[] poisonSolution = GenerateSolution(safeSolution);

        return new Order(safeSolution, poisonSolution);
    }

    static int[] GenerateSolution()
    {
        int totalIngredients = 0;

        int[] solutionIngredients = new int[_numIngredientTypes];
        for (int i = 0; i < solutionIngredients.Length; i++)
        {
            bool hasIngredient = Random.Range(0, 2) == 1;

            if (hasIngredient)
            {
                int upperBound = Mathf.Min((_maxIngredients - totalIngredients), _maxRepetition) + 1;
                if (upperBound > 1)
                {
                    solutionIngredients[i] = Random.Range(1, upperBound);
                }
                else
                {
                    solutionIngredients[i] = 0;
                }
            }
            else
            {
                solutionIngredients[i] = 0;
            }

            totalIngredients += solutionIngredients[i];
        }

        // ensure an empty solution is not generated
        bool solutionIsEmpty = true;
        for (int i = 0; i < solutionIngredients.Length; i++)
        {
            if (solutionIngredients[i] != 0)
            {
                solutionIsEmpty = false;
                break;
            }
        }
        if (solutionIsEmpty)
        {
            int ingredientType = Random.Range(0, solutionIngredients.Length);
            solutionIngredients[ingredientType] = Random.Range(0, _maxRepetition + 1);
            totalIngredients += solutionIngredients[ingredientType];
        }

        return solutionIngredients;
    }

    static int GetNumberOfIngredients(int[] solution)
    {
        int result = 0;
        
        foreach (int ingredientCount in solution)
        {
            result += ingredientCount;
        }

        return result;
    }

    static int[] GenerateSolution(int[] solutionToExclude)
    {
        int[] solutionIngredients = GenerateSolution();

        for (int i = 0; i < solutionIngredients.Length; i++)
        {
            // if the two solutions do not match, then the generated solution is valid
            if (solutionIngredients[i] != solutionToExclude[i])
            {
                return solutionIngredients;
            }
        }

        // Only gets executed if the two solutions are the same
        int choice = 0;
        if (GetNumberOfIngredients(solutionIngredients) == _maxIngredients)
        {
            choice = Random.Range(0, 2);
        }
        else if (GetNumberOfIngredients(solutionIngredients) < _maxIngredients)
        {
            choice = Random.Range(0, 3);
        }
        // 0 - remove an ingredient
        // 1 - swap an ingredient
        // 2 - add an ingredient

        // We need to know which ones can be increased and which can be decreased, so create two lists to store each
        List<int> ingredientsToIncrease = new List<int>();
        List<int> ingredientsToDecrease = new List<int>();
        for (int i = 0; i < solutionIngredients.Length; i++)
        {
            // If an ingredient is greater than 0, it can be decreased
            if (solutionIngredients[i] > 0)
            {
                ingredientsToDecrease.Add(i);
            }

            // If an ingredient is less than the max number of repetitions, it can be increased
            if (solutionIngredients[i] < _maxRepetition)
            {
                ingredientsToIncrease.Add(i);
            }
        }

        // Carry out the choice selected earlier
        switch (choice)
        {
            case 0:
                // Remove an ingredient
                solutionIngredients[ingredientsToDecrease[Random.Range(0, ingredientsToDecrease.Count)]]--;
                break;
            case 1:
                // Swap two ingredients
                // Select an ingredient to decrease
                int ingredientDecreased = ingredientsToDecrease[Random.Range(0, ingredientsToDecrease.Count)];
                solutionIngredients[ingredientDecreased]--;
                // If the decreased ingredient is also eligible for increase, remove it. Increasing the same ingredient will result in the original solution, which is invalid
                if (ingredientsToIncrease.Remove(ingredientDecreased))
                {
                    ingredientsToIncrease.TrimExcess();
                }
                // Select an ingredient to increase from the remaining ingredients
                solutionIngredients[ingredientsToIncrease[Random.Range(0, ingredientsToIncrease.Count)]]++;
                break;
            case 2:
                // Add an ingredient
                solutionIngredients[ingredientsToIncrease[Random.Range(0, ingredientsToIncrease.Count)]]++;
                break;
        }

        return solutionIngredients;
    }
}
