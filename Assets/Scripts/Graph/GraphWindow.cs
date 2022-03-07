using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphWindow : MonoBehaviour
{
    [SerializeField]
    private Sprite circleSprite;

    private RectTransform graphRect;
    private int highest = 50;
    private int maxNumberOfPoints = 10;
    private int ySeparatorCount = 10;
    private float distanceBetweenPoints = 0;
    private List<Text> xLabels = new List<Text>();
    private Color graphElementColor = Color.white;
    private List<RectTransform> points;
    private List<RectTransform> lines;

    public void InitGraph(int maxValue)
    {
        this.highest = maxValue;
        points = new List<RectTransform>();
        lines = new List<RectTransform>();
        graphRect = this.GetComponent<RectTransform>();
        //CreateCircle(new Vector2(2, 2));
        distanceBetweenPoints = graphRect.rect.width / maxNumberOfPoints;
        CreateSeparators();
        CreateLabels();
    }

    //-----------------------------------------------
    //Creates the labels for the graph
    private void CreateLabels()
    {
        Text label = new GameObject($"label", typeof(Text)).GetComponent<Text>();
        label.transform.SetParent(graphRect);
        RectTransform rectTransform = label.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.localScale = new Vector3(1, 1, 1);
        rectTransform.anchoredPosition = new Vector3(graphRect.rect.width / 2, -20, 1);
        label.text = "day";
        label.color = graphElementColor;
        label.alignment = TextAnchor.MiddleCenter;
        label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        //X axis
        for (int i = 1; i <= maxNumberOfPoints; i++)
        {
            Text labelX = Instantiate(label, graphRect, false);
            labelX.GetComponent<RectTransform>().anchoredPosition = new Vector3(distanceBetweenPoints * i, -7.5f, 1);
            labelX.text = (i).ToString();
            xLabels.Add(labelX);
        }

        int yValueDifference = highest / ySeparatorCount;
        float yValueDist = graphRect.rect.height / ySeparatorCount;
        //Y axis
        for (int i = 0; i <= ySeparatorCount; i++)
        {
            Text labelY = Instantiate<Text>(label, graphRect, false);
            labelY.GetComponent<RectTransform>().anchoredPosition = new Vector3(-60, i * yValueDist, 1);
            labelY.text = (i * yValueDifference).ToString();
            labelY.alignment = TextAnchor.MiddleRight;
        }
    }

    //-----------------------------------------------------------------------------
    //Creates the separator lines
    private void CreateSeparators()
    {
        //X separators
        float yValueDist = graphRect.rect.height / ySeparatorCount;
        RectTransform xSeparator = new GameObject("xLine", typeof(Image)).GetComponent<RectTransform>();
        xSeparator.transform.SetParent(graphRect);
        xSeparator.anchorMin = new Vector2(0, 0);
        xSeparator.anchorMax = new Vector2(0, 0);
        xSeparator.localScale = new Vector3(1, 1, 1);
        xSeparator.sizeDelta = new Vector2(graphRect.rect.width, 1);
        xSeparator.anchoredPosition = new Vector3(graphRect.rect.width / 2, 0, 1);
        xSeparator.GetComponent<Image>().color = graphElementColor;
        for (int i = 1; i < maxNumberOfPoints; i++)
        {
            RectTransform newXline = Instantiate<RectTransform>(xSeparator, graphRect, false);
            newXline.anchoredPosition = new Vector3(graphRect.rect.width / 2, i * yValueDist, 1);
        }

        //Y separators
        RectTransform ySeparator = Instantiate<RectTransform>(xSeparator, graphRect, false);
        ySeparator.sizeDelta = new Vector2(1, graphRect.rect.height);
        ySeparator.anchoredPosition = new Vector3(0, graphRect.rect.height / 2, 1);
        for (int i = 1; i < ySeparatorCount; i++)
        {
            RectTransform newYline = Instantiate<RectTransform>(ySeparator, graphRect, false);
            newYline.anchoredPosition = new Vector3(i * distanceBetweenPoints, graphRect.rect.height / 2, 1);
        }
    }

    //------------------------------------------------------------
    //Creates a single point
    private void CreatePoint(Vector2 anchoredPos)
    {
        if (points.Count != 0)
        {
            CreateDotConnection(points[points.Count - 1].GetComponent<RectTransform>().anchoredPosition, anchoredPos);
        }
        if (points.Count == maxNumberOfPoints)
        {
            RectTransform rectTransform = points[0];
            points.RemoveAt(0);
            rectTransform.anchoredPosition = anchoredPos;
            Destroy(lines[0].gameObject);
            lines.RemoveAt(0);
            points.Add(rectTransform);
            PushToTheLeft();
        }
        else
        {
            GameObject circle = new GameObject("circle", typeof(Image));
            circle.transform.SetParent(graphRect, false);
            circle.GetComponent<Image>().sprite = circleSprite;
            circle.GetComponent<Image>().color = graphElementColor;
            RectTransform rectTransform = circle.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPos;
            rectTransform.sizeDelta = new Vector2(5, 5);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            points.Add(rectTransform);
        }
    }

    //------------------------------------------------------------------------------------
    //Pushes every element to the right
    private void PushToTheLeft()
    {
        foreach (var point in points)
        {
            point.anchoredPosition = new Vector3(point.anchoredPosition.x - distanceBetweenPoints, point.anchoredPosition.y);
        }

        foreach (var line in lines)
        {
            line.anchoredPosition = new Vector3(line.anchoredPosition.x - distanceBetweenPoints, line.anchoredPosition.y);
        }

        int smallest = System.Convert.ToInt32(xLabels[0].text) + 1;
        for (int i = 0; i < maxNumberOfPoints; i++)
        {
            xLabels[i].text = (smallest + i).ToString();
        }
    }

    //---------------------------------------------------------------------
    /// <summary>
    /// Adds a value to the graph
    /// </summary>
    /// <param name="value">What we want to illustrate</param>
    public void AddValue(float value)
    {
        if (points == null)
        {
            return;
        }

        CreatePoint(new Vector2(distanceBetweenPoints * points.Count, value / highest * graphRect.rect.height));
    }

    //-------------------------------------------------------------
    //Connect two points
    private void CreateDotConnection(Vector2 startPosition, Vector2 endPosition)
    {
        GameObject line = new GameObject("line", typeof(Image));
        line.transform.SetParent(graphRect);
        line.GetComponent<Image>().color = graphElementColor;
        RectTransform rectTransform = line.GetComponent<RectTransform>();
        Vector2 dir = (endPosition - startPosition).normalized;
        float dist = Vector2.Distance(startPosition, endPosition);
        rectTransform.sizeDelta = new Vector2(dist, 1.5f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        rectTransform.anchoredPosition = startPosition + dir * dist * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI);
        lines.Add(rectTransform);
    }

    //-------------------------------------
    //Adds multiple points
    private void DisplayMultiplePoints(List<float> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            AddValue(values[i]);
        }
    }
}