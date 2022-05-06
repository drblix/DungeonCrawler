using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class RoomGeneration : MonoBehaviour
{
    private AstarPath pathfinder;
    private Transform grid;

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private AudioSource musicPlayer;

    [Header("Rooms")]

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

    public List<GameObject> generatedRooms = new List<GameObject>();

    private static int generationSeed;
    public static int GenerationSeed { get { return generationSeed; } }

    private const int minimumRooms = 6;
    private const int maximumRooms = 25;

    private static bool usingRandomSeed = true;

    bool doneLoading = false;
    public bool DoneLoading { get { return doneLoading; } }

    private void Awake() 
    {
        if (!usingRandomSeed)
        {
            Random.InitState(generationSeed);
        }
    }

    private void Start()
    {
        pathfinder = FindObjectOfType<AstarPath>();
        grid = GameObject.FindGameObjectWithTag("Grid").transform;
        generatedRooms.Clear();
        FindObjectOfType<Player>().ToggleEnabled(false);
        StartCoroutine(LoadingCleanup(6f));
    }

    public GameObject FetchRoom(int openDirection)
    {
        if (generatedRooms.Count >= maximumRooms)
        {
            if (doneLoading) { return null; }

            doneLoading = true;
            return null;
        }

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

    private IEnumerator LoadingCleanup(float delay)
    {
        Debug.Log("Starting coroutine!");

        yield return new WaitForSeconds(delay);

        // Gives time for trigger colliders to register
        yield return new WaitForSeconds(2f);

        // <summary> Instantiates closer rooms at door points that aren't
        // connected to another room </summary>

        Debug.Log("Instantiating closers");
        int num1 = 0;

        DoorPoint[] doorPoints = FindObjectsOfType<DoorPoint>();

        foreach (DoorPoint point in doorPoints)
        {
            if (!point.Connected)
            {
                point.CreateCloser();
                num1++;
            }
        }

        Debug.Log(string.Format("{0} closers instantiated", num1));


        // <summary> Destroys overlaying rooms that spawn randomly throughout the
        // generation process by checking if the x-coordinate can be converted into
        // an integer value (super inefficient but it works for me lul :]) </summary>

        Debug.Log("Deleting overlaying rooms");
        int num2 = 0;

        foreach (Transform child in grid)
        {
            string x = child.position.x.ToString();

            if (!int.TryParse(x, out _))
            {
                Destroy(child.gameObject);
                generatedRooms.Remove(child.gameObject);
                num2++;
            }
        }

        Debug.Log(string.Format("Deleted {0} overlaying rooms", num2));

        // <summary> Checks if the amount of generated rooms is less than the minimum required rooms.
        // If so, reloads the scene </summary>

        if (generatedRooms.Count < minimumRooms)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // <summary> Grabs the last room added to the list of generated rooms and makes
        // it the boss room </summary>

        GameObject bossRoom = generatedRooms[generatedRooms.Count - 1];
        RoomContentCreator creator1 = bossRoom.GetComponent<RoomContentCreator>();
        
        if (creator1 != null)
        {
            creator1.ToggleBossRoom(true);
        }

        foreach (GameObject room in generatedRooms)
        {
            RoomContentCreator creator2 = room.GetComponent<RoomContentCreator>();
            
            if (creator2 != null)
            {
                creator2.GenerateContent();
            }
        }

        // <summary> Finishes loading </summary>
        pathfinder.Scan();
        loadingScreen.SetActive(false);
        musicPlayer.Play();
        FindObjectOfType<Player>().ToggleEnabled(true);
    }

    public static void SetGenerationSeed(int newSeed)
    {
        usingRandomSeed = false;
        generationSeed = newSeed;
    }

    public static void RandomGenerationSeed()
    {
        usingRandomSeed = true;
        Random.InitState((int)System.DateTime.Now.Ticks);
    }
}