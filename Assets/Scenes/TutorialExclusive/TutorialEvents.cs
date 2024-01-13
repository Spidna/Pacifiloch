using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor;
using GF = GlobalFunctions;


public class TutorialEvents : EventParent
{
    // Examples from parent class
    //public System.Action onStart;
    //public System.Action onDestroy;
    //public System.Action onUpdate;

    [Header("Scene References")]
    [SerializeField] private GameObject player;
    [Tooltip("Where the player's headset is looking.")]
    [SerializeField] private Transform playerHead;
    [SerializeField] private SheathsList sheaths;
    [SerializeField] private Flock enemySpawner;

    [Header("Patience")]
    [Tooltip("Whether player has hit the right button recently enough")]
    [Range(2f, 50f)][SerializeField] private float patienceInputThreshold;
    [Tooltip("How long since the player has gone without pushing right button")]
    [SerializeField] private float patienceInputProgress;
    [SerializeField] InputActionReference leftStrokeButton;
    [SerializeField] InputActionReference rightStrokeButton;
    [SerializeField] InputActionReference leftGrabButton;
    [SerializeField] InputActionReference rightGrabButton;

    [Tooltip("Is the player completing this lesson fast enough")]
    [Range(2f, 50f)][SerializeField] private float patienceLessonThreshold;
    [Tooltip("How long the player has gone without passing the lesson")]
    [SerializeField] private float patienceLessonProgress;
    [Tooltip("How far along teacher is in lecture")]
    [SerializeField] protected float lectureProgress;

    [Tooltip("Distance player has to turn (1=180), to succeed turning lesson.")]
    [Range(0.5f, 1f)][SerializeField] private float turnThreshold;
    [Tooltip("Where the player has to get to, to succeed swimming lesson.")]
    [SerializeField] private Transform swimThreshold;
    [Tooltip("How close the player is to SwimThreshold")]
    [SerializeField] private float swimProgress;
    [Tooltip("Where the Roe table is")]
    [SerializeField] private Transform roeTable;
    [Tooltip("Where the Pearls table is")]
    [SerializeField] private Transform pearlTable;
    [Tooltip("How close the player should be to a table to be told to pick up")]
    [SerializeField] private float tableThreshold;



    [Header("Lecture Notes")]
    [Tooltip("Teacher's Voicebox")]
    public AudioSource voice;
    [Tooltip("Queued Lecture while current one fades out")]
    public Queue<AudioClip> nextLecture;
    [Tooltip("How much to fade by each second to prep new-Lecture")]
    [Range(0.5f, 10f)][SerializeField] protected float interuptFade;

    [Header("Welcome and teach turning to the player")]
    public AudioClip turnLecture;
    public AudioClip turnButton;
    public AudioClip turnReminder;
    public AudioClip complimentTail;
    public AudioClip complimentHandTurn;

    [Header("Teach the player how to swim")]
    public AudioClip swimLecture;
    public AudioClip swimButton;
    public AudioClip swimReminder;
    public AudioClip complimentSwim;

    [Header("Send the player on their first fetchquest")]
    public AudioClip fetchLecture;
    public AudioClip grabLecture;
    public AudioClip grabButton;
    public AudioClip grabReminder;
    public AudioClip sheathCompliment;

    [Header("Teach combat")]
    public AudioClip announceFight1;
    public AudioClip announceFight2;

    // Start is called before the first frame update
    protected override void Start()
    {


        onUpdate += countPatience;
        onUpdate += TurnTutCheckClear;
        LectureChange(turnLecture);
    }

    /// <summary>
    /// Increment the patienceProgress
    /// </summary>
    protected void countPatience()
    {
        patienceInputProgress += Time.deltaTime;
        patienceLessonProgress += Time.deltaTime;
    }

