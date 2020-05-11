using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
public abstract class BasePawnStateController : MonoBehaviour
{
    public BasePawn pawn { get; set; }
    public Animator animator { get { return pawn.animator; } }

    Dictionary<int, BasePawnState> pawnStateDict;

    public BasePawnState currentState { get; protected set; }
    public BasePawnState previousState { get; protected set; }

    public bool forceNextCrossFadeInstant { get; set; }

    public float lastManualStateChangeTime { get; protected set; }
    public int lastStateManualChangeFrame { get; protected set; }

    public bool IsWaitingToApplyNewState { get { return needApplyState; } }

    AnimatorStateInfo currentAnimatorStateInfo;
    AnimatorStateInfo previousAnimatorStateInfo;

    bool needApplyState = false;
    bool waitingToEnterInNewAnimatorState;
    bool resetting;

    Coroutine updateCoroutine;

    const int layerIndex = 0;

    protected virtual bool debugLogStateChanges { get { return false; } }
    protected virtual bool debugLogDetail { get { return false; } }

    public Action<BasePawnState, BasePawnState, bool> OnPawnStateChanged;

    public void Init(BasePawn p)
    {
        pawn = p;

        pawnStateDict = new Dictionary<int, BasePawnState>();

        InitPawnStates();

        currentState = pawnStateDict[GetDefaultState()];

        if (gameObject.activeInHierarchy && updateCoroutine == null)
        {
            updateCoroutine = StartCoroutine(ProcessUpdate());
        }
    }

    void OnEnable()
    {
        if (updateCoroutine == null)
        {
            updateCoroutine = StartCoroutine(ProcessUpdate());
        }
    }

