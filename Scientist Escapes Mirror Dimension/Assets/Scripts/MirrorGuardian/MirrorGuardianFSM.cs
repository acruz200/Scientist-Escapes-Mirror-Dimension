using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MirrorGuardianState
{
    Patrol,
    Chase,
    Attack,
    Retreat
}

public class MirrorGuardianFSM : MonoBehaviour
{
    [Header("FSM Settings")]
    [SerializeField] private float healthThresholdForRetreat = 0.3f; // Retreat when health is below 30%
    [SerializeField] private float timeToForgetPlayer = 10f; // Time in seconds before forgetting the player
    
    private MirrorGuardianState currentState = MirrorGuardianState.Patrol;
    private MirrorGuardianController controller;
    
    void Start()
    {
        controller = GetComponent<MirrorGuardianController>();
        if (controller == null)
        {
            Debug.LogError("MirrorGuardianFSM requires a MirrorGuardianController component!");
            enabled = false;
            return;
        }
    }
    
    public void UpdateState(MirrorGuardianController guardian)
    {
        // Check for state transitions
        MirrorGuardianState newState = DetermineNewState(guardian);
        
        // If state has changed, update the current state
        if (newState != currentState)
        {
            currentState = newState;
            Debug.Log("Mirror Guardian state changed to: " + currentState);
        }
        
        // Execute the current state
        ExecuteState(guardian);
    }
    
    private MirrorGuardianState DetermineNewState(MirrorGuardianController guardian)
    {
        // Check health first - if health is low, retreat
        if (guardian.GetHealthPercentage() < healthThresholdForRetreat)
        {
            return MirrorGuardianState.Retreat;
        }
        
        // Get current conditions
        bool playerVisible = guardian.IsPlayerVisible();
        float distanceToPlayer = guardian.GetDistanceToPlayer();
        float timeSinceLastSeenPlayer = guardian.GetTimeSinceLastSeenPlayer();
        
        // State transition logic
        switch (currentState)
        {
            case MirrorGuardianState.Patrol:
                // From Patrol to Chase if player is visible
                if (playerVisible)
                {
                    return MirrorGuardianState.Chase;
                }
                break;
                
            case MirrorGuardianState.Chase:
                // From Chase to Attack if player is close enough
                if (playerVisible && distanceToPlayer <= 5f)
                {
                    return MirrorGuardianState.Attack;
                }
                // From Chase to Patrol if player is not visible for too long
                else if (!playerVisible && timeSinceLastSeenPlayer > timeToForgetPlayer)
                {
                    return MirrorGuardianState.Patrol;
                }
                break;
                
            case MirrorGuardianState.Attack:
                // From Attack to Chase if player moves away
                if (distanceToPlayer > 5f)
                {
                    return MirrorGuardianState.Chase;
                }
                // From Attack to Patrol if player is not visible for too long
                else if (!playerVisible && timeSinceLastSeenPlayer > timeToForgetPlayer)
                {
                    return MirrorGuardianState.Patrol;
                }
                break;
                
            case MirrorGuardianState.Retreat:
                // From Retreat to Patrol if health is recovered
                if (guardian.GetHealthPercentage() >= healthThresholdForRetreat)
                {
                    return MirrorGuardianState.Patrol;
                }
                break;
        }
        
        // If no state change is needed, return the current state
        return currentState;
    }
    
    private void ExecuteState(MirrorGuardianController guardian)
    {
        // Execute the appropriate behavior based on the current state
        switch (currentState)
        {
            case MirrorGuardianState.Patrol:
                guardian.Patrol();
                break;
                
            case MirrorGuardianState.Chase:
                guardian.Chase();
                break;
                
            case MirrorGuardianState.Attack:
                guardian.Attack();
                break;
                
            case MirrorGuardianState.Retreat:
                guardian.Retreat();
                break;
        }
    }
    
    public void SetState(MirrorGuardianState newState)
    {
        currentState = newState;
    }
    
    public MirrorGuardianState GetCurrentState()
    {
        return currentState;
    }
} 