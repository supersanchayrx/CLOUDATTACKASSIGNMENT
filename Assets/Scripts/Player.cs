using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
public class Player : MonoBehaviour
{

    Rigidbody rb;
    public Animator anim;
    public bool updateStreak,destroyStreak;
    public int Streak;
    public static int points;
    public static string username, useremail;
    UIDocument quizDoc, userdata;
    public TextField playerName, playermail;
    Button saveButton, loadButton, quitButton;
    FireBaseData firebase;
    public Label playerPoints;
    Camera cam;
    public bool camUp, canRotate, movecamera;
    Quaternion startAngle, targetAngle;
    [SerializeField] float rotSpeed;
    QuizSystem quizScript;
    bool onceDone;
    GameObject quizSYSTEM;
    public int pointsToSHow;

    private void Awake()
    {
        userdata = GameObject.Find("userDATA").GetComponent<UIDocument>();
        quizDoc = GameObject.Find("QuizQuestion").GetComponent<UIDocument>();
        playerName = userdata.rootVisualElement.Q("username") as TextField;
        playermail = userdata.rootVisualElement.Q("usermail") as TextField;
        saveButton = userdata.rootVisualElement.Q("saveButton") as Button;
        quitButton = userdata.rootVisualElement.Q("quit") as Button;
        loadButton = userdata.rootVisualElement.Q("loadButton") as Button;
        playerPoints = userdata.rootVisualElement.Q("points") as Label;
        firebase = GameObject.Find("DataBaseManager").GetComponent<FireBaseData>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        quizScript = GameObject.Find("QuizQuestion").GetComponent<QuizSystem>();
        //quizSYSTEM = GameObject.Find("QuizQuestion");

        

    }
    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        updateStreak = false; destroyStreak = false;
        Streak = 0;
        hideUIquiz();
        camUp = true;
        startAngle = Quaternion.Euler(-55f,0f,0f);
        targetAngle = Quaternion.Euler(0f, 0f, 0f);
        onceDone = false;
        //quizSYSTEM.SetActive(false);
        loadButton.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        pointsToSHow = points;
        {
            saveButton.RegisterCallback<ClickEvent>((saveevt) => onSubmit());
            loadButton.RegisterCallback<ClickEvent>((loadevt) => onLoad());
            quitButton.RegisterCallback<ClickEvent>((quitevt) => onQuit());
            //playerPoints.text = points.ToString();
        }

        if(updateStreak)
        {
            UpdateStreak();
        }
        else if(destroyStreak) 
        {
            DestroyStreak();
        }

        if(movecamera)
        {
            moveCam();
        }

        //quizScript.rightTime = camUp ? false : true;
        
    }
    void UpdateStreak()
    {
        points += quizScript.pointsToAdd;
        Streak++;
        updateStreak = false;
        //firebase.OnPostData();

    }

    void DestroyStreak()
    {
        points += 0;
        Streak = 0;
        destroyStreak = false;
        //firebase.OnPostData();

    }

    public void onSubmit()
    {
        //quizSYSTEM.SetActive(true);

        if (onceDone)
        {   
            quizScript.Start();
            Debug.Log("button clicked");
            username = playerName.text;
            useremail = playermail.text;
            firebase.OnPostData();
            hideUIuser();
            unhideUIquiz();
            onceDone = false;

        }     
        else
        {
            Debug.Log("button clicked");
            username = playerName.text;
            useremail = playermail.text;
            firebase.OnPostData();
            hideUIuser();
            unhideUIquiz();
        }
        
        
    }

    public void onLoad()
    {
        Debug.Log("button clicked");
        username = playerName.text;
        useremail = playermail.text;
        firebase.LoadData();
    }


    public void hideUIuser()
    {
        userdata.rootVisualElement.Q<VisualElement>("container").style.visibility = Visibility.Hidden;
        movecamera = true;
        loadButton.visible = false;

        //unhideUIquiz();
    }
    public void unhideUIuser()
    {
        userdata.rootVisualElement.Q<VisualElement>("container").style.visibility = Visibility.Visible;
        movecamera = true;
        loadButton.visible = true;

        //hideUIquiz();
    }

    public void unhideUIquiz()
    {
        quizDoc.rootVisualElement.Q<VisualElement>("Container").style.visibility = Visibility.Visible;
        //hideUIuser();
    }

    public void hideUIquiz()
    {
        quizDoc.rootVisualElement.Q<VisualElement>("Container").style.visibility = Visibility.Hidden;
        //unhideUIuser();
    }

    /* void moveCam()
     {
         if(Mathf.Approximately(Quaternion.Angle(cam.transform.rotation, targetAngle), 0.1f))
         {
             canRotate = false;
             movecamera = false;
         }
         else 
         {
             canRotate= true;
         }
         if(canRotate) 
         {
             if (camUp)
             {
                 cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetAngle, rotSpeed);
             }
             else
             {
                 cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, startAngle, rotSpeed);
             }
         }
         cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetAngle, rotSpeed);

     }*/

    void moveCam()
    {
        
        bool isCloseToTarget = Mathf.Approximately(Quaternion.Angle(cam.transform.rotation, camUp ? targetAngle : startAngle), 0.1f);

       
        if (isCloseToTarget)
        {
            canRotate = false;
            movecamera = false;
        }
        else
        {
            canRotate = true;

            
            Quaternion targetRotation = camUp ? targetAngle : startAngle;

            
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

          
            float angleDifference = Quaternion.Angle(cam.transform.rotation, targetRotation);
            if (angleDifference < 0.01f)
            {
                
                camUp = !camUp;
                canRotate = false;
                movecamera = false;
            }
        }
    }


    public void showRESULTS()
    {
        //camUp = true;
        onceDone = true;
        playerName.value = username;
        playermail.value = useremail;
        playerPoints.text = pointsToSHow.ToString();      
    }

    void onQuit()
    {
        Application.Quit();
    }
}
