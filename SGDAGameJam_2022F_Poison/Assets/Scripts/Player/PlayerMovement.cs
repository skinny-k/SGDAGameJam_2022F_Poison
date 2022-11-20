using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementSpeed = 1f;
    [SerializeField] float _tileSnapDistance = 0.1f;

    List<Tile> _tilesOccupied = new List<Tile>();
    List<Tile> _pathToFollow = new List<Tile>();
    int _nodeInPath = 0;
    bool _hasPath = false;
    Rigidbody2D _rb;
    Vector3 _movementThisFrame = Vector3.zero;
    
    void OnEnable()
    {
        Tile.OnEnter += AddOccupiedTile;
        Tile.OnExit += RemoveOccupiedTile;

        Tile.OnClick += FindPathToTile;
    }
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        MoveManually();
        if (_hasPath)
        {
            MoveAutomatically();
        }
    }

    void MoveManually()
    {
        _movementThisFrame.x = Input.GetAxis("Horizontal");
        _movementThisFrame.y = Input.GetAxis("Vertical");
        _movementThisFrame.z = 0;

        if (_movementThisFrame != Vector3.zero)
        {
            _movementThisFrame = _movementThisFrame.normalized;
            _movementThisFrame *= _movementSpeed * Time.deltaTime;

            _rb.MovePosition(transform.position + _movementThisFrame);

            _hasPath = false;
        }
    }

    void MoveAutomatically()
    {
        Vector3 posWithoutZ = transform.position;
        posWithoutZ.z = 0;
        if (Vector3.Distance(posWithoutZ, _pathToFollow[_nodeInPath].transform.position) <= _tileSnapDistance)
        {
            _nodeInPath++;
            if (_nodeInPath >= _pathToFollow.Count)
            {
                _hasPath = false;
                // _nodeInPath = 0;
                return;
            }
        }

        _movementThisFrame = _pathToFollow[_nodeInPath].transform.position - transform.position;
        _movementThisFrame.z = 0;

        if (_movementThisFrame != Vector3.zero)
        {
            _movementThisFrame = _movementThisFrame.normalized;
            _movementThisFrame *= _movementSpeed * Time.deltaTime;

            _rb.MovePosition(transform.position + _movementThisFrame);
        }
    }

    Tile GetCurrentTile()
    {
        if (_tilesOccupied.Count == 1)
        {
            return _tilesOccupied[0];
        }
        else
        {
            Tile closestTile = null;
            foreach (Tile tile in _tilesOccupied)
            {
                if (closestTile == null || Vector3.Distance(transform.position, tile.transform.position) < Vector3.Distance(transform.position, closestTile.transform.position))
                {
                    closestTile = tile;
                }
            }

            return closestTile;
        }
    }

    void AddOccupiedTile(Tile tile)
    {
        _tilesOccupied.Add(tile);
    }

    void RemoveOccupiedTile(Tile tile)
    {
        _tilesOccupied.Remove(tile);
        _tilesOccupied.TrimExcess();
    }

    void FindPathToTile(Tile target)
    {
        if (target != null)
        {
            Map.FindPath(GetCurrentTile(), target, out _pathToFollow);
        }

        _nodeInPath = 0;
        _hasPath = true;
    }

    void OnDisable()
    {
        Tile.OnEnter -= AddOccupiedTile;
        Tile.OnExit -= RemoveOccupiedTile;

        Tile.OnClick -= FindPathToTile;
    }
}
