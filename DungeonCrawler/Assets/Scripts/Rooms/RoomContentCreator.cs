using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomContentCreator : MonoBehaviour
{
    [SerializeField]
    private Transform objContainer;

    [SerializeField]
    private GameObject[] bosses;

    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();
    
    [SerializeField]
    private List<GameObject> utilities = new List<GameObject>();

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();

    private bool isBossRoom = false;

    public void GenerateContent()
    {
        if (isBossRoom) { GenerateBoss(); return; }

        int num = Random.Range(1, 100); 
        bool willMakeEnemies;
        bool willSpawnUtilities;

        if (num > 25) // 75% chance to spawn enemies, 25% chance to spawn potions, etc.
        {
            willMakeEnemies = true;
            willSpawnUtilities = false;
        }
        else
        {
            willSpawnUtilities = true;
            willMakeEnemies = false;
        }
        //bool willMakeEnemies = Random.Range(1, 100) < 50; // 50% chance to make enemies
        //bool willSpawnUtilities = Random.Range(1, 100) < 30; // 30% chance to spawn potions etc.
        
        foreach (Transform spawn in spawnPoints)
        {
            bool chance1 = Random.Range(1, 100) < 75; // 75% chance to spawn enemy

            if (willMakeEnemies && chance1 && spawn.gameObject.activeInHierarchy)
            {
                Instantiate(enemies[Random.Range(0, enemies.Count)], spawn.position, Quaternion.identity, objContainer);
                spawn.gameObject.SetActive(false);
            }

            bool chance2 = Random.Range(1, 100) < 25; // 25% chance to spawn a utility

            if (willSpawnUtilities && chance2 && spawn.gameObject.activeInHierarchy)
            {
                Instantiate(utilities[Random.Range(0, utilities.Count)], spawn.position, Quaternion.identity, objContainer);
                spawn.gameObject.SetActive(false);
            }
        }

        objContainer.gameObject.SetActive(false);

        GetComponentInChildren<RoomCameraHandler>().ContentGenerated();
    }

    private void GenerateBoss()
    {
        Debug.Log("Making boss!");
        Instantiate(bosses[Random.Range(0, bosses.Length)], transform.position, Quaternion.identity, objContainer);

        objContainer.gameObject.SetActive(false);
        transform.name = "BossRoom";

        GetComponentInChildren<RoomCameraHandler>().ContentGenerated();
    }

    public void ToggleBossRoom(bool state)
    {
        isBossRoom = state;
    }
}
