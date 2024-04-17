using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;
using UnityEngine.UIElements;
using System.Linq;





/*[Serializable]

    public class saveData
{
    public string user_name;
    public string user_email;
    public int points;
}*/

public class Database : MonoBehaviour
{
    /*UIDocument quizDoc;
    Button saveButton, loadButton;
    public saveData svDT;
    public string userId;

    DatabaseReference dbRef;

    private void Awake()
    {
        quizDoc = GameObject.Find("QuizQuestion").GetComponent<UIDocument>();
        saveButton = quizDoc.rootVisualElement.Q("saveButton") as Button;
        loadButton = quizDoc.rootVisualElement.Q("loadButton") as Button;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Update()
    {
        saveButton.RegisterCallback<ClickEvent>((saveevt) => SaveData());
        loadButton.RegisterCallback<ClickEvent>((loadevt) => LoadData());
    }

    void SaveData()
    {
        string json = JsonUtility.ToJson(svDT);
        dbRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    

    void LoadData() 
    {

    }*/
}
