using System.Collections.Generic;
using UnityEngine;

public class SeaMeshController : MonoBehaviour
{
    // Credits --> Code referenced and edited from Ilham Effendi's Dynamic 2D Water

    /* Note: Current amount of vertexes are not good enough for a quality mesh, but too many nodes cause memory issues
    * We need to find a way to create additional vertexes between nodes without increasing the actual amount of nodes
    * But this is not a priority at the moment
    */ 

    [Header("Water Settings")]
    public Material waterMaterial;
    private List<Vector3> vertices = new();
    private Mesh mesh;

    void Start()
    {
        vertices = new();
        foreach (GameObject node in SeaController.seaNodeGameObjects.ToArray())  // add all initial node positions to vertices
        {
            vertices.Add(node.transform.position);
        }
        GenerateMesh();
    }

    private void Update()
    {
        vertices.Clear();  // wipe the vertices list
        foreach (GameObject node in SeaController.seaNodeGameObjects.ToArray())  // add the updated positions into the list
        {
            vertices.Add(node.transform.position);
        }
        mesh.vertices = vertices.ToArray();
    }

    private void GenerateMesh()
    {
        // generate triangles 
        int[] template = new int[6];
        template[0] = SeaController.quality;
        template[1] = 0;
        template[2] = SeaController.quality + 1;
        template[3] = 0;
        template[4] = 1;
        template[5] = SeaController.quality + 1; 
        int marker = 0;
        int[] tris = new int[(SeaController.quality - 1) * 2 * 3];
        for (int i = 0; i < tris.Length; i++)
        {
            tris[i] = template[marker++]++;
            if (marker >= 6) marker = 0;
        }
        
        // generate mesh
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        if (waterMaterial) meshRenderer.sharedMaterial = waterMaterial;
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}
