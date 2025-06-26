using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private PlayerBodyController playerBodyController;

    [SerializeField] private Transform rayTransform;
    
    private void Update()
    {
        //Debug.DrawRay(rayTransform.position, rayTransform.forward*5, Color.red);
        if (Physics.Raycast(rayTransform.position, rayTransform.forward, out RaycastHit hit, 5f))
        {
            playerBodyController.SetDontDodge(false);
        }
        else
        {
            playerBodyController.SetDontDodge(true);
        }
    }
    
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
                        playerBodyController.SetHopable(obstacle);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
