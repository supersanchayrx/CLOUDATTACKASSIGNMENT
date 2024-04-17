using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Models;
using UnityEngine.UIElements;
using System.Linq;
using Firebase.Database;

public class FireBaseData : MonoBehaviour
{
    UIDocument quizDoc;
    //Button saveButton, loadButton;
    //public saveData svDT;
    Player player;

    string databaseURL = "https://testgame1-c6436-default-rtdb.firebaseio.com/";
    USER user = new USER();

    private void Awake()
    {
        player = GameObject.Find("player").GetComponent<Player>();
        quizDoc = GameObject.Find("QuizQuestion").GetComponent<UIDocument>();
        /*saveButton = quizDoc.rootVisualElement.Q("saveButton") as Button;
        loadButton = quizDoc.rootVisualElement.Q("loadButton") as Button;*/
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        /*saveButton.RegisterCallback<ClickEvent>((saveevt) => OnPostData());
        loadButton.RegisterCallback<ClickEvent>((loadevt) => LoadData());*/
    }

    void getScore()
    {
        player.points = user.points;
        player.username = user.user_name;
        player.useremail = user.user_email;
        player.showRESULTS();
    }

    public void OnPostData()
    { 
        USER user = new USER();
        RestClient.Post(databaseURL + player.username+".json", user);
        Debug.Log("user Email : "+ user.user_email + "username :  " + user.user_name);
    }

    public void LoadData()
    {
        RestClient.Get<USER>(databaseURL + player.username + ".json").Then(onResolved: response =>
        {
            user = response;
            getScore();
        });
    }
}
