using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    [SerializeField] LayerMask _useLayers = 0;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Vector3 dest = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));

            RaycastHit2D hit = Physics2D.Raycast(origin, dest - origin, Mathf.Infinity, _useLayers);

            if (hit.collider != null)
            {
                Tile hitTile = hit.collider.gameObject.GetComponent<Tile>();
                hitTile.ClickTile();
            }
        }
    }
}
