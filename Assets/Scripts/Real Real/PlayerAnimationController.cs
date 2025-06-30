using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private AnimationInfoSO tap, doubleTap, hold, leftSwipe, rightSwipe, upSwipe, downSwipe;
    private List<AnimationInfoSO> defaultAnimations;
    
    [SerializeField] private Animator controlAnimator;
    [SerializeField] private Animator appearanceAnimator;

    
    
    private void Start()
    {
        defaultAnimations = new List<AnimationInfoSO>();
        defaultAnimations.Add(tap);
        defaultAnimations.Add(doubleTap);
        defaultAnimations.Add(hold);
        
        defaultAnimations.Add(leftSwipe);
        defaultAnimations.Add(rightSwipe);
        defaultAnimations.Add(upSwipe);
        defaultAnimations.Add(downSwipe);

        foreach (AnimationInfoSO animation in defaultAnimations)
        {
            animation.OverrideAnimations.Clear();
            animation.EventListeners.Clear();
        }
    }
    
    // ============================================================ INPUT CONTROLLER ================================== 
    private Vector2 startTouch;
    private Vector2 endTouch;
    
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
        if (Math.Abs(startTouch.magnitude - endTouch.magnitude) < 50)
        {
            // Tap
            PlayAnimation(tap);
            return;
        }
        
        float heightDifference = startTouch.y - endTouch.y, widthDifference = startTouch.x - endTouch.x;

        if (Math.Abs(heightDifference) > Math.Abs(widthDifference))
        {
            if (endTouch.y < startTouch.y)
            {
                // Down
                PlayAnimation(downSwipe);
            } 
            else if (endTouch.y > startTouch.y)
            {
                // Up 
                PlayAnimation(upSwipe);
            }
        }
        else
        {
            if (endTouch.x < startTouch.x)
            {
                // Left
                PlayAnimation(leftSwipe);
            } 
            else if (endTouch.x > startTouch.x)
            {
                // Right
                PlayAnimation(rightSwipe);
            }
        }
    }
    // ============================================================ INPUT CONTROLLER ================================== 

    private void PlayAnimation(AnimationInfoSO animationInfo)
    {
        controlAnimator.CrossFade(animationInfo.DefaultAnimation, 0.2f);
        
        if (animationInfo.OverrideAnimations.Count > 0)
        {
            appearanceAnimator.CrossFade(animationInfo.OverrideAnimations[0], 0.2f);
            animationInfo.OverrideAnimations.RemoveAt(0);
        }
        else
        {
            appearanceAnimator.CrossFade(animationInfo.DefaultAppearanceAnimation, 0.2f);
        }

        foreach (UnityEvent uEvent in animationInfo.EventListeners)
        {
            uEvent?.Invoke();
        }
    }

    public void SetOverrideAnimation(DefaultAnimationTypes animationType, string overrideName)
    {
        AnimationInfoSO desiredAnimationSO = GetAnimationInfoFromSO(animationType);
        // ADDING OVERRIDE ANIMATION FROM OBSTACLE
        Debug.Log("Overriding animation: " + desiredAnimationSO.DefaultAnimationAssignment + ", With the animation: " + overrideName);
        desiredAnimationSO.OverrideAnimations.Add(overrideName);
    }

    public void SetEventListener(DefaultAnimationTypes animationType, UnityEvent unityEvent)
    {
        AnimationInfoSO desiredAnimationSO = GetAnimationInfoFromSO(animationType);
        // ADDING UNITY EVENT LISTENER TO OBSTACLE
        Debug.Log("Adding event listener to: " + desiredAnimationSO.DefaultAnimationAssignment);
        desiredAnimationSO.EventListeners.Add(unityEvent);
    }

    private AnimationInfoSO GetAnimationInfoFromSO(DefaultAnimationTypes animationType)
    {
        foreach (AnimationInfoSO animationInfo in defaultAnimations)
        {
            if (animationInfo.DefaultAnimationAssignment == animationType)
            {
                return animationInfo;
            }
        }
        Debug.LogError("ERROR: Couldn't find DefaultAnimationTypes enum in PlayerAnimationController/GetAnimationInfoFromSO");
        return null;
    }
}
