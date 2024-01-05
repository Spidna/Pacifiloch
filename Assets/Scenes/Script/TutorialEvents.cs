using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor;

public class TutorialEvents : EventParent
{
    // Examples from parent class
    //public System.Action onStart;
    //public System.Action onDestroy;
    //public System.Action onUpdate;

    [SerializeField] private GameObject player;
    [Tooltip("Where the player's headset is looking.")]
    [SerializeField] private Transform playerHead;

    [Tooltip("Whether player has hit the right button recently enough")]
    [Range(2f, 50f)][SerializeField] private float patienceInputThreshold;
    [Tooltip("How long since the player has gone without pushing right button")]
    [SerializeField] private float patienceInputProgress;
    [SerializeField] InputActionReference leftStrokeButton;
    [SerializeField] InputActionReference rightStrokeButton;
    [Tooltip("Is the player completing this lesson fast enough")]
    [Range(2f, 50f)][SerializeField] private float patienceLessonThreshold;
    [Tooltip("How long the player has gone without passing the lesson")]
    [SerializeField] private float patienceLessonProgress;


    [Tooltip("Degrees the player has to turn, to succeed turning lesson.")]
    [Range(90f, 180f)][SerializeField] private float turnThreshold;
    [Tooltip("Where the player has to get to, to succeed swimming lesson.")]
    [SerializeField] private Transform swimThreshold;
    [Tooltip("How close the player is to SwimThreshold")]
    [SerializeField] private float swimProgress;


    // Start is called before the first frame update
    protected override void Start()
    {
        onUpdate += countPatience;
        onUpdate += TurnTutClearCheck;
    }

    //protected void findFields()
    //{
    //
    //}
    /// <summary>
    /// Check if the player has turned enough to clear the Turning Lesson
    /// </summary>
    protected void TurnTutClearCheck()
    {
        // Check if player accomplished turning in real life
        float turnProgress = Mathf.Abs(playerHead.rotation.y);
        if (turnProgress > turnThreshold)
        {
            TurnTutClearCheck();
            // Compliment tail use
            // Give next lesson

            //Bail cuz I've done my job
            return;
        }

        // Check if the player has accomplished turning with swim controls
        turnProgress = Mathf.Abs(player.transform.rotation.y + playerHead.rotation.y);
        if (turnProgress > turnThreshold)
        {
            TurnTutCleared();
            // Give next lesson
            //TODO ADD NEXT EVENT

            //Bail cuz I've done my job
            return;
        }

        // Check if patience for button input should be reset
        if (leftStrokeButton.action.IsPressed()
            || rightStrokeButton.action.IsPressed())
        {
            patienceInputProgress = 0f;
        }
        // Check if teacher should encourage better turning
        if (patienceLessonProgress > patienceLessonThreshold)
        {
            // Reset patience incase we need to remind again
            patienceLessonProgress = 0f;

            // Encourage better turning TODO

            return;
        }

        // Check if teacher should encourage using the right button
        if (patienceInputProgress > patienceInputThreshold)
        {
            patienceInputProgress = 0f;

            // TODO ENCOURAGE BUTTON
            //BAOTN

            return;

        }
    }
    protected void TurnTutCleared()
    {
        // Don't make this check anymore
        onUpdate -= TurnTutClearCheck;

        patienceInputProgress = 0f;
    }
    /// <summary>
    /// Increment the patienceProgress
    /// </summary>
    protected void countPatience()
    {
        patienceInputProgress += Time.deltaTime;
        patienceLessonProgress += Time.deltaTime;
    }


    // Called at a fixed interval in ballpark of framerate
    //protected override void FixedUpdate()
    //{

    //}

    //protected override void OnDestroy()
    //{
    //    base.Destroy();
    //}
}
