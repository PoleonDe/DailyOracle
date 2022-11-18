using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Sirenix.OdinInspector;

public class ScreenChange : MonoBehaviour
{
    [Required] public GameObject[] screens;

    void OnEnable()
    {
        BindButtons();
    }


    private void BindButtons()
    {
        VisualElement root = this.gameObject.GetComponent<UIDocument>().rootVisualElement;

        Button dashboardButton = root.Q<Button>("DashboardButton");
        Button cardsButton = root.Q<Button>("CardsButton");
        Button settingsButton = root.Q<Button>("SettingsButton");

        dashboardButton.clicked += () => ChangeScreen(0);
        cardsButton.clicked += () => ChangeScreen(1);
        settingsButton.clicked += () => ChangeScreen(2);
    }

    private void ChangeScreen(int index)
    {
        if (index > screens.Length)
        {
            Debug.Log("Tried Switching to an Out of Bounds Index");
            return;
        }

        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive((i == index));
        }
    }
}
