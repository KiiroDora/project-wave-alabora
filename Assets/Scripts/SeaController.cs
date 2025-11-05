using System.Collections.Generic;
using UnityEngine;

public class SeaController : MonoBehaviour
{
    [SerializeField] private GameObject seaNode;
    [SerializeField] private GameObject seaContainer;

    private Dictionary<Vector2, SeaNode> seaNodes;
    [SerializeField] public static List<GameObject> seaNodeGameObjects = new();

    public static int quality = 1200;


    void Awake()
    {
        GenerateSea(60, 10, -15, -12, 0.5f);
    }

    public void GenerateSea(int horizontal, int vertical, int offsetX, int offsetY, float unitSize)
    {
        seaNodes = new();

        for (float x = unitSize + offsetX; x <= horizontal + offsetX; x += unitSize)
        {
            for (float y = unitSize + offsetY; y <= vertical + offsetY; y += unitSize)
            {
                GameObject spawnedNodeGameObject = Instantiate(seaNode, new Vector3(x, y), Quaternion.identity, seaContainer.transform);
                spawnedNodeGameObject.name = $"SeaNode {x} {y}";
                seaNodeGameObjects.Add(spawnedNodeGameObject);

                SeaNode spawnedNode = spawnedNodeGameObject.GetComponent<SeaNode>();
                seaNodes.Add(new Vector2(x, y), spawnedNode);
            }
        }

        for (float x = unitSize + offsetX; x <= horizontal + offsetX; x += unitSize)
        {
            for (float y = unitSize + offsetY; y <= vertical + offsetY; y += unitSize)
            {
                seaNodes.TryGetValue(new Vector2(x, y), out SeaNode spawnedNode);
                GameObject spawnedNodeGameObject = spawnedNode.gameObject;

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


                SpringJoint2D[] joints = spawnedNodeGameObject.GetComponents<SpringJoint2D>();
                if (joints.Length > 0)
                {
                    for (int i = 0; i < joints.Length; i++)
                    {
                        joints[i].connectedBody = spawnedNode.neighborNodes[i].GetComponent<Rigidbody2D>();
                    }
                }

                if (y == unitSize + offsetY || x == unitSize + offsetX || x - offsetX == horizontal)
                {
                    spawnedNodeGameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                }
            }
        }

    }
}
