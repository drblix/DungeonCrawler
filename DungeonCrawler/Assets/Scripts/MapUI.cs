using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    private Player player;
    private Camera mapCamera;
    private Transform mapMstrParent;

    const int maxMapSize = 80;
    const int minMapSize = 25;

    bool mapFull = false;

    private void Awake() 
    {
        player = FindObjectOfType<Player>();
        mapCamera = GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>();
        mapMstrParent = transform.GetChild(0);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.M) && player.PlayerEnabled)
        {
            if (!mapFull)
            {
                mapFull = true;
                mapMstrParent.LeanMoveLocal(Vector2.zero, 0.75f).setEaseOutCubic();
                mapMstrParent.LeanScale(new Vector3(3f, 3f, 3f), 0.75f).setEaseOutCubic();
            }
            else
            {
                mapFull = false;
                mapMstrParent.LeanMoveLocal(new Vector2(273f, 105f), 0.75f).setEaseOutCubic();
                mapMstrParent.LeanScale(Vector3.one, 0.75f).setEaseOutCubic();
            }
        }
    }

    public void MapZoomIn()
    {
        float newSize = mapCamera.orthographicSize + 5f;

        mapCamera.orthographicSize = newSize;

        if (mapCamera.orthographicSize > maxMapSize)
        {
            mapCamera.orthographicSize = maxMapSize;
        }
    }

    public void MapZoomOut()
    {
        float newSize = mapCamera.orthographicSize - 5f;

        mapCamera.orthographicSize = newSize;

        if (mapCamera.orthographicSize < minMapSize)
        {
            mapCamera.orthographicSize = minMapSize;
        }
    }

    public void SizeUpButton(Transform obj)
    {
        UIElementEffects.SizeUpButton(obj);
    }

    public void SizeDownButton(Transform obj)
    {
        UIElementEffects.SizeDownButton(obj);
    }
}