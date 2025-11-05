using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SeaSpriteShapeController : MonoBehaviour
{
    // SoftBody2D using sprite shape, code used and edited from LoneX on YouTube

    public SpriteShapeController spriteShape;
    public static List<Transform> points = new();

    private const float SplineOffset = 0.5f;

    private void Awake()
    {
        UpdateVertices();
    }

    private void Update()
    {
        UpdateVertices();
    }

    private void UpdateVertices()
    {
        spriteShape.spline.Clear();

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 vertex = points[i].localPosition;
            Vector2 towardsCenter = (Vector2.zero - vertex).normalized;

            spriteShape.spline.InsertPointAt(i, points[i].position);

            float colliderRadius = points[i].gameObject.GetComponent<CircleCollider2D>().radius;

            try
            {
                spriteShape.spline.SetPosition(i, vertex - towardsCenter * colliderRadius);
            }
            catch
            {
                Debug.Log("Spline too close, recalculate");
                spriteShape.spline.SetPosition(i, vertex - towardsCenter * (colliderRadius + SplineOffset));
            }

            Vector2 leftTangent = spriteShape.spline.GetLeftTangent(i);
            Vector2 newRightTangent = Vector2.Perpendicular(towardsCenter) * leftTangent.magnitude;
            Vector2 newLeftTangent = Vector2.zero - newRightTangent;

            spriteShape.spline.SetRightTangent(i, newRightTangent);
            spriteShape.spline.SetLeftTangent(i, newLeftTangent);
        }
    }
}
