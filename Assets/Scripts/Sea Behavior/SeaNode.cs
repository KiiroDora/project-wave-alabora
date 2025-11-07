using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaNode : MonoBehaviour
{
    Rigidbody2D rb2d;

    public List<SeaNode> neighborNodes = new();
    public SeaNode[] leftNeighbors;
    public SeaNode[] rightNeighbors;

    public Vector2 initialPosition;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void InitializeNeighbors()
    {
        // find the neighbors to the left
        leftNeighbors = neighborNodes.FindAll(SeaNode => SeaNode.initialPosition.x < initialPosition.x).ToArray();
        // find the neighbors to the right
        rightNeighbors = neighborNodes.FindAll(SeaNode => SeaNode.initialPosition.x > initialPosition.x).ToArray();
    }

    public IEnumerator SendWave(float force, float dampRate, Vector2 direction, float cooldownAmount)
    {
        if (force > 1f)
        {  // if the force has dampened to a marginal amount, stop the process

            rb2d.AddForce(direction * force);  // add force to node in given direction

            SeaNode[] affectedNeighbors = null;
            if (direction.x <= 0) // if direction is left (or neutral)
            {
                affectedNeighbors = leftNeighbors;
            }

            else // if direction is right
            {
                affectedNeighbors = rightNeighbors;
            }

            yield return new WaitForSeconds(cooldownAmount);

            foreach (SeaNode affectedNeighbor in affectedNeighbors)  // send a dampened wave to any existing affected neighbor
            {
                affectedNeighbor.StartCoroutine(SendWave(force * dampRate, dampRate, direction, cooldownAmount));  
            }
        }

        yield return null;
    }
}
