using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPoint : MonoBehaviour
{
    private Transform grid;
    private RoomGeneration roomGeneration;

    [SerializeField] [Range(1, 4)] [Tooltip("1 - Top; 2 - Right; 3 - Bottom; 4 - Left")]
    private int openingDirection; // 1 - Top; 2 - Right; 3 - Bottom; 4 - Left

    [SerializeField]
    private bool connected = false;
    public bool Connected { get { return connected; } }

    private void Awake()
    {
        roomGeneration = FindObjectOfType<RoomGeneration>();
        grid = GameObject.FindGameObjectWithTag("Grid").transform;
    }

    public void CreateCloser()
    {
        GameObject room = roomGeneration.FetchCloserRoom(openingDirection);

        Instantiate(room, transform.parent.position, Quaternion.identity, grid);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        connected = true;
    }
}
