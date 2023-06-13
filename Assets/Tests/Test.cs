using UnityEngine;

public class Test : MonoBehaviour
{

    public Transform sphereCenter;
    public float radius;
    public Transform Rect1;
    public Transform Rect2;
    public Transform Rect3;
    public Transform Rect4;

    public bool test = false;
    
    private void OnDrawGizmos()
    {
        if (!test) { return; }

        Gizmos.color = Color.cyan;
        
        Gizmos.DrawLine(Rect1.position, Rect2.position);
        Gizmos.DrawLine(Rect2.position, Rect3.position);
        Gizmos.DrawLine(Rect3.position, Rect4.position);
        Gizmos.DrawLine(Rect4.position, Rect1.position);

        bool hit = CollisionSystem.SphereToRectangleIntersection(sphereCenter.position, radius, new RectData(Rect1.position, Rect2.position, Rect3.position, Rect4.position));
        Gizmos.color = hit? Color.green : Color.red;
        Gizmos.DrawSphere(sphereCenter.position, radius);
    }
}
