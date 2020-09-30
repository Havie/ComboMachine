using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

[CreateAssetMenu(fileName = "AnimationMoveSet", menuName = "AnimationMoveSet")]
public class AnimationMoveSet : ScriptableObject
{
    //public int[] _normal;
    //public int[] _strong;
    //The work around is to make them ints, set size in inspector, then make transitions, or it bugs
    public ClipState.Transition[] _normal;
    public ClipState.Transition[] _strong;
}
