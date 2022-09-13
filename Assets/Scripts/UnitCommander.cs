using System;
using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    public GameObject selectionMarkerPrefab;
    public LayerMask layerMask;

    private UnitSelection _unitSelection;
    private Camera _cam;

    private void Awake()
    {
        _unitSelection = GetComponent<UnitSelection>();
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _unitSelection.HasUnitsSelected())
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Unit[] selectedUnits = _unitSelection.GetSelectedUnits();

            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                _unitSelection.RemoveNullUnitsFromSelection();
                
                if (hit.collider.CompareTag("Ground"))
                {
                    UnitsMoveToPosition(hit.point, selectedUnits);
                    CreateSelectionMarker(hit.point, false);
                }
                else if (hit.collider.CompareTag("Resource"))
                {
                    UnitsGatherResource(hit.collider.GetComponent<ResourceSource>(), selectedUnits);
                    CreateSelectionMarker(hit.collider.transform.position, true);
                }
                else if (hit.collider.CompareTag("Unit"))
                {
                    Unit enemy = hit.collider.gameObject.GetComponent<Unit>();

                    if (!Player.me.IsMyUnit(enemy))
                    {
                        UnitsAttackEnemy(enemy, selectedUnits);
                        CreateSelectionMarker(enemy.transform.position, false);
                    }
                }
            }
        }
    }

    private void UnitsMoveToPosition(Vector3 movePos, Unit[] units)
    {
        Vector3[] destinations = UnitMover.GetUnitGroupDestinations(movePos, units.Length, 2);
        
        for (int x = 0; x < units.Length; x++)
        {
            units[x].MoveToPosition(destinations[x]);
        }
    }

    private void UnitsGatherResource(ResourceSource resource, Unit[] units)
    {
        if (units.Length == 1)
        {
            units[0].GatherResource(resource, UnitMover.GetUnitDestinationAroundResource(resource.transform.position));
        }
        else
        {
            Vector3[] destinations = UnitMover.GetUnitGroupDestinationsAroundResource(resource.transform.position, units.Length);

            for (int x = 0; x < units.Length; x++)
            {
                units[x].GatherResource(resource, destinations[x]);
            }
        }
    }

    private void UnitsAttackEnemy(Unit target, Unit[] units)
    {
        for (int x = 0; x < units.Length; x++)
        {
            units[x].AttackUnit(target);
        }
    }

    private void CreateSelectionMarker(Vector3 pos, bool large)
    {
        GameObject marker = Instantiate(selectionMarkerPrefab, new Vector3(pos.x, 0.01f, pos.z), Quaternion.identity);

        if (large)
        {
            marker.transform.localScale = Vector3.one * 3;
        }
    }
}
