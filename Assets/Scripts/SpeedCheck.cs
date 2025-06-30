using System.Collections;
using UnityEngine;

public class SpeedCheck : MonoBehaviour
{
    [SerializeField] private float multiplier;
    [SerializeField] private float speed;
    [SerializeField] private bool isPlaying;
    [SerializeField] private Transform rayStart;
    private void Start()
    {
        if (isPlaying)
        {
            StartCoroutine(CalcSpeed());
        }
    }

    private void Update()
    {
        Debug.DrawRay(new Vector3(rayStart.position.x, rayStart.position.y, rayStart.position.z + (speed * multiplier)), rayStart.forward * 5, Color.red, 0.01f);
    }
    
    private IEnumerator CalcSpeed()
    {
        while (isPlaying)
        {
            Vector3 prevPos = transform.position;
            yield return new WaitForSeconds(0.05f);
            speed = Mathf.RoundToInt(Vector3.Distance(transform.position, prevPos) / Time.fixedDeltaTime);
            Debug.Log("1234567890: "+speed);
        }
    }
}
