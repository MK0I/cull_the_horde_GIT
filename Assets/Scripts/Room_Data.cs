using UnityEngine;

public class Room_Data : MonoBehaviour
{
    [Header("Room Exits")]
    public bool north;
    public bool south;
    public bool east;
    public bool west;

    [Header("Connectors")]
    public Transform northConnector;
    public Transform southConnector;
    public Transform eastConnector;
    public Transform westConnector;

    [Header("Enemy Spawner")]
    public Enemy_Spawner enemySpawner;

}