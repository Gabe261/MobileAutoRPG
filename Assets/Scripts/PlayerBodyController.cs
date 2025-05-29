using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBodyController : MonoBehaviour
{
    [SerializeField] private GameObject playerDefeat;

    public UnityEvent OnPlayerHit;
    
    private void Start()
    {
        OnPlayerHit ??= new UnityEvent();
        playerDefeat.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        playerDefeat.gameObject.transform.SetParent(null);
        GetComponent<MeshRenderer>().enabled = false;
        playerDefeat.gameObject.SetActive(true);
        OnPlayerHit?.Invoke();
    }
}
