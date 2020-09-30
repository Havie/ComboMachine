
// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using UnityEngine;



     public  class ActionAnimator : MonoBehaviour
    {
        /************************************************************************************************************************/

        // Without Animancer, you would reference an Animator component to control animations.
        // But with Animancer, you reference an AnimancerComponent instead.
        [SerializeField] private AnimancerComponent _Animancer;

        // Without Animancer, you would create an Animator Controller to define animation states and transitions.
        // But with Animancer, you can directly reference the AnimationClips you want to play.
        [SerializeField] private ClipState.Transition _Idle;
        [SerializeField] private ClipState.Transition _Walk;

        //Normals
        [SerializeField] private AnimationMoveSet _normal;
        [SerializeField] private AnimationMoveSet _strong;
        //Musou
         [SerializeField] private ClipState.Transition _O;
         //Transitions
        [SerializeField] private ClipState.Transition[] _transToIdle;

        [SerializeField] private ClipState.Transition lastKnown;


        [SerializeField] private SimpleEventReceiver _EventReceiver;

        /************************************************************************************************************************/

        /// <summary>
        /// On startup, play the idle animation.
        /// </summary>
        private void OnEnable()
        {
            // On startup, play the idle animation.
            _Animancer.Play(_Idle).Speed = 0.15f;
        }

        /************************************************************************************************************************/

        private void Update()
        {

        }
        public void ReturnToIdle()
        {
            PlayAnim(-1);
        }

        public void PlayAnim(int id)
        {
            if (id != -1 && id != 0)
                print("heard " + id);

            switch (id)
            {
                case -1:
                    _Animancer.Play(_Idle, 0.3f).Speed = 0.15f;
                    lastKnown.Clip = _Idle.Clip;
                    break;
                case 0:
                    _Animancer.Play(_Walk, 0.3f).Speed = 0.75f;
                    lastKnown.Clip = _Walk.Clip;
                    break;
            
            }
        }

        /************************************************************************************************************************/

        private void OnActionEnd()
        {

            if (lastKnown.Clip == _Walk.Clip)
                print("TRUE");
            // Now that the action is done, go back to idle. But instead of snapping to the new animation instantly,
            // tell it to fade gradually over 0.25 seconds so that it transitions smoothly.
            _Animancer.Play(_Idle, 0.25f).Speed = 0.15f;
        }

        /************************************************************************************************************************/
    
}
