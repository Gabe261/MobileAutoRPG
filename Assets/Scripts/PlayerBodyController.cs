using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBodyController : MonoBehaviour
{
    [SerializeField] private GameObject playerDefeat;

    public UnityEvent OnPlayerHit;

    private Vector2 startTouch;
    private Vector2 endTouch;
    
    private Coroutine movementCoroutine;
    
    private void Start()
    {
        OnPlayerHit ??= new UnityEvent();
        playerDefeat.gameObject.SetActive(false);

        movementCoroutine = null;
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouch = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouch = Input.GetTouch(0).position;

            DoEndTouchLogic();
        }
    }

    private void DoEndTouchLogic()
    {
        // Debug.Log($"Start Touch X: {startTouch.x}, Y: {startTouch.y}, End Touch X: {endTouch.x}, End Touch Y: {endTouch.y}");
        
        // Debug.Log($"TOUCH DIFFERENCE: {startTouch.magnitude - endTouch.magnitude}");
        
        if (Math.Abs(startTouch.magnitude - endTouch.magnitude) < 50)
        {
            HandleTap();
            return;
        }
        
        float heightDifference = startTouch.y - endTouch.y, widthDifference = startTouch.x - endTouch.x;

        if (Math.Abs(heightDifference) > Math.Abs(widthDifference))
        {
            if (endTouch.y < startTouch.y)
            {
                HandleDown();
            } 
            else if (endTouch.y > startTouch.y)
            {
                HandleUp();
            }
        }
        else
        {
            if (endTouch.x < startTouch.x)
            {
                HandleLeft();
            } 
            else if (endTouch.x > startTouch.x)
            {
                HandleRight();
            }
        }
    }

    private void HandleTap()
    {
        Debug.Log("Tap");
    }

    private void HandleLeft()
    {
        StartMoveCoroutine(-0.1f, 0);
        Debug.Log("Left");
    }

    private void HandleRight()
    {
        StartMoveCoroutine(0.1f, 0);
        Debug.Log("Right");
    }

    private void HandleUp()
    {
        StartMoveCoroutine(0, 0.1f);
        Debug.Log("Up");
    }

    private void HandleDown()
    {
        StartMoveCoroutine(0, -0.1f);
        Debug.Log("Down");
    }


    private void StartMoveCoroutine(float xMovement, float yMovement)
    {
        if (movementCoroutine == null)
        {
            movementCoroutine = StartCoroutine(MovePlayer(xMovement, yMovement));
        }
    }
    
    private IEnumerator MovePlayer(float xMovement, float yMovement)
    {
        float iterations = 15, movementJump = 0.1f, holdDelay = 0.4f, smoothPause = 0.01f;
        for (int i = 0; i < iterations; i++)
        {
            transform.position = new Vector3(transform.position.x + xMovement, transform.position.y + yMovement, transform.position.z);
            yield return new WaitForSeconds(smoothPause);
        }
        yield return new WaitForSeconds(holdDelay);
        for (int i = 0; i < iterations; i++)
        {
            transform.position = new Vector3(transform.position.x - xMovement, transform.position.y - yMovement, transform.position.z);
            yield return new WaitForSeconds(smoothPause);
        }
        movementCoroutine = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ParticlePlayer pp))
        {
            return;
        }
        
        playerDefeat.gameObject.transform.SetParent(null);
        GetComponent<MeshRenderer>().enabled = false;
        playerDefeat.gameObject.SetActive(true);
        OnPlayerHit?.Invoke();
    }
}
