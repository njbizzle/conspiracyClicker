using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public enum upgradeTypes{
        cloutPerGen,
        cloutGenSpeed
    }

    [SerializeField] GameObject upgradePreFab;

    [Header("Grids")]
    [SerializeField] GameObject availableUpgradeGrid;
    [SerializeField] GameObject purchasedUpgradeGrid;

    [Header("Upgrade Data Lists (only edit first)")]
    [SerializeField] List<UpgradeData> allUpgradeData = new List<UpgradeData>();
    [SerializeField] List<UpgradeData> unshownUpgradeData = new List<UpgradeData>();

    void Start(){
        unshownUpgradeData = new List<UpgradeData>(allUpgradeData);
        ConspiracyLevel[] conspiracyLevels = FindObjectsOfType<ConspiracyLevel>();
        foreach (UpgradeData upgradeData in allUpgradeData){
            
            foreach (ConspiracyLevel conspiracyLevel in conspiracyLevels){
                if (conspiracyLevel.gameObject.name == upgradeData.conspiracyLevelName){
                    upgradeData.conspiracyLevel = conspiracyLevel;
                }
            }
            upgradeData.canPurchase = false;
            upgradeData.added = false;
            upgradeData.purchased = false;
        }
    }

    void Update(){
        foreach (UpgradeData upgradeData in allUpgradeData){
            // add all
            if (!upgradeData.added){
                upgradeData.added = true;
                CreateUpgrade(availableUpgradeGrid, upgradeData);
                upgradeData.addToAvailable = false; 
            }
        }
    }

    void CreateUpgrade(GameObject grid, UpgradeData upgradeData){
        GameObject newUpgradeObject = Instantiate(upgradePreFab, Vector3.zero, Quaternion.identity); // instanciate it

        newUpgradeObject.GetComponent<RectTransform>().SetParent(grid.transform); // move it under the grid
        newUpgradeObject.GetComponent<Upgrade>().SetUpgradeData(upgradeData); // set the data to the upgrade prefab instance
    }

    public void MoveToPurchase(RectTransform upgradeTransform){
        upgradeTransform.SetParent(purchasedUpgradeGrid.transform);
    }
}