    // Lecture System
    /// <summary>
    /// Change to a new lecture, after fading out of previous lecture to ensure
    /// audio quality
    /// </summary>
    /// <param name="toSay">New lecture</param>
    protected void LectureChange(AudioClip toSay)
    {
        // Setup buffer lectures
        nextLecture.Clear();
        nextLecture.Enqueue(toSay);
        // Reset Patience
        patienceLessonProgress = 0f;
        patienceInputProgress = 0f;

        // Set onUpdate to include EXACTLY 1 CancellingLecture
        onUpdate -= CancellingLecture;
        onUpdate += CancellingLecture;
    }
    /// <summary>
    /// Add 2 lectures to the buffer, clearing current audio
    /// </summary>
    /// <param name="a">First</param>
    /// <param name="b">Second</param>
    protected void LectureChange(AudioClip a, AudioClip b)
    {
        // Queue both lectures
        LectureChange(a);
        // LectureChange before Enqueue so a doesn't get wiped
        nextLecture.Enqueue(b);
    }
    /// <summary>
    /// ONLY USE FROM WITHIN CHANGELECTURE
    /// </summary>
    protected void CancellingLecture()
    {
        // If we've fully faded out
        if (voice.volume <= 0f)
        {
            // Stop fading
            onUpdate -= CancellingLecture;
            // Switch Lectures and reset volume
            voice.clip = nextLecture.Dequeue();
            voice.volume = 1f;
            voice.Play();

            // Prep next queued lecture if there is one
            if (nextLecture.Count > 0)
            {
                // Set exactly 1 check for queued lectures
                onUpdate -= QueuedLecture; // YOU GOT IT ALREADY STOP DUPING THIS LINE
                onUpdate += QueuedLecture;
            }

            return;
        }

        // Fade a bit this frame
        voice.volume -= interuptFade * Time.deltaTime;
    }
    protected void QueuedLecture()
    {
        // If there's audio currently playing then the next lecture isn't ready yet
        if (voice.isPlaying)
        { return; }

        // Otherwise switch to next line
        if (nextLecture.Count > 0)
        {
            LectureChange(nextLecture.Dequeue());
        }
        else
        {
            // If we're out of lectures then cancel the queue
            onUpdate -= QueuedLecture;
            return;
        }

    }

    // Turning Tutorial
    /// <summary>
    /// Check if the player has turned enough to clear the Turning Lesson
    /// </summary>
    protected void TurnTutCheckClear()
    {
        // Check if player accomplished turning in real life
        float turnProgress = Mathf.Abs(playerHead.rotation.y);
        if (turnProgress > turnThreshold)
        {
            onUpdate += SwimTutCheckClear;

            // Don't make this check anymore
            onUpdate -= TurnTutCheckClear;
            // Wipe Patience
            patienceInputProgress = 0f;
            patienceLessonProgress = 0f;
            // Give next lesson and compliment their methods
            LectureChange(complimentTail, swimLecture);

            //Bail cuz I've done my job
            return;
        }

        // Check if the player has accomplished turning with swim controls
        turnProgress = Mathf.Abs(player.transform.rotation.y + playerHead.rotation.y);
        if (turnProgress > turnThreshold)
        {
            // Transition to next lesson
            onUpdate += SwimTutCheckClear;

            // Don't make this check anymore
            onUpdate -= TurnTutCheckClear;
            // Wipe Patience
            patienceInputProgress = 0f;
            patienceLessonProgress = 0f;
            // Give next lesson and encourage them
            LectureChange(complimentHandTurn, swimLecture);

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
            // Reset patience so reminders can happen in a timely manner
            patienceLessonProgress = 0f;
            patienceInputProgress = 0f;
            // Remind the player how to move
            LectureChange(turnReminder);

            return;
        }

        // Check if teacher should encourage using the right button
        if (patienceInputProgress > patienceInputThreshold)
        {
            // Update Patience so reminders can happen in a timely manner
            patienceInputProgress = 0f;
            patienceLessonProgress -= 6f; // So Button prompt doesn't get interrupted by talking
            // Remind the player which button to push
            LectureChange(turnButton);

            return;

        }
    }

    protected void SwimTutCheckClear()
    {
        // Check if player accomplished Swimming
        Vector3 swimVec = swimThreshold.position - player.transform.position;
        swimVec = GF.VecTimes(swimVec, swimThreshold.localScale);
        swimProgress = swimVec.x + swimVec.y + swimVec.z;
        if (swimProgress < 0f)
        {
            // Change state to fetchquest
            onUpdate -= SwimTutCheckClear;
            onUpdate -= SwimTutCheckClear; // Double up cuz I'm scared

            onUpdate += FetchCheckReached;
            LectureChange(complimentSwim, fetchLecture);

            //Bail cuz I've done my job
            return;
        }

        // Check if patience for button input should be reset
        if (leftStrokeButton.action.IsPressed()
            && rightStrokeButton.action.IsPressed())
        {
            patienceInputProgress = 0f;
        }
        // Check if teacher should encourage better swimming
        if (patienceLessonProgress > patienceLessonThreshold)
        {
            // Reset patience so reminders can happen in a timely manner
            patienceLessonProgress = 0f;
            patienceInputProgress = 0f;
            // Encourage better swimming
            LectureChange(swimReminder);

            return;
        }

        // Check if teacher should encourage using the right button
        if (patienceInputProgress > patienceInputThreshold)
        {
            // Update Patience so reminders can happen in a timely manner
            patienceInputProgress = 0f;
            patienceLessonProgress -= 6f; // So Button prompt doesn't get interrupted by talking

            //Encourage use of buttons
            LectureChange(swimButton);

            return;
        }
    }

