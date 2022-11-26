﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] RectTransform _pauseMenu = null;
    [SerializeField] TMP_Text _goldText = null;

    [Header("Reward Settings")]
    [SerializeField] int _rewardAmountPerIngredient = 5;

    [Header("Suspicion Settings")]
    [SerializeField] Slider _suspicionTracker = null;
    [SerializeField] float _suspicionDecayRate = 2f;
    [SerializeField] float _suspicionFillSpeed = 10f;
    [SerializeField] float _customerDeathSuspicionAmount = 10f;
    [SerializeField] float _customerTimeoutSuspicionAmount = 5f;

    [Header("Game End Settings")]
    [SerializeField] int _goldToWin = 100;
    [SerializeField] string _winSceneName = "Win Scene";
    [SerializeField] float _maxSuspicion = 100f;
    [SerializeField] string _loseSceneName = "Los Scene";

    float _suspicion = 0f;
    int _gold = 0;
    
    void OnEnable()
    {
        Customer.OnDeath += HandleCustomerDeath;
        Customer.OnTimeout += HandleCustomerTimeout;
        Customer.OnOrderComplete += HandleCompletedOrder;
    }
    
    void Start()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncreaseSuspicion(20);
        }

        if (_suspicionTracker.value != _suspicion)
        {
            _suspicionTracker.value = Mathf.Clamp(_suspicionTracker.value + _suspicionFillSpeed * Time.deltaTime, 0f, _suspicion);
        }

        _suspicion = Mathf.Clamp(_suspicion - (_suspicionDecayRate * Time.deltaTime), 0f, 100f);
    }

    void TogglePause()
    {
        if (_pauseMenu.gameObject.activeSelf)
        {
            _pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            _pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void HandleCustomerDeath()
    {
        IncreaseSuspicion(_customerDeathSuspicionAmount);
    }

    void HandleCustomerTimeout()
    {
        IncreaseSuspicion(_customerTimeoutSuspicionAmount);
    }

    void HandleCompletedOrder(Order orderCompleted, bool wasPoisoned)
    {
        int reward = orderCompleted.GetReward(_rewardAmountPerIngredient, wasPoisoned);
        GiveGold(reward);
    }

    void GiveGold(int amount)
    {
        _gold += amount;
        _goldText.text = _gold + "";
        if (_gold >= _goldToWin)
        {
            TransitionManager.Instance.TransitionToScene(_loseSceneName);
        }
    }

    void IncreaseSuspicion(float amount)
    {
        _suspicion += amount;
        if (_suspicion >= _maxSuspicion)
        {
            TransitionManager.Instance.TransitionToScene(_winSceneName);
        }
    }

    void OnDisable()
    {
        Customer.OnDeath -= HandleCustomerDeath;
        Customer.OnTimeout -= HandleCustomerTimeout;
        Customer.OnOrderComplete -= HandleCompletedOrder;
    }
}
