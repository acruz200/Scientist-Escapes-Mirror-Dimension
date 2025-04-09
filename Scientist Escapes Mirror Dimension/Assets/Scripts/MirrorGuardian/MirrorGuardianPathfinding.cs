using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MirrorGuardianPathfinding : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private int gridWidth = 20;
    [SerializeField] private int gridHeight = 20;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private bool visualizeGrid = false;
    [SerializeField] private Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    [SerializeField] private Color pathColor = Color.green;
    
    private NavMeshAgent navAgent;
    private Vector3 gridOrigin;
    private Node[,] grid;
    private List<Node> currentPath = new List<Node>();
    
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            navAgent = gameObject.AddComponent<NavMeshAgent>();
        }
        
        // Initialize the grid
        InitializeGrid();
    }
    
    private void InitializeGrid()
    {
        // Set the grid origin to the guardian's position
        gridOrigin = transform.position - new Vector3(gridWidth * gridSize * 0.5f, 0, gridHeight * gridSize * 0.5f);
        
        // Create the grid
        grid = new Node[gridWidth, gridHeight];
        
        // Initialize each node
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 worldPoint = gridOrigin + new Vector3(x * gridSize + gridSize * 0.5f, 0, z * gridSize + gridSize * 0.5f);
                bool walkable = !Physics.CheckSphere(worldPoint, gridSize * 0.4f, obstacleLayer);
                
                grid[x, z] = new Node(walkable, worldPoint, x, z);
            }
        }
    }
    
    public Vector3 FindValidPosition(Vector3 targetPosition)
    {
        // Convert world position to grid coordinates
        int targetX = Mathf.RoundToInt((targetPosition.x - gridOrigin.x) / gridSize);
        int targetZ = Mathf.RoundToInt((targetPosition.z - gridOrigin.z) / gridSize);
        
        // Clamp to grid bounds
        targetX = Mathf.Clamp(targetX, 0, gridWidth - 1);
        targetZ = Mathf.Clamp(targetZ, 0, gridHeight - 1);
        
        // If the target position is not walkable, find the nearest walkable position
        if (!grid[targetX, targetZ].walkable)
        {
            // Search in expanding squares until we find a walkable position
            for (int radius = 1; radius < Mathf.Max(gridWidth, gridHeight); radius++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    for (int z = -radius; z <= radius; z++)
                    {
                        // Only check the perimeter of the current radius
                        if (Mathf.Abs(x) == radius || Mathf.Abs(z) == radius)
                        {
                            int checkX = targetX + x;
                            int checkZ = targetZ + z;
                            
                            // Check if the position is within the grid
                            if (checkX >= 0 && checkX < gridWidth && checkZ >= 0 && checkZ < gridHeight)
                            {
                                // If the position is walkable, return it
                                if (grid[checkX, checkZ].walkable)
                                {
                                    return grid[checkX, checkZ].worldPosition;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        // If we couldn't find a walkable position, return the original target position
        return targetPosition;
    }
    
    public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos)
    {
        // Convert world positions to grid coordinates
        Node startNode = WorldToNode(startPos);
        Node endNode = WorldToNode(endPos);
        
        // If either the start or end node is not walkable, find the nearest walkable position
        if (!startNode.walkable)
        {
            startNode = FindNearestWalkableNode(startNode);
        }
        
        if (!endNode.walkable)
        {
            endNode = FindNearestWalkableNode(endNode);
        }
        
        // Initialize open and closed sets
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        
        openSet.Add(startNode);
        
        // A* algorithm
        while (openSet.Count > 0)
        {
            // Find the node with the lowest fCost in the open set
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }
            
            // Remove the current node from the open set and add it to the closed set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            // If we've reached the end node, we've found a path
            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }
            
            // Check each neighbor of the current node
            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                // Skip if the neighbor is in the closed set
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }
                
                // Calculate the new gCost
                float newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                
                // If the new path to the neighbor is shorter or the neighbor is not in the open set
                if (newGCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    // Update the neighbor's costs
                    neighbor.gCost = newGCost;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;
                    
                    // Add the neighbor to the open set if it's not already there
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        
        // If we couldn't find a path, return an empty list
        return new List<Vector3>();
    }
    
    private Node WorldToNode(Vector3 worldPosition)
    {
        // Convert world position to grid coordinates
        int x = Mathf.RoundToInt((worldPosition.x - gridOrigin.x) / gridSize);
        int z = Mathf.RoundToInt((worldPosition.z - gridOrigin.z) / gridSize);
        
        // Clamp to grid bounds
        x = Mathf.Clamp(x, 0, gridWidth - 1);
        z = Mathf.Clamp(z, 0, gridHeight - 1);
        
        return grid[x, z];
    }
    
    private Node FindNearestWalkableNode(Node node)
    {
        // Search in expanding squares until we find a walkable position
        for (int radius = 1; radius < Mathf.Max(gridWidth, gridHeight); radius++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    // Only check the perimeter of the current radius
                    if (Mathf.Abs(x) == radius || Mathf.Abs(z) == radius)
                    {
                        int checkX = node.gridX + x;
                        int checkZ = node.gridZ + z;
                        
                        // Check if the position is within the grid
                        if (checkX >= 0 && checkX < gridWidth && checkZ >= 0 && checkZ < gridHeight)
                        {
                            // If the position is walkable, return it
                            if (grid[checkX, checkZ].walkable)
                            {
                                return grid[checkX, checkZ];
                            }
                        }
                    }
                }
            }
        }
        
        // If we couldn't find a walkable position, return the original node
        return node;
    }
    
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        
        // Check the 8 surrounding nodes
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                // Skip the current node
                if (x == 0 && z == 0)
                {
                    continue;
                }
                
                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;
                
                // Check if the position is within the grid
                if (checkX >= 0 && checkX < gridWidth && checkZ >= 0 && checkZ < gridHeight)
                {
                    neighbors.Add(grid[checkX, checkZ]);
                }
            }
        }
        
        return neighbors;
    }
    
    private float GetDistance(Node nodeA, Node nodeB)
    {
        // Calculate the distance between two nodes
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);
        
        // Diagonal movement costs more
        if (distX > distZ)
        {
            return 14f * distZ + 10f * (distX - distZ);
        }
        else
        {
            return 14f * distX + 10f * (distZ - distX);
        }
    }
    
    private List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;
        
        // Retrace the path from the end node to the start node
        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }
        
        // Add the start node
        path.Add(startNode.worldPosition);
        
        // Reverse the path to get it from start to end
        path.Reverse();
        
        // Store the current path for visualization
        currentPath = new List<Node>();
        foreach (Vector3 pos in path)
        {
            currentPath.Add(WorldToNode(pos));
        }
        
        return path;
    }
    
    public void VisualizePath()
    {
        // This method is called from the MirrorGuardianController to visualize the current path
        if (currentPath.Count > 0)
        {
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Debug.DrawLine(currentPath[i].worldPosition, currentPath[i + 1].worldPosition, pathColor);
            }
        }
    }
    
    void OnDrawGizmos()
    {
        // Visualize the grid
        if (visualizeGrid && grid != null)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridHeight; z++)
                {
                    Gizmos.color = grid[x, z].walkable ? gridColor : Color.red;
                    Gizmos.DrawCube(grid[x, z].worldPosition, Vector3.one * gridSize * 0.9f);
                }
            }
        }
    }
    
    // Node class for A* pathfinding
    public class Node
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridZ;
        
        public float gCost; // Cost from start to this node
        public float hCost; // Cost from this node to end (heuristic)
        public float fCost { get { return gCost + hCost; } } // Total cost
        
        public Node parent; // Parent node for path retracing
        
        public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridZ)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridZ = _gridZ;
        }
    }
} 