using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    [SerializeField] Player _myPlayer = null;
    
    [SerializeField] LayerMask _tileLayers = 0;
    [SerializeField] LayerMask _interactableLayers = 0;
    [SerializeField] LayerMask _tableLayers = 0;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForTile();

            bool clickedInteractable = CheckForInteractable();
            if (!clickedInteractable)
            {
                CheckForTable();
            }
        }
    }

    void CheckForTile()
    {
        Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 dest = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));

        RaycastHit2D hit = Physics2D.Raycast(origin, dest - origin, Mathf.Infinity, _tileLayers);

        if (hit.collider != null)
        {
            Tile hitTile = hit.collider.gameObject.GetComponent<Tile>();
            hitTile.Interact(_myPlayer);
        }
    }

    bool CheckForInteractable()
    {
        Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 dest = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));

        RaycastHit2D hit = Physics2D.Raycast(origin, dest - origin, Mathf.Infinity, _interactableLayers);

        if (hit.collider != null)
        {
            IInteractable hitInteractable = hit.collider.gameObject.GetComponent<IInteractable>();
            hitInteractable.Interact(_myPlayer);
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckForTable()
    {
        Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 dest = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));

        RaycastHit2D hit = Physics2D.Raycast(origin, dest - origin, Mathf.Infinity, _tableLayers);

        if (hit.collider != null)
        {
            Table hitTable = hit.collider.gameObject.GetComponent<Table>();
            hitTable.Interact(_myPlayer);
            return true;
        }
        else
        {
            return false;
        }
    }
}
