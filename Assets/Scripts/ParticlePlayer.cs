using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    
    private void OnTriggerEnter(Collider other)
    {
        particle.Play();
    }
}
