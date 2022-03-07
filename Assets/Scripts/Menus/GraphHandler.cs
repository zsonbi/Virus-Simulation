using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphHandler : MonoBehaviour
{
    public void GraphButtonClick()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}