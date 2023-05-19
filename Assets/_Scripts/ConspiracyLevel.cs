using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConspiracyLevel : MonoBehaviour
{
    [Header("Not Stats")]
    [SerializeField] GameManager_ gameManager;
    [SerializeField] TMP_Text levelNameDisplay;
    [SerializeField] Clicker clicker;

    [Space(10)]
    [SerializeField] GameObject conspiracistPreFab;
    [SerializeField] GameObject conspiracistGridContent;

    [Header("Stats")]
    [SerializeField] CustomButton showHideStatsButton;
    [SerializeField] TMP_Text showHideStatsButtonText;
    [SerializeField] bool statsShown;

    [SerializeField] float totalSpent;

    [Space(10)]
    [SerializeField] GameObject statsScreen;
    [SerializeField] TMP_Text statsText;

    [Header("Stuff that changes")]
    [SerializeField] string levelName;
    [SerializeField] float cloutPerClick;
    [SerializeField] float defaultCloutPerClick;

    [SerializeField] ConspiracistData[] conspiracistDataPlural;
    [SerializeField] List<Conspiracist> conspiracists;

    void Start()
    {
        showHideStatsButton.AddOnClick(showHideStats);

        levelNameDisplay.text = levelName;
        gameManager = FindObjectOfType<GameManager_>();

        cloutPerClick = defaultCloutPerClick;
        
        foreach(ConspiracistData conspiracistData in conspiracistDataPlural){
            conspiracistData.Reset();

            GameObject newConspiracistObject = Instantiate(conspiracistPreFab, Vector3.zero, Quaternion.identity);
            Conspiracist newConspiracist = newConspiracistObject.GetComponent<Conspiracist>();

            newConspiracist.SetConspiracistDataAndLevel(conspiracistData, this);
            newConspiracistObject.transform.SetParent(conspiracistGridContent.transform);

            conspiracists.Add(newConspiracist);
        }
    }

    void showHideStats(){
        statsShown = !statsShown;
        if (!statsShown){
            showHideStatsButtonText.text = "Show Stats";
            statsScreen.SetActive(false);
        } else{
            showHideStatsButtonText.text = "Hide Stats";
            statsScreen.SetActive(true);
        }
    }

    public void AddTotalSpent(float spent){
        totalSpent += spent;
    }
    public float GetTotalSpent(){
        return totalSpent;
    }

    public float GetToatlCPS(){
        float cps = 0;
        foreach(ConspiracistData conspiracistData in conspiracistDataPlural){
            if (conspiracistData.conspiracistQuantity > 0){
                cps += (conspiracistData.cloutPerGenerate * conspiracistData.cloutGenMult * conspiracistData.conspiracistQuantity) / conspiracistData.generateSpeed;
            }
        }
        return cps;
    }
}
