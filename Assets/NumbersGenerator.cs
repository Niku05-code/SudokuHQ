using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumbersGenerator : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Button[] numbers = new Button[9];
    public Text mistakesText;
    public Color clickedColor;
    public Color wrongColor = Color.red;
    public GameObject buttonEasy;
    public GameObject buttonMedium;
    public GameObject buttonHard;
    public Text hintText;
    public GameObject hintButton;
    

    public void generateNumbersButton()
    {
        int locationNumberX = 555;
        int locationNumberY = -150;
        for (int i = 0; i < 9; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, transform);
            newButton.name = $"Button{i + 1}";
            newButton.GetComponentInChildren<Text>().text = (i + 1).ToString();
            RectTransform rec = newButton.GetComponent<RectTransform>();
            rec.anchoredPosition = new Vector2(locationNumberX, locationNumberY);
            locationNumberX += 100;

            Button buttonComponent = newButton.GetComponent<Button>();
            numbers[i] = buttonComponent;
            int capturedI = i + 1;
            buttonComponent.onClick.AddListener(() => OnButtonClick(capturedI));
        }
    }

    private void OnButtonClick(int i)
    {
        Debug.Log($"Button clicked at [{i}]");
        int row = GameSense.selectedRow;
        int col = GameSense.selectedCol;

        if (GameSense.showBoard[row, col] == 0)
        {
            if(GameSense.board[row, col] == i)
            {
                GameSense.appearance[i]++;
                GameSense.showBoard[row, col] = i;

                string buttonName = "Button" + row.ToString() + col.ToString();
                GameObject obiect = GameObject.Find(buttonName);
                obiect.GetComponentInChildren<Text>().text = GameSense.showBoard[row, col].ToString();
            }
            else
            {
                GameSense.mistakes++;
                mistakesText.text = "Mistakes: " + GameSense.mistakes.ToString();
                string buttonName = "Button" + row.ToString() + col.ToString();
                GameObject obiect = GameObject.Find(buttonName);
                var image = obiect.GetComponent<Image>();
                image.color = wrongColor;
            }
            for (int k = 0; k < 9; k++)
                for (int l = 0; l < 9; l++)
                {
                    if (GameSense.showBoard[k, l] == i)
                    {
                        string buttonName2 = "Button" + k.ToString() + l.ToString();
                        GameObject obiect2 = GameObject.Find(buttonName2);
                        var image2 = obiect2.GetComponent<Image>();
                        image2.color = clickedColor;
                    }
                }
        }
        if(GameSense.appearance[i] == 9)
        {
            string buttonName = "Button" + (i).ToString();
            GameObject obiect = GameObject.Find(buttonName);
            Destroy(obiect);
        }
        if(finishGame())
        {
            destroyObj();
            RectTransform rt = buttonEasy.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(-300, 0);
            RectTransform rt2 = buttonMedium.GetComponent<RectTransform>();
            rt2.anchoredPosition = new Vector2(0, 0);
            RectTransform rt3 = buttonHard.GetComponent<RectTransform>();
            rt3.anchoredPosition = new Vector2(300, 0);

        }

    }
    private bool finishGame()
    {
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                if (GameSense.showBoard[i, j] != GameSense.board[i, j])
                    return false;
        return true;
    }
    private void destroyObj()
    {
        for (int i = 0; i < 9; ++i)
        {
            string lineName1 = "VerticalLine_" + i.ToString();
            GameObject obiect1 = GameObject.Find(lineName1);
            Destroy(obiect1);
            string lineName2 = "HorizontalLine_" + i.ToString();
            GameObject obiect2 = GameObject.Find(lineName2);
            Destroy(obiect2);
            for (int j = 0; j < 9; ++j)
            {
                string buttonName = "Button" + i.ToString() + j.ToString();
                GameObject obiect = GameObject.Find(buttonName);
                Destroy(obiect);
            }
        }
        for(int i = 0; i < 10; ++i)
            GameSense.appearance[i] = 0;
        GameSense.mistakes = 0;
        RectTransform rt = mistakesText.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(10000, 0);
        RectTransform rt2 = hintText.GetComponent<RectTransform>();
        rt2.anchoredPosition = new Vector2(10000, 0);
        RectTransform rt3 = hintButton.GetComponent<RectTransform>();
        rt3.anchoredPosition = new Vector2(10000, 0);
    }


}
