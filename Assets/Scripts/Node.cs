using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color notEnoughMoneyColor;
    public Color hoverColor;
    private Renderer rend;

    [Header ("Optional")]
    public GameObject turret;

    private Color startColor;

    public Vector3 postionOffset;

    BuildManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition() 
    {
          return transform.position + postionOffset;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (!buildManager.CanBuild) { return; }


        if(turret != null)
        {
            Debug.Log("can't build there - TODO: Display on screen.");
            return;
        }

        buildManager.BuildTurretOn(this);
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if(!buildManager.CanBuild) { return; }

        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
        
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }

}
