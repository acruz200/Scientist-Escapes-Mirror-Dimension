using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorGuardianBayesianNetwork : MonoBehaviour
{
    [Header("Bayesian Network Settings")]
    [SerializeField] private int historySize = 10; // Number of historical positions to keep
    [SerializeField] private float predictionConfidence = 0.7f; // Confidence threshold for predictions
    [SerializeField] private float learningRate = 0.1f; // How quickly the network learns from new data
    
    // Player movement history
    private List<Vector3> positionHistory = new List<Vector3>();
    private List<Vector3> directionHistory = new List<Vector3>();
    
    // Movement pattern probabilities
    private float straightLineProbability = 0.5f;
    private float zigzagProbability = 0.3f;
    private float circlingProbability = 0.2f;
    
    // Direction change probabilities
    private float directionChangeProbability = 0.3f;
    private float directionChangeAmount = 45f; // Degrees
    
    // Speed history
    private List<float> speedHistory = new List<float>();
    private float averageSpeed = 5f;
    
    void Start()
    {
        // Initialize with default values
        for (int i = 0; i < historySize; i++)
        {
            positionHistory.Add(Vector3.zero);
            directionHistory.Add(Vector3.forward);
            speedHistory.Add(averageSpeed);
        }
    }
    
    public void UpdateWithPlayerData(Vector3 position, Vector3 direction)
    {
        // Add new position and direction to history
        positionHistory.Add(position);
        directionHistory.Add(direction);
        
        // Calculate speed
        if (positionHistory.Count > 1)
        {
            float speed = Vector3.Distance(position, positionHistory[positionHistory.Count - 2]) / Time.deltaTime;
            speedHistory.Add(speed);
            
            // Update average speed
            averageSpeed = (averageSpeed * (speedHistory.Count - 1) + speed) / speedHistory.Count;
        }
        
        // Trim history to keep only the most recent entries
        if (positionHistory.Count > historySize)
        {
            positionHistory.RemoveAt(0);
            directionHistory.RemoveAt(0);
            speedHistory.RemoveAt(0);
        }
        
        // Update movement pattern probabilities based on recent history
        UpdateMovementPatternProbabilities();
    }
    
    private void UpdateMovementPatternProbabilities()
    {
        if (positionHistory.Count < 3)
        {
            return;
        }
        
        // Count occurrences of each movement pattern
        int straightLineCount = 0;
        int zigzagCount = 0;
        int circlingCount = 0;
        
        for (int i = 2; i < positionHistory.Count; i++)
        {
            Vector3 direction1 = (positionHistory[i] - positionHistory[i - 1]).normalized;
            Vector3 direction2 = (positionHistory[i - 1] - positionHistory[i - 2]).normalized;
            
            float angle = Vector3.Angle(direction1, direction2);
            
            // Straight line: small angle changes
            if (angle < 20f)
            {
                straightLineCount++;
            }
            // Zigzag: medium angle changes
            else if (angle < 90f)
            {
                zigzagCount++;
            }
            // Circling: large angle changes
            else
            {
                circlingCount++;
            }
        }
        
        // Calculate new probabilities
        float total = straightLineCount + zigzagCount + circlingCount;
        if (total > 0)
        {
            float newStraightLineProbability = straightLineCount / total;
            float newZigzagProbability = zigzagCount / total;
            float newCirclingProbability = circlingCount / total;
            
            // Smoothly update probabilities
            straightLineProbability = Mathf.Lerp(straightLineProbability, newStraightLineProbability, learningRate);
            zigzagProbability = Mathf.Lerp(zigzagProbability, newZigzagProbability, learningRate);
            circlingProbability = Mathf.Lerp(circlingProbability, newCirclingProbability, learningRate);
        }
        
        // Update direction change probability
        int directionChangeCount = 0;
        for (int i = 1; i < directionHistory.Count; i++)
        {
            float angle = Vector3.Angle(directionHistory[i], directionHistory[i - 1]);
            if (angle > 20f)
            {
                directionChangeCount++;
            }
        }
        
        float newDirectionChangeProbability = directionChangeCount / (float)(directionHistory.Count - 1);
        directionChangeProbability = Mathf.Lerp(directionChangeProbability, newDirectionChangeProbability, learningRate);
    }
    
    public Vector3 PredictPlayerPosition()
    {
        if (positionHistory.Count < 2)
        {
            return Vector3.zero;
        }
        
        // Get the most recent position and direction
        Vector3 currentPosition = positionHistory[positionHistory.Count - 1];
        Vector3 currentDirection = directionHistory[directionHistory.Count - 1];
        
        // Determine the predicted movement pattern
        float random = Random.value;
        Vector3 predictedDirection = currentDirection;
        
        // Apply direction change based on probability
        if (Random.value < directionChangeProbability)
        {
            float angleChange = Random.Range(-directionChangeAmount, directionChangeAmount);
            predictedDirection = Quaternion.Euler(0, angleChange, 0) * currentDirection;
        }
        
        // Apply movement pattern
        if (random < straightLineProbability)
        {
            // Straight line: continue in the same direction
            return currentPosition + predictedDirection * averageSpeed * Time.deltaTime * 5f;
        }
        else if (random < straightLineProbability + zigzagProbability)
        {
            // Zigzag: change direction slightly
            float zigzagAngle = Random.Range(-45f, 45f);
            Vector3 zigzagDirection = Quaternion.Euler(0, zigzagAngle, 0) * predictedDirection;
            return currentPosition + zigzagDirection * averageSpeed * Time.deltaTime * 5f;
        }
        else
        {
            // Circling: move in a circular pattern
            float circleAngle = Random.Range(-90f, 90f);
            Vector3 circleDirection = Quaternion.Euler(0, circleAngle, 0) * predictedDirection;
            return currentPosition + circleDirection * averageSpeed * Time.deltaTime * 5f;
        }
    }
    
    public float GetPredictionConfidence()
    {
        // Calculate confidence based on the consistency of the movement pattern
        float maxProbability = Mathf.Max(straightLineProbability, zigzagProbability, circlingProbability);
        return maxProbability * predictionConfidence;
    }
    
    public Vector3 GetMostLikelyDirection()
    {
        if (directionHistory.Count == 0)
        {
            return Vector3.forward;
        }
        
        // Return the most recent direction
        return directionHistory[directionHistory.Count - 1];
    }
    
    public float GetAverageSpeed()
    {
        return averageSpeed;
    }
    
    // Debug visualization
    void OnDrawGizmos()
    {
        if (positionHistory.Count < 2)
        {
            return;
        }
        
        // Draw the predicted position
        Vector3 predictedPosition = PredictPlayerPosition();
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(predictedPosition, 0.5f);
        
        // Draw a line from the current position to the predicted position
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(positionHistory[positionHistory.Count - 1], predictedPosition);
    }
} 