using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.PlayerLoop;

public class DebugLogToCanvasTMP : MonoBehaviour
{
    public static DebugLogToCanvasTMP Instance;
    public TextMeshProUGUI logText; // Reference to the TMP Text component
    public Button clearButton; // Reference to the Clear Log button
    public string[] allowedMessages; // Array of allowed log messages


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keep across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        logText.text = ""; // Clear text at the start
        //clearButton.onClick.AddListener(ClearLog); // Attach the ClearLog method to the button


        if (logText == null)
        {
            Debug.LogError("logText is not assigned in the Inspector.");
        }
        else
        {
            Debug.Log("Log Initialized"); // Test message
        }
    }
    public void Update()
    {
    }

    public void UpdateLog(string message)
    {
        string timestampedMessage = message + "\n"; // Format the message
        logText.text += timestampedMessage; // Display the message

        StartCoroutine(ClearSpecificMessageAfterDelay(timestampedMessage, 5f)); // Clear it after 5 seconds
    }

    // Coroutine to clear a specific message after a delay
    private IEnumerator ClearSpecificMessageAfterDelay(string message, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Remove the specific message from the log
        logText.text = logText.text.Replace(message, "");
    }

    public IEnumerator Clear(float duration = 5.0f)
    {
        yield return new WaitForSeconds(duration);
        logText.text = "";
    }

    // Method to clear the log text
    public void ClearLog()
    {
        logText.text = "";
    }

    // Optional: Capture all Unity Debug.Log messages
    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wand")){
            ClearLog();
        }
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Only update the log if the message contains "NullReferenceException"
        /*if (!logString.Contains("NullReferenceException"))
        {
            
        }*/
        UpdateLog(logString); // Update log with the NullReferenceException message
    }
}