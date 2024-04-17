using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System.Linq;

public class QuizSystem : MonoBehaviour
{
    UIDocument quizDoc;
    Button[] ansButton;
    Label quizQuestion;
    Player playerScript;
    ORC orcScript;
    public int currentQuestionIndex, pointsToAdd;
    string url = "https://opentdb.com/api.php?amount=6&category=15&difficulty=easy&type=multiple";
    Root responseData;
    FireBaseData fireScript;
    float startTime;
    bool playAnim;
    //public bool rightTime;
    ProgressBar timer;
    float TimeLeft;
    public float maxTime;
    //public bool fetch;

    private void Awake()
    {
        quizDoc = GetComponent<UIDocument>();
        quizQuestion = quizDoc.rootVisualElement.Q("Question") as Label;

        ansButton = new Button[4];


        playerScript = GameObject.Find("player").GetComponent<Player>();
        orcScript = GameObject.Find("orc").GetComponent<ORC>();

        ansButton[0] = quizDoc.rootVisualElement.Q("Option1") as Button;
        ansButton[1] = quizDoc.rootVisualElement.Q("Option2") as Button;
        ansButton[2] = quizDoc.rootVisualElement.Q("Option3") as Button;
        ansButton[3] = quizDoc.rootVisualElement.Q("Option4") as Button;
        currentQuestionIndex = 0;
        fireScript = GameObject.Find("DataBaseManager").GetComponent<FireBaseData>();
        timer = quizDoc.rootVisualElement.Q("Timer") as ProgressBar;
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Quiz
    {
        public string type { get; set; }
        public string difficulty { get; set; }
        public string category { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> incorrect_answers { get; set; }
    }

    public class Root
    {
        public int response_code { get; set; }
        public List<Quiz> results { get; set; }
    }



    public void Start()
    {
        currentQuestionIndex = 0;
        playAnim = false;
        //if(rightTime)
        StartCoroutine(QuizDataFetcher(url));
        TimeLeft = 20f;
        maxTime = 20f;
    }

    
    

    public IEnumerator QuizDataFetcher(string uri)
    {
        using (UnityWebRequest webrequest = UnityWebRequest.Get(uri))
        {
            yield return webrequest.SendWebRequest();

            switch (webrequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(string.Format("Something Went Wrong : {0}", webrequest.error));
                    break;

                case UnityWebRequest.Result.Success:

                    responseData = JsonConvert.DeserializeObject<Root>(webrequest.downloadHandler.text);
                    QuestionLoader(responseData);
                    break;
            }
        }
    }

    void QuestionLoader(Root responseData)
    {

        if (currentQuestionIndex < 6)
        {
            Debug.Log("index Used : " + currentQuestionIndex);
            Quiz questionClass = responseData.results[currentQuestionIndex];
            Debug.Log(questionClass.question);
            quizQuestion.text = questionClass.question;
            AnswerManager(questionClass);
            currentQuestionIndex++;
            Debug.Log("question : " + currentQuestionIndex);
            startTime = Time.time;
        }
        else
        {
            Debug.Log("Quiz Completed");
            playerScript.onSubmit();
            playerScript.hideUIquiz();
            playerScript.unhideUIuser();
            fireScript.LoadData();
        }
            

    }

    IEnumerator WaitAndFetchQuiz()
    {
        yield return new WaitForSeconds(2f);
        //StartCoroutine(QuizDataFetcher(url));
        QuestionLoader(responseData);
        TimeLeft = 20f;
    }

    void AnswerManager(Quiz questionClass)
    {
        if (currentQuestionIndex < responseData.results.Count)
        {
            Debug.Log(questionClass.correct_answer);
            List<string> allAnswers = new List<string>();
            allAnswers.Add(questionClass.correct_answer);
            allAnswers.AddRange(questionClass.incorrect_answers);

            allAnswers = allAnswers.OrderBy(x => Random.value).ToList();



            for (int i = 0; i < allAnswers.Count; i++)
            {
                ansButton[i].text = allAnswers[i];

                if (ansButton[i].text == questionClass.correct_answer)
                {
                    ansButton[i].RegisterCallback<ClickEvent>((evt) => OnClickCorrect());
                }

                else
                {
                    ansButton[i].RegisterCallback<ClickEvent>((evt) => OnClick());
                }
            }



        }

        else
        {
            Debug.Log("QuizEnded");
        }

    }

    private void Update()
    {
        //Debug.Log("time left : " + TimeLeft);
        if(playAnim)
        {
            if (playerScript.Streak == 0)
            {
                playerScript.anim.SetTrigger("lightAttack");
            }

            if (playerScript.Streak >= 1 && playerScript.Streak < 3)
            {
                playerScript.anim.SetTrigger("groundAttack");
            }

            else if (playerScript.Streak >= 3)
            {
                playerScript.anim.SetTrigger("heavyAttack");
            }
            playAnim = false;
        }

        if(TimeLeft>0)
        {
            TimeLeft -= Time.deltaTime;
            UpdateTimerBar();
            
        }
        else
        {
            Debug.Log("Time's Over");
            WaitAndFetchQuiz();
        }

    }

    void OnClick()
    {
        Debug.Log("Incorrect Answer Clicked");
        playerScript.destroyStreak = true;
        StartCoroutine(WaitAndFetchQuiz());
    }

    void UpdateTimerBar()
    {
        float fillAmt = TimeLeft / maxTime;
        timer.value = fillAmt;
    }

    void OnClickCorrect()
    {
        float timeTaken = Time.time - startTime;

        if (timeTaken <= 5f)
        {
            pointsToAdd = 20;
        }
        else if (timeTaken <= 10f)
        {
            pointsToAdd = 15;
        }
        else if (timeTaken <= 15f)
        {
            pointsToAdd = 10;
        }
        else
        {
            pointsToAdd = 5;
        }
        
        playerScript.updateStreak = true;
        Debug.Log("Correct ans Clicked");
        playAnim = true;
        StartCoroutine(WaitAndFetchQuiz());
    }


}