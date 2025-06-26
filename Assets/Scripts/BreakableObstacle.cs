using UnityEngine;

public class BreakableObstacle : MonoBehaviour, IObstacle
{
    public ObstacleTypes GetObstacleType()
    {
        return ObstacleTypes.Breakable;
    }

    public void PerformAction()
    {
        GetComponent<Collider>().enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
    }
}
