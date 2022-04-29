using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnemyCreator : MonoBehaviour
{
    private Transform enemyContainer;

    private void Awake() 
    {
        enemyContainer = transform.Find("EnemyContainer");
    }

    
}
