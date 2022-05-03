using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class SpawnPoint : MonoBehaviour
{
    private Transform grid;
    private RoomGeneration roomGeneration;

    [SerializeField] [Range(1, 4)] [Tooltip("1 - Top; 2 - Right; 3 - Bottom; 4 - Left")]
    private int openingDirection; // 1 - Top; 2 - Right; 3 - Bottom; 4 - Left

    [SerializeField]
    private bool occupied = false;
    public bool Occupied { get { return occupied; } }

    private const float waitTime = 0.25f; // Min time = 0.25f

    private void Awake()
    {
        roomGeneration = FindObjectOfType<RoomGeneration>();
        grid = GameObject.FindGameObjectWithTag("Grid").transform;
    }

    private void Start()
    {
        Invoke(nameof(CreateRoom), waitTime);
        // Invoke(nameof(CreateCloseRoom), 0.5f);
    }

    private void SetOccupied(bool state)
    {
        occupied = state;
    }

    private void CreateRoom()
    {
        if (!Occupied)
        {
            GameObject room = roomGeneration.FetchRoom(openingDirection);
            
            if (room == null) { return; }

            GameObject generatedRoom = Instantiate(room, transform.position, Quaternion.identity, grid);
            roomGeneration.generatedRooms.Add(generatedRoom);

            SetOccupied(true);
        }
    }

    private void CreateCloseRoom()
    {
        GameObject room = roomGeneration.FetchCloserRoom(openingDirection);

        Instantiate(room, transform.parent.position, Quaternion.identity, grid);
        Destroy(gameObject);
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        yield return new WaitForSeconds(0.1f);

        if (!collision.GetComponent<SpawnPoint>().Occupied && !Occupied)
        {
            // GameObject room = roomGeneration.FetchCloserRoom(openingDirection);

            // Instantiate(room, transform.parent.position, Quaternion.identity, grid);
            Destroy(gameObject);
        }

        SetOccupied(true);
    }
}