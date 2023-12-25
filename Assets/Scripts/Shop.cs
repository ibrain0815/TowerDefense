using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileTurret;
    public TurretBlueprint laserBeamer;

    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SelectStandardTurret() 
    {
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileTurret()
    {
        Debug.Log("Missile Turret Purchased");
        buildManager.SelectTurretToBuild(missileTurret);
    }

    public void SelectLazerBeamer()
    {
        Debug.Log("Missile Turret Purchased");
        buildManager.SelectTurretToBuild(laserBeamer);
    }


}
