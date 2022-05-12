using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraHandler : MonoBehaviour
{
    private Transform plrCamera;
    private GameObject roomObjContainer;

    [SerializeField]
    private EdgeCollider2D barrier;

    [SerializeField]
    private bool isStarterRoom = false;

    private static bool canMoveRooms = true;

    [SerializeField]
    int numEnemies = 0;

    private void Awake() 
    {
        canMoveRooms = true;
        
        plrCamera = Camera.main.transform;

        if (!isStarterRoom)
        {
            roomObjContainer = transform.parent.Find("ObjectContainer").gameObject;
        }

        GetComponent<BoxCollider2D>().size = new Vector2Int(16, 17);
    }

    private void Start() 
    {
        if (isStarterRoom)
        {
            Vector3 cameraPos = transform.position;
            cameraPos.y += 1f;
            cameraPos.z = -10f;

            plrCamera.position = cameraPos;
        }
    }

    public void ContentGenerated()
    {
        foreach (Transform child in roomObjContainer.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                numEnemies++;
            }
        }
    }

    private IEnumerator CheckEnemies()
    {
        while (numEnemies > 0)
        {
            /*
            foreach (Transform child in roomObjContainer.transform)
            {
                if (child.CompareTag("Enemy"))
                {
                    EnemyHealth enemyHealth = child.gameObject.GetComponent<EnemyHealth>();

                    if (enemyHealth.Dead)
                    {
                        numEnemies--;
                    }
                }
            }
            */

            Debug.Log(numEnemies);
            yield return new WaitForSeconds(1f);
        }

        canMoveRooms = true;
        barrier.enabled = false;
    }

    public void EnemyDied()
    {
        numEnemies--;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && canMoveRooms)
        {
            if (!isStarterRoom)
            {
                if (numEnemies > 0)
                {
                    canMoveRooms = false;
                    barrier.enabled = true;

                    StartCoroutine(CheckEnemies());
                }
            }

            Vector3 cameraPos = transform.position;
            cameraPos.y += 1f;
            cameraPos.z = -10f;

            plrCamera.LeanMove(cameraPos, 0.75f).setEaseOutSine();
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            if (roomObjContainer != null)
            {
                roomObjContainer.SetActive(true);
                
                foreach (Transform child in roomObjContainer.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && canMoveRooms)
        {
            if (roomObjContainer != null)
            {
                foreach (Transform child in roomObjContainer.transform)
                {
                    if (!child.CompareTag("Enemy"))
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                //roomObjContainer.SetActive(false);
            }
        }
    }
}
