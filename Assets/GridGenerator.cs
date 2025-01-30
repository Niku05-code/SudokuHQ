using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    public GameObject buttonEasy;
    public GameObject buttonMedium;
    public GameObject buttonHard;
    public GameObject buttonPrefab;
    private Button[,] buttonGrid = new Button[9, 9];
    public Color normalColor, clickedColor, rowCol;

    public void GenerateGrid()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                GameObject newButton = Instantiate(buttonPrefab, transform);

                newButton.name = $"Button{row}{col}";
                newButton.GetComponentInChildren<Text>().text = "";
                Button buttonComponent = newButton.GetComponent<Button>();
                buttonGrid[row, col] = buttonComponent;

                int capturedRow = row;
                int capturedCol = col;
                buttonComponent.onClick.AddListener(() => OnButtonClick(capturedRow, capturedCol));
            }
        }
        RectTransform rt = buttonEasy.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(10000, 0);
        RectTransform rt2 = buttonMedium.GetComponent<RectTransform>();
        rt2.anchoredPosition = new Vector2(10000, 0);
        RectTransform rt3 = buttonHard.GetComponent<RectTransform>();
        rt3.anchoredPosition = new Vector2(10000, 0);

    }

    private void OnButtonClick(int row, int col)
    {
        Debug.Log($"Button clicked at [{row}, {col}]");
        ResetAllButtonColors();
        GameSense.selectedRow = row;
        GameSense.selectedCol = col;

        if(GameSense.showBoard[row, col] != 0)
        {
            for (int k = 0; k < 9; k++)
                for (int l = 0; l < 9; l++)
                {
                    if (GameSense.showBoard[k, l] == GameSense.showBoard[row, col])
                    {
                        string buttonName2 = "Button" + k.ToString() + l.ToString();
                        GameObject obiect2 = GameObject.Find(buttonName2);
                        var image = obiect2.GetComponent<Image>();
                        image.color = clickedColor;
                    }
                }
        }
        else
        {
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 9; ++j)
                    if (i == row || j == col)
                    {
                        string buttonName1 = "Button" + i.ToString() + j.ToString();
                        GameObject obiect1 = GameObject.Find(buttonName1);
                        var image1 = obiect1.GetComponent<Image>();
                        image1.color = rowCol;
                    }
            string buttonName = "Button" + row.ToString() + col.ToString();
            GameObject obiect = GameObject.Find(buttonName);
            var image = obiect.GetComponent<Image>();
            image.color = clickedColor;
        }
    }

    private void ResetAllButtonColors()
    {
        for (int row = 0; row < 9; ++row)
            for (int col = 0; col < 9; ++col)
            {
                string buttonName = "Button" + row.ToString() + col.ToString();
                GameObject obiect = GameObject.Find(buttonName);
                var image = obiect.GetComponent<Image>();
                image.color = normalColor;

            }
    }
}

