using System.Collections.Generic;
using UnityEngine;

public class SeaController : MonoBehaviour
{
    [SerializeField] private GameObject seaNode;
    [SerializeField] private GameObject seaContainer;

    private Dictionary<Vector2, SeaNode> seaNodes;
    [SerializeField] public static List<GameObject> seaNodeGameObjects = new();

    public static int quality;


    void Awake()
    {
        GenerateSea(30, 10, -15, -12);
    }

    public void GenerateSea(int horizontal, int vertical, int offsetX, int offsetY)
    {
        quality = horizontal * vertical / 2;
    
        seaNodes = new();

        for (int x = 1 + offsetX; x <= horizontal + offsetX; x++)
        {
            for (int y = 1 + offsetY; y <= vertical + offsetY; y++)
            {
                GameObject spawnedNodeGameObject = Instantiate(seaNode, new Vector3(x, y), Quaternion.identity, seaContainer.transform);
                spawnedNodeGameObject.name = $"SeaNode {x} {y}";
                seaNodeGameObjects.Add(spawnedNodeGameObject);

                SeaNode spawnedNode = spawnedNodeGameObject.GetComponent<SeaNode>();
                seaNodes.Add(new Vector2(x, y), spawnedNode);
            }
        }

        for (int x = 1 + offsetX; x <= horizontal + offsetX; x++)
        {
            for (int y = 1 + offsetY; y <= vertical + offsetY; y++)
            {
                seaNodes.TryGetValue(new Vector2(x, y), out SeaNode spawnedNode);
                GameObject spawnedNodeGameObject = spawnedNode.gameObject;

                if (seaNodes.ContainsKey(new Vector2(x - 1, y)))
                {
                    seaNodes.TryGetValue(new Vector2(x - 1, y), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x + 1, y)))
                {
                    seaNodes.TryGetValue(new Vector2(x + 1, y), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x, y + 1)))
                {
                    seaNodes.TryGetValue(new Vector2(x, y + 1), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x, y - 1)))
                {
                    seaNodes.TryGetValue(new Vector2(x, y - 1), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x - 1, y - 1)))
                {
                    seaNodes.TryGetValue(new Vector2(x - 1, y - 1), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x + 1, y - 1)))
                {
                    seaNodes.TryGetValue(new Vector2(x + 1, y - 1), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x - 1, y + 1)))
                {
                    seaNodes.TryGetValue(new Vector2(x - 1, y + 1), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }
                if (seaNodes.ContainsKey(new Vector2(x + 1, y + 1)))
                {
                    seaNodes.TryGetValue(new Vector2(x + 1, y + 1), out SeaNode neighborNode);
                    spawnedNode.neighborNodes.Add(neighborNode);
                    spawnedNodeGameObject.AddComponent(typeof(SpringJoint2D));
                }


                SpringJoint2D[] joints = spawnedNodeGameObject.GetComponents<SpringJoint2D>();
                if (joints.Length > 0)
                {
                    for (int i = 0; i < joints.Length; i++)
                    {
                        joints[i].connectedBody = spawnedNode.neighborNodes[i].GetComponent<Rigidbody2D>();
                    }
                }

                if (y == 1 + offsetY || x == 1 + offsetX || x - offsetX == horizontal)
                {
                    spawnedNodeGameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                }
            }
        }

    }
}
