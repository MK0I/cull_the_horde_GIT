using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Infinite_Room_Generator : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] private Room_Data[] roomPrefabs;

    [Header("Hallways")]
    [SerializeField] private GameObject hallwayNS;
    [SerializeField] private GameObject hallwayWE;

    [Header("Starting Room")]
    [SerializeField] private Room_Data startingRoom;

    private Room_Data currentRoom;

    private List<Vector3> occupiedPositions = new();

    private List<Bounds> occupiedBounds = new();

    private void Start()
    {
        SpawnStartingRoom();

        Invoke(nameof(BeginTest), 1f);
    }

    private void BeginTest()
    {
        Generate(Direction.North);
    }

    private void Generate(Direction travelDirection)
    {
        GameObject hallwayPrefab = (travelDirection == Direction.North || travelDirection == Direction.South)
            ? hallwayNS
            : hallwayWE;

        Transform roomExit = GetRoomConnector(currentRoom, travelDirection);

        if (roomExit == null)
        {
            Debug.Log("Generation finished.");
            return;
        }

        GameObject hallwayObject = Instantiate(hallwayPrefab);

        Hallway_Data hallway = hallwayObject.GetComponent<Hallway_Data>();

        Transform hallwayEntrance = GetHallwayConnector(hallway, GetOppositeDirection(travelDirection));

        hallwayObject.transform.position += roomExit.position - hallwayEntrance.position;

        Direction entrance = GetOppositeDirection(travelDirection);

        List<Room_Data> compatible = new();

        foreach (Room_Data room in roomPrefabs)
        {
            if (RoomSupportsEntrance(room, entrance))
                compatible.Add(room);
        }

        Transform hallwayExit = GetHallwayConnector(hallway, travelDirection);

        Room_Data roomInstance = null;

        foreach (Room_Data candidate in compatible)
        {
            Room_Data testRoom = Instantiate(candidate);

            Transform roomEntrance = GetRoomConnector(testRoom, entrance);

            Vector3 targetPosition = hallwayExit.position - roomEntrance.position;

            testRoom.transform.position = targetPosition;

            if (occupiedPositions.Contains(targetPosition))
            {
                Destroy(testRoom.gameObject);
                continue;
            }

            roomInstance = testRoom;
            break;
        }

        if (roomInstance == null)
        {
            Debug.LogWarning("No valid room placement found.");
            return;
        }

        Bounds newBounds = GetRoomBounds(roomInstance);

        bool overlaps = false;

        foreach (Bounds bounds in occupiedBounds)
        {
            if (bounds.Intersects(newBounds))
            {
                overlaps = true;
                break;
            }
        }

        if (overlaps)
        {
            Destroy(roomInstance.gameObject);
            Debug.Log("Room overlaps existing room.");
            return;
        }

        occupiedPositions.Add(roomInstance.transform.position);

        occupiedBounds.Add(newBounds);

        currentRoom = roomInstance;

        Direction next = GetNextExit(roomInstance, entrance);

        Debug.Log($"{roomInstance.name} -> {next}");

        Invoke(nameof(ContinueGeneration), 1f);

        pendingDirection = next;
    }

    private Direction pendingDirection;

    private Bounds GetRoomBounds(Room_Data room)
    {
        TilemapRenderer[] renderers = room.GetComponentsInChildren<TilemapRenderer>();

        Bounds bounds = renderers[0].bounds;

        foreach (TilemapRenderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }

    private void ContinueGeneration()
    {
        Generate(pendingDirection);
    }

    private Direction GetNextExit(Room_Data room, Direction entrance)
    {
        List<Direction> exits = new();

        if (room.north && entrance != Direction.North)
            exits.Add(Direction.North);

        if (room.south && entrance != Direction.South)
            exits.Add(Direction.South);

        if (room.east && entrance != Direction.East)
            exits.Add(Direction.East);

        if (room.west && entrance != Direction.West)
            exits.Add(Direction.West);

        if (exits.Count == 0)
            return entrance;

        return exits[Random.Range(0, exits.Count)];
    }

    private bool RoomSupportsEntrance(Room_Data room, Direction direction)
    {
        return direction switch
        {
            Direction.North => room.north,
            Direction.South => room.south,
            Direction.East => room.east,
            Direction.West => room.west,
            _ => false
        };
    }

    private Transform GetRoomConnector(Room_Data room, Direction direction)
    {
        return direction switch
        {
            Direction.North => room.northConnector,
            Direction.South => room.southConnector,
            Direction.East => room.eastConnector,
            Direction.West => room.westConnector,
            _ => null
        };
    }

    private Transform GetHallwayConnector(Hallway_Data hallway, Direction direction)
    {
        return direction switch
        {
            Direction.North => hallway.northConnector,
            Direction.South => hallway.southConnector,
            Direction.East => hallway.eastConnector,
            Direction.West => hallway.westConnector,
            _ => null
        };
    }

    private Direction GetOppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            Direction.East => Direction.West,
            Direction.West => Direction.East,
            _ => Direction.North
        };
    }

    private void SpawnStartingRoom()
    {
        currentRoom = Instantiate(startingRoom, Vector3.zero, Quaternion.identity);

        occupiedPositions.Add(currentRoom.transform.position);

        occupiedBounds.Add(GetRoomBounds(currentRoom));
    }
}