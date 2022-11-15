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

    void OnEnable()
    {
        LoadTasksFromJson();
        SetupList();
    }


    private void SetupList()
    {
        VisualElement root = this.gameObject.GetComponent<UIDocument>().rootVisualElement;   
        
        ListView listView = root.Q<ListView>("DashboardCards");
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
    }

    [Button]
    public void SaveTasksToJson()
    {
        string json = JsonUtility.ToJson(taskList, true);
        File.WriteAllText(Application.dataPath + "/Resources/Tasks.json", json);
    }

    [Button]
    public void LoadTasksFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/Tasks.json");
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