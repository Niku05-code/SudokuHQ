using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGenerator : MonoBehaviour
{
    public GameObject linePrefab;

    public void AddSeparators()
    {
        for (int col = 0; col < 4; col++)
        {
            GameObject verticalLine = Instantiate(linePrefab, transform);
            verticalLine.name = $"VerticalLine_{col}";
            RectTransform rt = verticalLine.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(500 + col * 300, 485);
        }

        for (int row = 0; row < 4; row++)
        {
            GameObject horizontalLine = Instantiate(linePrefab, transform);
            horizontalLine.name = $"HorizontalLine_{row}";
            RectTransform rt = horizontalLine.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(900, 10);
            rt.anchoredPosition = new Vector2(950, 930 - row * 300);
        }
    }

}
