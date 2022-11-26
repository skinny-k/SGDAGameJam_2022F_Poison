using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] Customer _customerPrefab = null;
    [SerializeField] float _spawnInterval = 15f;
    [SerializeField] float _spawnChance = 0.50f;
    [SerializeField] Transform[] _spawnNodes = null;
    
    Customer[] _customers;
    float _timer = 0;

    void OnEnable()
    {
        Customer.OnDespawn += RemoveCustomer;
    }
    
    void Start()
    {
        _customers = new Customer[_spawnNodes.Length];
        for (int i = 0; i < _customers.Length; i++)
        {
            _customers[i] = null;
        }

        // spawn initial customers
        if (!SpawnCustomers())
        {
            // force at least one customer to spawn
            int node = Random.Range(0, _customers.Length);
            Customer customerSpawned = Instantiate(_customerPrefab, _spawnNodes[node].position, Quaternion.identity);
            customerSpawned.CustomerIndex = node;
            _customers[node] = customerSpawned;
        }
    }
    
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            SpawnCustomers();
            _timer = 0;
        }
    }

    bool SpawnCustomers()
    {
        bool spawned = false;
        
        for (int i = 0; i < _spawnNodes.Length; i++)
        {
            if (_customers[i] == null && Random.Range(0.0f, 100.0f) <= _spawnChance)
            {
                Customer customerSpawned = Instantiate(_customerPrefab, _spawnNodes[i].position, Quaternion.identity);
                _customers[i] = customerSpawned;
                customerSpawned.CustomerIndex = i;
                spawned = true;
            }
        }

        return spawned;
    }

    void RemoveCustomer(Customer customerToRemove)
    {
        for (int i = 0; i < _customers.Length; i++)
        {
            if (_customers[i] == customerToRemove)
            {
                _customers[i] = null;
            }
        }
    }

    void OnDisable()
    {
        Customer.OnDespawn -= RemoveCustomer;
    }
}
