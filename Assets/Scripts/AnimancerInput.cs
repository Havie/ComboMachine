
 // Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.Events
{
    /// <summary>
    /// An <see cref="GolfHitController"/> that uses Animancer Events configured entirely in the Inspector.
    /// </summary>
    [AddComponentMenu(Strings.MenuPrefix + "Examples/Golf Events - Animancer")]
    [HelpURL(Strings.APIDocumentationURL + ".Examples.AnimationEvents/GolfHitControllerAnimancer")]
    public sealed class AnimancerInput : MonoBehaviour
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
        [SerializeField] private ClipState.Transition _S;
        [SerializeField] private ClipState.Transition _SS;
        [SerializeField] private ClipState.Transition _SSS;
        [SerializeField] private ClipState.Transition _SSSS;
        [SerializeField] private ClipState.Transition _SSSSS;
        [SerializeField] private ClipState.Transition _SSSSSS;
        //Strong
        [SerializeField] private ClipState.Transition _T;
        [SerializeField] private ClipState.Transition _ST;
        [SerializeField] private ClipState.Transition _SST;
        [SerializeField] private ClipState.Transition _SSST;
        [SerializeField] private ClipState.Transition _SSSST;
        [SerializeField] private ClipState.Transition _SSSSST;
        //Musou
        [SerializeField] private ClipState.Transition _O;

        //Transitions
        [SerializeField] private ClipState.Transition _S_toidle;
        [SerializeField] private ClipState.Transition _SS_toidle;
        [SerializeField] private ClipState.Transition _SSS_toidle;
        [SerializeField] private ClipState.Transition _SSSS_toidle;
        [SerializeField] private ClipState.Transition _SSSSS_toidle;
        [SerializeField] private ClipState.Transition _SSSSSS_toidle;
        [SerializeField] private ClipState.Transition _T_toidle;
        [SerializeField] private ClipState.Transition _ST_toidle;
        [SerializeField] private ClipState.Transition _SST_toidle;
        [SerializeField] private ClipState.Transition _SSST_toidle;
        [SerializeField] private ClipState.Transition _SSSST_toidle;
        [SerializeField] private ClipState.Transition _SSSSST_toidle;



        [SerializeField] private ClipState.Transition lastKnown;


        [SerializeField] private SimpleEventReceiver _EventReceiver;

        /************************************************************************************************************************/

        /// <summary>
        /// On startup, play the idle animation.
        /// </summary>
        private void OnEnable()
        {
            // On startup, play the idle animation.
            _Animancer.Play(_Idle).Speed=0.15f;
        }

        /************************************************************************************************************************/

        private void Update()
        {
            // Every update, check if the user has clicked the left mouse button (mouse button 0).
           /* 
             if (Input.GetMouseButtonDown(0))
            {
                // If they have, then play the action animation.
                lastKnown = _Animancer.Play(_Action, 0.25f);
                lastKnown.Speed = 0.5f;

                // The Play method returns the AnimancerState which manages that animation so you can access and
                // control various details, for example:
                // state.Time = 1;// Skip 1 second into the animation.
                // state.NormalizedTime = 0.5f;// Skip halfway into the animation.
                // state.Speed = 2;// Play the animation twice as fast.

                // In this case, we just want it to call the OnActionEnd method (see below) when the animation ends.
                lastKnown.Events.OnEnd = OnActionEnd;
            } 
            */
        }
        public void ReturnToIdle()
        {
            PlayAnim(-1);
        }

        public void PlayAnim(int id)
        {
            if(id!=-1 && id !=0)
                print("heard " + id);

            switch(id)
            {
                case -1:
                     _Animancer.Play(_Idle, 0.3f).Speed = 0.15f;
                    lastKnown.Clip = _Idle.Clip;
                     break;
                case 0:
                    _Animancer.Play(_Walk, 0.3f).Speed = 0.75f;
                    lastKnown.Clip = _Walk.Clip;
                    break;
                case 1:
                    _Animancer.Play(_S, 0.5f).Speed = 0.55f;
                    lastKnown.Clip = _S.Clip;
                   // Time.timeScale = 0.5f;
                    break;
                case 2:
                    _Animancer.Play(_SS, 0.5f).Speed = 0.55f;
                    lastKnown.Clip = _SS.Clip;
                    break;
                case 3:
                    _Animancer.Play(_SSS, 0.5f).Speed = 0.55f;
                    lastKnown.Clip = _SSS.Clip;
                    break;
                case 4:
                    _Animancer.Play(_SSSS, 0.5f).Speed = 0.55f;
                    lastKnown.Clip = _SSSS.Clip;
                    break;
                case 5:
                    _Animancer.Play(_SSSSS, 0.5f).Speed = 0.55f;
                    lastKnown.Clip = _SSSSS.Clip;
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
            _Animancer.Play(_Idle, 0.25f).Speed = 0.15f ;
        }

        /************************************************************************************************************************/
    }
}
