using UnityEngine;

public class DodgeAble : MonoBehaviour, IObstacle
{
    public ObstacleTypes GetObstacleType()
    {
        return ObstacleTypes.Low;
    }

    public void PerformAction()
    {
        GetComponent<Collider>().enabled = false;
    }
}
