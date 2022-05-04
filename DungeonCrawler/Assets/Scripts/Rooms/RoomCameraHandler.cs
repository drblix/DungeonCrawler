using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraHandler : MonoBehaviour
{
    private Transform plrCamera;
    private GameObject roomObjContainer;

    [SerializeField]
    private bool isStarterRoom = false;

    private void Awake() 
    {
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

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Moving camera!");

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
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            if (roomObjContainer != null)
            {
                roomObjContainer.SetActive(false);
            }
        }
    }
}