    /// <summary>
    /// Check if the player has reached the fetch areas
    /// </summary>
    protected void FetchCheckReached()
    {
        // Find how close the player is to the closest table
        float playerToTable = (player.transform.position - roeTable.position).sqrMagnitude;
        float playerToSecondTable = (player.transform.position - pearlTable.position).sqrMagnitude;
        if (playerToTable > playerToSecondTable)
            playerToTable = playerToSecondTable;

        // If player is close enough, teach them to grab things
        if (playerToTable < tableThreshold)
        {
            LectureChange(grabLecture);
            onUpdate -= FetchCheckReached;
            onUpdate += GrabCheckClear;

            return;
        }
    }
    /// <summary>
    /// Teach the player to grab things
    /// </summary>
    protected void GrabCheckClear()
    {
        // If the player has successfully sheathed something, call it a success
        foreach (Sheath i in sheaths.Slot)
        {
            if (i.myContent != null)
            {
                LectureChange(sheathCompliment);
                onUpdate -= GrabCheckClear;
                onUpdate += CollectCheckClear;

                return;
            }
        }


        // Check if patience for button input should be reset
        if (leftGrabButton.action.IsPressed()
            || rightGrabButton.action.IsPressed())
        {
            patienceInputProgress = 0f;
        }
        // Check if teacher should remind how to collect things
        if (patienceLessonProgress > patienceLessonThreshold)
        {
            // Reset patience so reminders can happen in a timely manner
            patienceLessonProgress = 0f;
            patienceInputProgress = 0f;
            // Grant reminder
            LectureChange(grabReminder);

            return;
        }

        // Check if teacher should encourage using the right button
        if (patienceInputProgress > patienceInputThreshold)
        {
            // Update Patience so reminders can happen in a timely manner
            patienceInputProgress = 0f;
            patienceLessonProgress -= 6f; // So Button prompt doesn't get interrupted by talking

            //Encourage use of buttons
            LectureChange(grabButton);

            return;
        }
    }

    /// <summary>
    /// Check if player has sheathed everything for fetchquest
    /// </summary>
    protected void CollectCheckClear()
    {
        // If the player has sheathed everything, call it a success
        int sheathTotal = 0;
        foreach (Sheath i in sheaths.Slot)
        {
            if (i.myContent != null)
            {
                sheathTotal++;
            }
        }
        if (sheathTotal >= 4)
        {
            LectureChange(announceFight1);
            onUpdate -= CollectCheckClear;
            onUpdate += fight1CheckClear;

            // Start the first fight
            enemySpawner.spawnCount = 1;
            enemySpawner.GenerateUnits();
            enemySpawner.spawnCount = 10;
            return;
        }


        /// Check if patience for button input should be reset
        ///if (leftGrabButton.action.IsPressed()
        ///    || rightGrabButton.action.IsPressed())
        ///{
        ///    patienceInputProgress = 0f;
        ///}
        /// Check if teacher should remind how to collect things
        ///if (patienceLessonProgress > patienceLessonThreshold)
        ///{
        ///    // Reset patience so reminders can happen in a timely manner
        ///    patienceLessonProgress = 0f;
        ///    patienceInputProgress = 0f;
        ///    // Grant reminder
        ///    LectureChange(grabReminder);
        ///    return;
        ///}
        /// Check if teacher should encourage using the right button
        ///if (patienceInputProgress > patienceInputThreshold)
        ///{
        ///    // Update Patience so reminders can happen in a timely manner
        ///    patienceInputProgress = 0f;
        ///    patienceLessonProgress -= 6f; // So Button prompt doesn't get interrupted by talking
        ///    //Encourage use of buttons
        ///    LectureChange(grabButton);
        ///    return;
        ///}
    }

    /// <summary>
    /// Check if all the enemies are dead
    /// </summary>
    protected void fight1CheckClear()
    {
        if (enemySpawner.allAgents.Count <= 0)
        {
            onUpdate -= fight1CheckClear;
            onUpdate -= fight2CheckClear;
            onUpdate += fight2CheckClear;
            LectureChange(announceFight2);

            // TODO remove once no longer Demo Build
            // This is just to make the Demo theoreticaly infinite
            enemySpawner.spawnCount *= 15 / 10;
        }
    }
    /// <summary>
    /// Identical to fight1CheckClear for the demo
    /// </summary>
    protected void fight2CheckClear()
    {
        fight1CheckClear();
    }

}
