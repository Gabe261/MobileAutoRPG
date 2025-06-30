using UnityEngine;

public class DodgeAble : MonoBehaviour, IObstacle
{
    private bool hasBeenPreformed;
    public ObstacleTypes GetObstacleType()
    {
        return ObstacleTypes.Low;
    }

    public void PerformAction(PlayerAnimationController player)
    {
        if (hasBeenPreformed) { return; }
        hasBeenPreformed = true;
        
        player.SetOverrideAnimation(DefaultAnimationTypes.Jumping, "Flip");
        GetComponent<Collider>().enabled = false;
    }
}
