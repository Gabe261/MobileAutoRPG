using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AnimationInfoSO", menuName = "AnimationSO/AnimationInfoSO")]
public class AnimationInfoSO : ScriptableObject
{
    public DefaultAnimationTypes DefaultAnimationAssignment;
    [Space]
    [Header("Control and Appearance Animation")]
    public string DefaultAnimation;
    public string DefaultAppearanceAnimation;
    
    /*[HideInInspector]*/ public List<string> OverrideAnimations;
    /*[HideInInspector]*/ public List<UnityEvent> EventListeners;
}
