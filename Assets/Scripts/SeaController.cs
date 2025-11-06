using System.Collections.Generic;
using UnityEngine;

public class SeaController : MonoBehaviour
{
    [SerializeField] private GameObject seaNode;
    [SerializeField] private GameObject seaContainer;

    private Dictionary<Vector2, SeaNode> seaNodes;
    public static List<GameObject> seaNodeGameObjects = new();

    public static int quality = 1200;  // affects mesh generation for SeaMeshController


    void Awake()
    {
        GenerateSea(60, 10, -15, -12, 0.5f);
    }

    void Update()
    {
        int time = Mathf.FloorToInt(Time.time);
        if (time != 0 && time % 5 == 0)  // make a small wave every 5 seconds
        {
            int random = Random.Range(0, seaNodeGameObjects.Count);  // pick a random node object
            seaNodes.TryGetValue(seaNodeGameObjects[random].transform.position, out SeaNode node);  // get its node
            node.StartCoroutine(node.SendWave(10f, 0.5f, Vector2.left, 0.5f));
        }

         // node.StartCoroutine(node.SendWave(20f, 0.5f, new Vector2 (-0.5f, 2f), 0.2f)); <-- example for big waves
    }

    public void GenerateSea(int horizontal, int vertical, int offsetX, int offsetY, float unitSize)
    {
        seaNodes = new();

        // we have to instantiate all nodes first, then handle their relations

        for (float x = unitSize + offsetX; x <= horizontal + offsetX; x += unitSize)
        {
            for (float y = unitSize + offsetY; y <= vertical + offsetY; y += unitSize)
            {
                // instantiate new node gameObject
                GameObject spawnedNodeGameObject = Instantiate(seaNode, new Vector3(x, y), Quaternion.identity, seaContainer.transform);
                spawnedNodeGameObject.name = $"SeaNode {x} {y}";
                seaNodeGameObjects.Add(spawnedNodeGameObject);

                // assign initialPosition to the node and add it to seaNodes dictionary
                SeaNode spawnedNode = spawnedNodeGameObject.GetComponent<SeaNode>();
                spawnedNode.initialPosition = new Vector2(x, y);
                seaNodes.Add(spawnedNode.initialPosition, spawnedNode);
            }
        }

        for (float x = unitSize + offsetX; x <= horizontal + offsetX; x += unitSize)
        {
            for (float y = unitSize + offsetY; y <= vertical + offsetY; y += unitSize)
            {
                // get the node gameObject and its SeaNode
                seaNodes.TryGetValue(new Vector2(x, y), out SeaNode spawnedNode);
                GameObject spawnedNodeGameObject = spawnedNode.gameObject;

                // add a SpringJoint2D for each existing node adjacent to it and add those nodes into its list of neighbors
                if (seaNodes.ContainsKey(new Vector2(x - unitSize, y)))
                {
                    seaNodes.TryGetValue(new Vector2(x - unitSize, y), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x + unitSize, y)))
                {
                    seaNodes.TryGetValue(new Vector2(x + unitSize, y), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x, y + unitSize)))
                {
                    seaNodes.TryGetValue(new Vector2(x, y + unitSize), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x, y - unitSize)))
                {
                    seaNodes.TryGetValue(new Vector2(x, y - unitSize), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x - unitSize, y - unitSize)))
                {
                    seaNodes.TryGetValue(new Vector2(x - unitSize, y - unitSize), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x + unitSize, y - unitSize)))
                {
                    seaNodes.TryGetValue(new Vector2(x + unitSize, y - unitSize), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x - unitSize, y + unitSize)))
                {
                    seaNodes.TryGetValue(new Vector2(x - unitSize, y + unitSize), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x + unitSize, y + unitSize)))
                {
                    seaNodes.TryGetValue(new Vector2(x + unitSize, y + unitSize), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }

                spawnedNode.InitializeNeighbors();  // adds neighbors on the left and right to Array inside the SeaNode script

                // connect each joint on the node to one neighbor, forming a mesh like structure from springs
                SpringJoint2D[] joints = spawnedNodeGameObject.GetComponents<SpringJoint2D>();
                if (joints.Length > 0)
                {
                    for (int i = 0; i < joints.Length; i++)
                    {
                        joints[i].connectedBody = spawnedNode.neighborNodes[i].GetComponent<Rigidbody2D>();
                    }
                }

                // set the bordering nodes to Static so that the sea keeps in place
                if (y == unitSize + offsetY || x == unitSize + offsetX || x - offsetX == horizontal)
                {
                    spawnedNodeGameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                }
            }
        }

    }
}
