using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSense : MonoBehaviour
{
    public static int[,] board = new int[9, 9];
    public static int[,] showBoard = new int[9, 9];
    public static int selectedRow;
    public static int selectedCol;
    public static int mistakes;
    public static int[] appearance = new int[10];

    public int difficulty;
    public int hintNumber;
    public Button hintButton;
    public Text hintRemain;
    public Text mistakesText;
    public Color clickedColor = Color.gray;
    public Color normalColor = Color.white;
    public Color hintColor = Color.green;
    public GameObject buttonEasy;
    public GameObject buttonMedium;
    public GameObject buttonHard;
    public Text hintText;

    public static void GenerateSudokuBoard()
    {
        FillBoard(0, 0);
    }

    private static bool FillBoard(int row, int col)
    {
        if (row == 9)
            return true;

        int nextRow = col == 8 ? row + 1 : row;
        int nextCol = (col + 1) % 9;

        var numbers = GetShuffledNumbers();

        foreach (int num in numbers)
        {
            if (IsValidPlacement(row, col, num))
            {
                board[row, col] = num;

                if (FillBoard(nextRow, nextCol))
                    return true;

                board[row, col] = 0;
            }
        }

        return false;
    }

    private static int[] GetShuffledNumbers()
    {
        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        System.Random rand = new System.Random();

        for (int i = 0; i < numbers.Length; i++)
        {
            int randomIndex = rand.Next(i, numbers.Length);
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }

        return numbers;
    }

    private static bool IsValidPlacement(int row, int col, int num)
    {
        for (int i = 0; i < 9; i++)
            if (board[row, i] == num || board[i, col] == num)
                return false;

        int startRow = (row / 3) * 3;
        int startCol = (col / 3) * 3;

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[startRow + i, startCol + j] == num)
                    return false;
        return true;
    }

    private static void CreatePuzzle(int emptyCellsCount)
    {
        System.Random rand = new System.Random();
        int cellsRemoved = 0;

        while (cellsRemoved < emptyCellsCount)
        {
            int row = rand.Next(0, 9);
            int col = rand.Next(0, 9);

            if (showBoard[row, col] != 0)
            {
                showBoard[row, col] = 0;
                cellsRemoved++;
            }
        }
    }

    public void difficultyEasy()
    {
        difficulty = 40;
        hintNumber = 6;
    }
    public void difficultyMedium()
    {
        difficulty = 50;
        hintNumber = 4;
    }
    public void difficultyHard()
    {
        difficulty = 60;
        hintNumber = 2;
    }


    public void ShowNumbers()
    {
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                showBoard[i, j] = board[i, j];

        CreatePuzzle(difficulty);

        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
            {
                string buttonName = "Button" + i.ToString() + j.ToString();
                if(showBoard[i, j] != 0)
                {
                    appearance[showBoard[i, j]]++;
                    GameObject obiect = GameObject.Find(buttonName);
                    obiect.GetComponentInChildren<Text>().text = showBoard[i, j].ToString();
                }
            }
    }

    public void HintButtonAppear()
    {
        RectTransform rt = hintButton.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, -1050);
        RectTransform rt2 = hintRemain.GetComponent<RectTransform>();
        rt2.anchoredPosition = new Vector2(0, -950);
        RectTransform rt3 = mistakesText.GetComponent<RectTransform>();
        rt3.anchoredPosition = new Vector2(0, -850);
        hintRemain.text = "Hint Remain: " + (hintNumber + 1).ToString();
    }

    public void Hint()
    {
        ResetAllButtonColors();

        if (hintNumber == 0)
        {
            RectTransform rt = hintButton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(10000, 0);
        }

        System.Random rand = new System.Random();
        int row, col;
        do
        {
            row = rand.Next(0, 9);
            col = rand.Next(0, 9);
        } while (showBoard[row, col] != 0);
    
        showBoard[row, col] = board[row, col];
        appearance[showBoard[row, col]]++;
        hintNumber--;

        string buttonName = "Button" + row.ToString() + col.ToString();
        GameObject obiect = GameObject.Find(buttonName);
        obiect.GetComponentInChildren<Text>().text = showBoard[row, col].ToString();
        var image = obiect.GetComponentInChildren<Image>();
        hintRemain.text = "Hint Remain: " + (hintNumber + 1).ToString();

        if (appearance[showBoard[row, col]] == 9)
        {
            string buttonName2 = "Button" + showBoard[row, col].ToString();
            GameObject obiect2 = GameObject.Find(buttonName2);
            Destroy(obiect2);
        }
        for (int k = 0; k < 9; k++)
            for (int l = 0; l < 9; l++)
            {
                if (GameSense.showBoard[k, l] == GameSense.showBoard[row, col])
                {
                    string buttonName3 = "Button" + k.ToString() + l.ToString();
                    GameObject obiect3 = GameObject.Find(buttonName3);
                    var image2 = obiect3.GetComponent<Image>();
                    image2.color = clickedColor;
                }
            }
        image.color = hintColor;
        if(finishGame())
        {
            if (finishGame())
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

    }

    private bool finishGame()
    {
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                if (GameSense.showBoard[i, j] != GameSense.board[i, j])
                    return false;
        return true;
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
        for (int i = 0; i < 10; ++i)
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
