using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomContentCreator : MonoBehaviour
{
    [SerializeField]
    private Transform objContainer;

    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();

    private bool isBossRoom = false;

    public void GenerateContent()
    {
        if (isBossRoom) { GenerateBoss(); return; }

        bool willMakeEnemies = Random.Range(1, 100) < 50; // 50% chance to make enemies
        bool willSpawnUtilities = Random.Range(1, 100) < 20; // 20% chance to spawn potions etc.
        
        foreach (Transform spawn in spawnPoints)
        {
            bool chance1 = Random.Range(1, 100) < 75; // 75% chance to spawn enemy

            if (willMakeEnemies && chance1 && spawn.gameObject.activeInHierarchy)
            {
                Instantiate(enemies[Random.Range(0, enemies.Count)], spawn.position, Quaternion.identity, objContainer);
                //spawnPoints.Remove(spawn);
                spawn.gameObject.SetActive(false);
            }

            bool chance2 = Random.Range(1, 100) < 25; // 25% chance to spawn a utility

            if (willSpawnUtilities && chance2 && spawn.gameObject.activeInHierarchy)
            {
                //spawnPoints.Remove(spawn);
                spawn.gameObject.SetActive(false);
            }
        }

        objContainer.gameObject.SetActive(false);
    }

    private void GenerateBoss()
    {

    }

    public void ToggleBossRoom(bool state)
    {
        isBossRoom = state;
    }
}
