using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private TurretBlueprint turretToBuild;

    //public GameObject standardTurretPrefab;
    //public GameObject missileTurretPrefab;
    public GameObject buildEffect;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one BuildManager in Scene");
        }
        instance = this;
    }

    public bool CanBuild { get { return turretToBuild != null; } }

    public bool HasMoney {get { return PlayerStats.Money >= turretToBuild.cost; } }

    public void BuildTurretOn(Node node)
    {
        if(PlayerStats.Money < turretToBuild.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }
        PlayerStats.Money -= turretToBuild.cost;

       GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
       node.turret = turret;

        GameObject effect = (GameObject)Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 1f);

        Debug.Log("Turret build! Money left"+ PlayerStats.Money);
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }

    //public void SetTurretToBuild(GameObject turret)
    //{
    //    turretToBuild = turret;
    //}
}
