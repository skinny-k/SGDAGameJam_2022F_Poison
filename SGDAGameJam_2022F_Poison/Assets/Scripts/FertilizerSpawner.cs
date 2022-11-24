using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilizerSpawner : MonoBehaviour
{
    [SerializeField] Fertilizer _fertilizerPrefab = null;
    [SerializeField] Transform _spawnNode = null;

    Fertilizer _myFertilizer = null;
    bool _hasFertilizer = false;
    
    void OnEnable()
    {
        Customer.OnDeath += SpawnFertilizer;
    }

    void SpawnFertilizer()
    {
        if (!_hasFertilizer)
        {
            _myFertilizer = Instantiate(_fertilizerPrefab, _spawnNode.position, Quaternion.identity);
            _myFertilizer.OnPickUp += ReleaseFertilizer;
            _hasFertilizer = true;
        }
    }

    void ReleaseFertilizer(PlayerPickup player)
    {
        _myFertilizer.OnPickUp -= ReleaseFertilizer;
        _myFertilizer = null;
    }

    void OnDisable()
    {
        Customer.OnDeath -= SpawnFertilizer;
    }
}
