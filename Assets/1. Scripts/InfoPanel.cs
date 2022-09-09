using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public GameObject infoPanel;

    public TextMeshProUGUI infoTitle;
    public TextMeshProUGUI infoDescription;

    private float maxShowTime = 3.0f;
    public float currentShowTime = 0.0f;

    private bool showInfo = false;

    private void Update() {
        if(showInfo == true) {
            currentShowTime += Time.deltaTime;
            if(currentShowTime >= maxShowTime) {
                showInfo = false;
                currentShowTime = 0.0f;
                infoPanel.SetActive(false);
            }
        }

    }


    public void ShowTitle(string _title, string _description, string _type) {
        currentShowTime = 0.0f;
        showInfo = true;
        infoPanel.SetActive(true);
        infoTitle.text = _title + " " + _type;
        infoDescription.text = _description;


    }


}
