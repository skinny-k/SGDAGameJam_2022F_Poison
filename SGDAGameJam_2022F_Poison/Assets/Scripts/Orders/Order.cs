﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    int[] _mySafeSolution = new int[OrderGenerator.NumIngredientTypes];
    public int[] SafeSolution
    {
        get => _mySafeSolution;
    }
    int[] _myPoisonSolution = new int[OrderGenerator.NumIngredientTypes];
    public int[] PoisonSolution
    {
        get => _myPoisonSolution;
    }
    
    public Order(int[] safeSolution, int[] poisonSolution)
    {
        Array.Copy(safeSolution, _mySafeSolution, safeSolution.Length);
        Array.Copy(poisonSolution, _myPoisonSolution, safeSolution.Length);

        Debug.Log(ToString());
    }

    public int ComparePotion(Potion potion)
    {
        bool matchesSafeSolution = true;
        bool matchesPoisonSolution = true;
        
        for (int i = 0; i < _mySafeSolution.Length; i++)
        {
            if (_mySafeSolution[i] != potion.Ingredients[i])
            {
                matchesSafeSolution = false;
            }
            if (_myPoisonSolution[i] != potion.Ingredients[i])
            {
                matchesPoisonSolution = false;
            }

            if (!(matchesSafeSolution || matchesPoisonSolution))
            {
                return 0;
            }
        }

        if (matchesSafeSolution)
        {
            return 1;
        }
        else if (matchesPoisonSolution)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public int GetReward(int valuePerIngredient, bool isPoison)
    {
        int[] _orderFilled;
        int reward = 0;

        if (isPoison)
        {
            _orderFilled = _myPoisonSolution;
        }
        else
        {
            _orderFilled = _mySafeSolution;
        }

        foreach (int ingredientCount in _orderFilled)
        {
            reward += ingredientCount * valuePerIngredient;
        }

        return reward;
    }

    public override string ToString()
    {
        string safeSolutionString = "(";
        for (int i = 0; i < _mySafeSolution.Length; i++)
        {
            safeSolutionString += _mySafeSolution[i] + "";
            if (i < _mySafeSolution.Length - 1)
            {
                safeSolutionString += ", ";
            }
        }
        safeSolutionString += ")";

        string poisonSolutionString = "(";
        for (int i = 0; i < _myPoisonSolution.Length; i++)
        {
            poisonSolutionString += _myPoisonSolution[i] + "";
            if (i < _mySafeSolution.Length - 1)
            {
                poisonSolutionString += ", ";
            }
        }
        poisonSolutionString += ")";
        
        return "Safe Solution:   " + safeSolutionString + "\n" + "Poison Solution: " + poisonSolutionString;
    }
}
