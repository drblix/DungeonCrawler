using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    // 18 x 10 rooms
    [SerializeField]
    private GameObject[] topRooms;
    [SerializeField]
    private GameObject[] rightRooms;
    [SerializeField]
    private GameObject[] bottomRooms;
    [SerializeField]
    private GameObject[] leftRooms;

    [SerializeField]
    private GameObject[] closerRooms; // 0 = Top; 1 = Right; 2 = Bottom; 3 = Left

    private void Start()
    {
        Invoke(nameof(InstantiateClosers), 5f);
    }

    public GameObject FetchRoom(int openDirection)
    {
        switch (openDirection)
        {
            case 1:
                GameObject room1 = topRooms[Random.Range(0, topRooms.Length)];
                return room1;

            case 2:
                GameObject room2 = rightRooms[Random.Range(0, rightRooms.Length)];
                return room2;

            case 3:
                GameObject room3 = bottomRooms[Random.Range(0, bottomRooms.Length)];
                return room3;

            case 4:
                GameObject room4 = leftRooms[Random.Range(0, leftRooms.Length)];
                return room4;

            default:
                Debug.LogError("No room could be fetched!");
                return null;
        }
    }

    public GameObject FetchCloserRoom(int openDirection)
    {
        switch (openDirection)
        {
            case 1:
                return closerRooms[0];

            case 2:
                return closerRooms[1];

            case 3:
                return closerRooms[2];

            case 4:
                return closerRooms[3];

            default:
                Debug.LogError("Closer room not found!");
                return null;
        }
    }

    private void InstantiateClosers()
    {
        Debug.Log("Instantiating closers");
        int num = 0;

        DoorPoint[] doorPoints = FindObjectsOfType<DoorPoint>();

        foreach (DoorPoint point in doorPoints)
        {
            if (!point.Connected)
            {
                point.CreateCloser();
                num++;
            }
        }

        Debug.Log(string.Format("{0} closers created", num));
    }
}
