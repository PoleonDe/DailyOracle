using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Sirenix.OdinInspector;
using System.IO;

public class PopulateList : SerializedMonoBehaviour
{
    public TaskList taskList = new TaskList();
    [Required] public VisualTreeAsset taskAsset;
    [Required] public string jsonFilePath = "/Resources/";
    [Required] public string fileName = "Tasks";

    void OnEnable()
    {
        LoadTasksFromJson();
        SetupList();
    }


    private void SetupList()
    {
        VisualElement root = this.gameObject.GetComponent<UIDocument>().rootVisualElement;   
        
        ListView listView = root.Q<ListView>();
        listView.fixedItemHeight = 68f;
        listView.itemsSource = taskList.tasks;
        listView.makeItem = MakeItem;
        listView.bindItem = BindItem;
    }

    private VisualElement MakeItem()
    {
        return taskAsset.CloneTree();
    }
    private void BindItem(VisualElement vE, int index)
    {
        Label description = vE.Q("Description") as Label;
        description.text = taskList.tasks[index].task;
        Toggle taskStatus = vE.Q("TaskStatus") as Toggle;
        taskStatus.value = taskList.tasks[index].status;
        VisualElement backdrop = vE.Q("Task");
        backdrop.style.backgroundImage = taskList.tasks[index].StyleBackground;
    }

    [Button]
    public void SaveTasksToJson()
    {
        string json = JsonUtility.ToJson(taskList, true);
        File.WriteAllText(Application.dataPath + jsonFilePath + fileName + ".json", json);
    }

    [Button]
    public void LoadTasksFromJson()
    {
       
        TextAsset textFile = Resources.Load<TextAsset>(fileName);
        string json = textFile.text;
        TaskList data = JsonUtility.FromJson<TaskList>(json);

        taskList.tasks = data.tasks;
    }
}

[System.Serializable]
public class Task
{
    public string task = "";
    public int difficulty = 3;
    public bool status = false;
    public string imageName = null;
    public StyleBackground StyleBackground { get => new StyleBackground(Resources.Load<Texture2D>("Images/" + imageName)); }
    public Task(){}
    public Task(string task, int difficulty, bool status)
    {
        this.task = task;
        this.difficulty = difficulty;
        this.status = status;
    }
}
[System.Serializable]
public class TaskList
{
    public Task[] tasks;
}