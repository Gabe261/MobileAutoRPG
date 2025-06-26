using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PlayerBodyController : MonoBehaviour
{
    [SerializeField] private GameObject playerAlive;
    [SerializeField] private GameObject playerDefeat;

	[SerializeField] private float speed;

    public UnityEvent OnPlayerHit;
    public UnityEvent OnPlayerWin;
    
    private Vector2 startTouch;
    private Vector2 endTouch;
    
    private Coroutine movementCoroutine;
    
    [SerializeField] private Animator controlAnimator;
    [SerializeField] private Animator appearanceAnimator;
    
    private List<string[]> animationLinks = new List<string[]>();

    private bool breakableObjectReady;
    private bool hoppable;
    private bool dontdodge;
    private IObstacle obstacleToAction;

    public void SetBreakable(IObstacle obstacle)
    {
        breakableObjectReady = true;
        obstacleToAction = obstacle;
    }

    public void SetHopable(IObstacle obstacle)
    {
        hoppable = true;
        obstacleToAction = obstacle;
    }

    public void SetDontDodge(bool toEnable)
    {
        dontdodge = toEnable;
    }
    
    private void Start()
    {
        OnPlayerHit ??= new UnityEvent();
        OnPlayerWin ??= new UnityEvent();
        playerDefeat.gameObject.SetActive(false);
        
        movementCoroutine = null;
        InitializeAnimations();
        StartWalk();
    }

    private void StartWalk()
    {
        appearanceAnimator.Play("Walk");
    }

    private void InitializeAnimations()
    {
        string[] strings = {"LeftDodge", "Walk"}; // 0
        animationLinks.Add(strings);
        
        strings = new string[] {"RightDodge", "Walk"}; // 1
        animationLinks.Add(strings);
        
        strings = new string[] {"Slide", "Ground"}; // 2
        animationLinks.Add(strings);
        
        strings = new string[] {"Jump", "Air", "Flip"}; // 3
        animationLinks.Add(strings);
        
        strings = new string[] {"Style", "Poke"}; // 4
        animationLinks.Add(strings);
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
        if (breakableObjectReady)
        {
            breakableObjectReady = false;
            obstacleToAction.PerformAction();
        }
        PlayAnimations(4);
        Debug.Log("Tap");
    }

    private void HandleLeft()
    {
        PlayAnimations(0);
        //StartMoveCoroutine(-0.1f, 0);
        Debug.Log("Left");
    }

    private void HandleRight()
    {
        if (dontdodge)
        {
            playerAlive.gameObject.SetActive(false);
            playerDefeat.gameObject.SetActive(true);
        }
        else
        {
           PlayAnimations(1); 
        }
        //StartMoveCoroutine(0.1f, 0);
        Debug.Log("Right");
    }

    private void HandleUp()
    {
        if (hoppable)
        {
            hoppable = false;
            PlayAnimations(3, true);
        }
        else
        {
             PlayAnimations(3);
        }
        //StartMoveCoroutine(0, 0.1f);
        Debug.Log("Up");
    }

    private void HandleDown()
    {
        PlayAnimations(2);
        //StartMoveCoroutine(0, -0.1f);
        Debug.Log("Down");
    }

    private void PlayAnimations(int index, bool alternate = false)
    {
        //Debug.Log("Playing animation: " + animationLinks[index][0] + " AND: " + animationLinks[index][1]);
        
        // controlAnimator.Play(animationLinks[index][0]);
        // appearanceAnimator.Play(animationLinks[index][1]);
        
        controlAnimator.CrossFade(animationLinks[index][0], .2f);
        if (alternate)
        {
            appearanceAnimator.CrossFade(animationLinks[index][2], .2f);
        }
        else
        {
            appearanceAnimator.CrossFade(animationLinks[index][1], .2f);
        }
    }
    

    // private void StartMoveCoroutine(float xMovement, float yMovement)
    // {
    //     if (movementCoroutine == null)
    //     {
    //         movementCoroutine = StartCoroutine(MovePlayer(xMovement, yMovement));
    //     }
    // }
    //
    // private IEnumerator MovePlayer(float xMovement, float yMovement)
    // {
    //     float iterations = 15, movementJump = 0.1f, holdDelay = 0.4f, smoothPause = 0.01f;
    //     for (int i = 0; i < iterations; i++)
    //     {
    //         transform.position = new Vector3((transform.position.x + xMovement), (transform.position.y + yMovement), transform.position.z);
    //         yield return new WaitForSeconds(smoothPause);
    //     }
    //     yield return new WaitForSeconds(holdDelay);
    //     for (int i = 0; i < iterations; i++)
    //     {
    //         transform.position = new Vector3((transform.position.x - xMovement), (transform.position.y - yMovement), transform.position.z);
    //         yield return new WaitForSeconds(smoothPause);
    //     }
    //     movementCoroutine = null;
    // }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ParticlePlayer pp))
        {
            OnPlayerWin?.Invoke();
            return;
        }
        
        playerAlive.gameObject.SetActive(false);
        playerDefeat.gameObject.SetActive(true);
        
        foreach (Transform child in playerDefeat.transform)
        {
            child.gameObject.transform.SetParent(null);
            child.GetComponent<Rigidbody>().AddForce(RandomForce(), ForceMode.Impulse);
        }
        
        OnPlayerHit?.Invoke();
    }

    public void PlayerReset()
    {
        playerAlive.gameObject.SetActive(true);
        playerDefeat.gameObject.SetActive(false);
    }
    
    
    private Vector3 RandomForce()
    {
        float forceAmount = 3f;
        return  new Vector3(Random.Range(-forceAmount, forceAmount), Random.Range(-forceAmount, forceAmount), Random.Range(-forceAmount, forceAmount));
    }
}
