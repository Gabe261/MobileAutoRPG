using System.Collections;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private Transform rayTransform;

    private Coroutine collisionChecksCoroutine;

    private void Start()
    {
        BeginLevelCollisionChecks();
    }
    
    public void BeginLevelCollisionChecks()
    {
        collisionChecksCoroutine = null;
        collisionChecksCoroutine = StartCoroutine(CollisionChecks());
    }

    public void EndCollisionChecks()
    {
        if (collisionChecksCoroutine != null)
        {
            StopCoroutine(collisionChecksCoroutine);
        }
    }

    private IEnumerator CollisionChecks()
    {
        while (true)
        {
            Debug.DrawRay(transform.position, transform.forward * 8,  Color.blue);
            
            if (Physics.SphereCast(transform.position, 1f, transform.forward * 8, out RaycastHit hit))
            {
                if (hit.transform.gameObject.TryGetComponent<MonoBehaviour>(out MonoBehaviour component) && component is IObstacle)
                {
                    IObstacle obstacle = component as IObstacle;
                    obstacle.PerformAction(playerAnimationController);
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    // private void Update()
    // {
    //     Debug.DrawRay(rayTransform.position, rayTransform.forward*5, Color.red);
    //      if (Physics.Raycast(rayTransform.position, rayTransform.forward, out RaycastHit hit, 5f))
    //      {
    //          playerBodyController.SetDontDodge(false);
    //      }
    //      else
    //      {
    //          playerBodyController.SetDontDodge(true);
    //      }
    // }
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.TryGetComponent<MonoBehaviour>(out MonoBehaviour component))
    //     {
    //         if (component is IObstacle)
    //         {
    //             IObstacle obstacle = component as IObstacle;
    //
    //             switch (obstacle.GetObstacleType())
    //             {
    //                 case ObstacleTypes.Breakable:
    //                     playerBodyController.SetBreakable(obstacle);
    //                     break;
    //                 case ObstacleTypes.Low:
    //                     playerBodyController.SetHopable(obstacle);
    //                     break;
    //                 default:
    //                     break;
    //             }
    //         }
    //     }
    // }
}
