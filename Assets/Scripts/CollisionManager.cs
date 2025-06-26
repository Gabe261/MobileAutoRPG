using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private PlayerBodyController playerBodyController;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MonoBehaviour>(out MonoBehaviour component))
        {
            if (component is IObstacle)
            {
                IObstacle obstacle = component as IObstacle;

                switch (obstacle.GetObstacleType())
                {
                    case ObstacleTypes.Breakable:
                        playerBodyController.SetBreakable(obstacle);
                        break;
                    case ObstacleTypes.Low:
                        Debug.Log("=== LOW");
                        playerBodyController.SetHopable(obstacle);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
