using UnityEngine;
using UnityEngine.Events;

public class BreakableObstacle : MonoBehaviour, IObstacle
{
    private bool hasBeenPreformed;
    private UnityEvent ObstacleEventList;
    
    private void Start()
    {
        ObstacleEventList = new UnityEvent();
        ObstacleEventList.AddListener(PreformReaction);
    }
    
    public ObstacleTypes GetObstacleType()
    {
        return ObstacleTypes.Breakable;
    }

    public void PerformAction(PlayerAnimationController player)
    {
        if (hasBeenPreformed) { return; }
        
        player.SetEventListener(DefaultAnimationTypes.Tap, ObstacleEventList);
        hasBeenPreformed = true;
    }

    private void PreformReaction()
    {
        GetComponent<Collider>().enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z); 
    }
}