    void OnDisable()
    {
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }
    }

    protected virtual void OnDestroy()
    {
        if (currentState != null)
        {
            currentState.OnCurrentStateDestroy();
        }
    }

    //TODO: prevent another state to be triggered during reset?
    public virtual void HardReset()
    {
        if (debugLogDetail)
        {
            Debug.Log("HardReset for " + gameObject.name + " - " + Time.frameCount);
        }

        resetting = true;

        lastManualStateChangeTime = Time.time;
        lastStateManualChangeFrame = Time.frameCount;

        currentState.OnExit(pawnStateDict[GetResetState()], false);
        animator.Play(GetResetState(), 0, UnityEngine.Random.Range(0f, 1f));
        currentState = pawnStateDict[GetResetState()];

        previousState = null;
        needApplyState = false;
        waitingToEnterInNewAnimatorState = false;
        forceNextCrossFadeInstant = false;
    }

    protected virtual int GetDefaultState()
    {
        return HashManager.Instance.SDefault;
    }
    protected virtual int GetResetState()
    {
        return HashManager.Instance.SDefault;
    }

    public virtual void InitPawnStates()
    {
        AddState(GetDefaultState());
    }

    public void AddState(int stateHash)
    {
        pawnStateDict[stateHash] = CreateState(stateHash);
        pawnStateDict[stateHash].Init();
    }

    protected virtual BasePawnState CreateState(int stateHash)
    {
        return PawnStateFactory.CreateState(stateHash, pawn);
    }

    public void AddState(BasePawnState state)
    {
        pawnStateDict[state.stateHash] = state;
        state.Init();
    }

    public BasePawnState GetState(int stateHash)
    {
        BasePawnState resultState = null;
        if (!pawnStateDict.TryGetValue(stateHash, out resultState))
        {
            Debug.LogError(this + " Pawn.GetState can't find state: " + GetNameForHash(stateHash) + "; did you add it in InitPawnStates?");

            // Get the default state
            if (!pawnStateDict.TryGetValue(GetDefaultState(), out resultState))
            {
                Debug.LogError(this + " can't find default state either.");
            }
        }
        return resultState;
    }

    public void GoToState(int nextStateHash)
    {
        if (!enabled || !gameObject.activeSelf)
        {
            Debug.LogWarning("GoToState shouldn't be called on a disabled PawnStateController! This call will be ignored.", gameObject);
            return;
        }
        if (debugLogDetail || debugLogStateChanges)
        {
            Debug.Log("Manual GoToState " + GetNameForHash(nextStateHash) + " for " + gameObject.name + " - " + Time.frameCount);
        }
        GoToState(nextStateHash, false);
    }

    private void GoToState(int nextStateHash, bool animatorAutomaticTransition)
    {
        if (resetting)
            return;

        if (debugLogDetail || debugLogStateChanges)
        {
            Debug.Log("GoToState " + GetNameForHash(nextStateHash) + " (automatic animator transition: " + animatorAutomaticTransition + ") " + Time.frameCount);
        }

        if (needApplyState && nextStateHash == currentState.stateHash)
        {
            return;
        }

        previousState = currentState;

        previousAnimatorStateInfo = currentAnimatorStateInfo;

        currentState = GetState(nextStateHash);

        if (previousState != null)
        {
            previousState.OnExit(currentState, animatorAutomaticTransition);
        }
        currentState.OnEnter(previousState);

        lastManualStateChangeTime = Time.time;
        lastStateManualChangeFrame = Time.frameCount;

        if (!animatorAutomaticTransition)
        {
            needApplyState = true;
        }

        OnPawnStateChanged(previousState, currentState, animatorAutomaticTransition);
    }

    public float GetCurrentStateNormalizedTime()
    {
        if (needApplyState || lastStateManualChangeFrame == Time.frameCount)
            return 0f;

        return currentAnimatorStateInfo.normalizedTime;
    }
    // Warning: depending on where you're using this, you may need to add Time.deltaTime / CurrentStateLength in order to apply it immediately
    public void SetCurrentStateNormalizedTime(float normalizedTime)
    {
        animator.Play(0, 0, normalizedTime);
    }

    //Warning: it's not bound to be exact if called from LateUpdate
    public float GetCurrentStateRealRemainingTime()
    {
        if (needApplyState)
        {
            //We're not in current state, so there's no way to know its duration
            return 999f;
        }
        return currentAnimatorStateInfo.length * (1f - GetCurrentStateNormalizedTime());
    }

    public float GetCurrentStateLength()
    {
        return currentAnimatorStateInfo.length;
    }

    public bool IsInTransition()
    {
        return animator.IsInTransition(layerIndex);
    }
    public float GetTransitionProgress()
    {
        return animator.GetAnimatorTransitionInfo(0).normalizedTime;
    }

    void Update()
    {
        if (currentState.ShouldTriggerStateEnd())
        {
            currentState.TriggerStateEnd();
        }
    }

    IEnumerator ProcessUpdate()
    {
        yield return null;
        while (true)
        {
            if (needApplyState)
            {
                needApplyState = false;
                ApplyState();
            }
            if (currentState != null)
            {
                currentState.Update();
            }
            yield return null;
        }
    }

    void LateUpdate()
    {
        if (animator.runtimeAnimatorController == null)
            return;

        if (resetting)
        {
            resetting = false;
            currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
            previousAnimatorStateInfo = currentAnimatorStateInfo;
        }

        if (animator && animator.enabled && animator.isInitialized)
        {
            currentAnimatorStateInfo = animator.IsInTransition(layerIndex) ? animator.GetNextAnimatorStateInfo(layerIndex)
                : animator.GetCurrentAnimatorStateInfo(layerIndex);

            // It can happen that the animator is playing a transition from a state to itself
            if (currentAnimatorStateInfo.fullPathHash != previousAnimatorStateInfo.fullPathHash ||
                (previousAnimatorStateInfo.fullPathHash == currentState.stateHash && waitingToEnterInNewAnimatorState))
            {
                if (animator.IsInTransition(layerIndex) &&
                    animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash != previousAnimatorStateInfo.fullPathHash
                    && previousAnimatorStateInfo.fullPathHash != 0)
                {
                    Debug.LogError("Animator is in transition from state " + GetNameForHash(animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash)
                        + " whereas " + GetNameForHash(previousAnimatorStateInfo.fullPathHash) + " was expected;"
                        + " (going to " + GetNameForHash(animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash) + ") - " + Time.frameCount);
                    Debug.Break();
                }
                else if (waitingToEnterInNewAnimatorState)
                {
                    if (currentAnimatorStateInfo.fullPathHash == currentState.stateHash)
                    {
                        waitingToEnterInNewAnimatorState = false;
                        previousAnimatorStateInfo = currentAnimatorStateInfo;
                        if (debugLogDetail)
                        {
                            Debug.Log("Did enter in awaited animator state with remaining time "
                                + (currentAnimatorStateInfo.loop ? " (looping) " : ((1f - currentAnimatorStateInfo.normalizedTime) * currentAnimatorStateInfo.length).ToString())
                                + " - " + Time.frameCount + " - " + Time.time);
                            Debug.Log("Set previousAnimatorStateInfo to " + GetNameForHash(previousAnimatorStateInfo.fullPathHash));
                        }
                    }
                    else
                    {
                        Debug.LogError("Animator for " + pawn.gameObject.name + " changed state but didn't reach expected one: "
                            + "Current animator state : " + currentAnimatorStateInfo.fullPathHash + " / " + GetNameForHash(currentAnimatorStateInfo.fullPathHash)
                            + " And expected animator state: " + currentState.stateHash + " / " + GetNameForHash(currentState.stateHash) + " - " + Time.frameCount);
                        if (debugLogDetail)
                            Debug.Break();
                    }
                }
                else if ((currentState.HasAutomaticAnimatorEndTransition() || currentState.IsDefault()) &&
                    (animator.IsInTransition(layerIndex) || lastStateManualChangeFrame != Time.frameCount))
                {
                    if (debugLogDetail)
                    {
                        Debug.Log("Transition started to " + GetNameForHash(currentAnimatorStateInfo.fullPathHash)
                            + (animator.IsInTransition(layerIndex) ? (" from " + GetNameForHash(animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash)) : "(not in transition anymore)")
                            + "; going to new pawn state controller state. " + Time.frameCount + " - " + Time.time);
                    }
                    GoToState(currentAnimatorStateInfo.fullPathHash, true);
                }
                else if (lastStateManualChangeFrame == Time.frameCount && previousState.HasAutomaticAnimatorEndTransition())
                {
                    previousAnimatorStateInfo = currentAnimatorStateInfo;
                }
                else
                {
                    Debug.LogError("Error: animator for " + pawn.gameObject.name + " unexpectedely changed state; CurrentState " + currentState.ToString() + " / In transition: " + animator.IsInTransition(layerIndex)
                        + " with current " + GetNameForHash(currentAnimatorStateInfo.fullPathHash) + " and previous " + GetNameForHash(previousAnimatorStateInfo.fullPathHash) + " - " + Time.frameCount);
                    if (debugLogDetail)
                        Debug.Break();
                }
            }
            else if (waitingToEnterInNewAnimatorState)
            {
                Debug.LogWarning("Warning: waiting to enter in new animator state " + GetNameForHash(currentState.stateHash) + " but still in previous one " + GetNameForHash(currentAnimatorStateInfo.fullPathHash) + ". " + Time.frameCount);
                if (animator.IsInTransition(layerIndex))
                {
                    Debug.LogWarning(">Animator is in transition from " + GetNameForHash(animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash)
                        + " to " + GetNameForHash(currentAnimatorStateInfo.fullPathHash));
                }
                else
                {
                    Debug.LogWarning(">Animator is playing " + GetNameForHash(currentAnimatorStateInfo.fullPathHash), pawn);
                }
                if (debugLogDetail)
                    Debug.Break();
            }
        }

        if (currentState != null)
        {
            currentState.LateUpdate();
        }
    }

    protected virtual void ApplyState()
    {
        if (pawn.animator.IsInTransition(layerIndex))
        {
            if (debugLogDetail)
            {
                Debug.Log("Update previous AnimatorStateInfo from " + GetNameForHash(previousAnimatorStateInfo.fullPathHash)
                    + " to " + GetNameForHash(pawn.animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash)
                    + " because applying state with animator still in transition - " + Time.frameCount);
            }
            previousAnimatorStateInfo = pawn.animator.GetCurrentAnimatorStateInfo(layerIndex);
        }

        AnimatorStateInfo asi = pawn.animator.GetCurrentAnimatorStateInfo(layerIndex);
        float transitionDuration = currentState.GetCrossfadeDuration(previousState);
        float normalizedTransitionDuration = transitionDuration / asi.length;
        if (forceNextCrossFadeInstant)
        {
            transitionDuration = 0f;
            normalizedTransitionDuration = 0f;
            forceNextCrossFadeInstant = false;
        }
        float normalizedTime = currentState.GetCrossfadeNormalizedTime(previousState, currentAnimatorStateInfo.normalizedTime);
        if (debugLogDetail)
        {
            Debug.Log("> Start crossfade to " + GetNameForHash(currentState.stateHash) + " with duration " + transitionDuration + " and normalizedTime " + normalizedTime
                + " with animator in current state " + GetNameForHash(asi.fullPathHash)
                + (asi.loop ? " (looping) " : (" with remaining time " + ((1f - asi.normalizedTime) * asi.length)))
                + (pawn.animator.IsInTransition(layerIndex) ? (" and in transition to " + GetNameForHash(pawn.animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash))
                    : " and not in transition ")
                + " / PSC currentAnimatorStateInfo normalized time: " + currentAnimatorStateInfo.normalizedTime
                + " - " + Time.deltaTime + " - " + Time.time);
        }
        animator.CrossFade(currentState.stateHash, normalizedTransitionDuration, layerIndex, normalizedTime);
        waitingToEnterInNewAnimatorState = true;
    }

    public virtual string GetNameForHash(int hash)
    {
        return HashManager.Instance.GetNameForHash(hash);
    }
}
*/