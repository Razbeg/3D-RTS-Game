using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public RectTransform selectionBox;
    public LayerMask unitLayerMask;

    private List<Unit> selectedUnits = new List<Unit>();
    private Vector2 _startPos;

    private Camera _cam;
    private Player _player;

    private void Awake()
    {
        _cam = Camera.main;
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSelectionVisual(false);
            selectedUnits = new List<Unit>();
            
            TrySelect(Input.mousePosition);
            _startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }
    }

    public Unit[] GetSelectedUnits()
    {
        return selectedUnits.ToArray();
    }

    private void TrySelect(Vector2 screenPos)
    {
        Ray ray = _cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, unitLayerMask))
        {
            Unit unit = hit.collider.GetComponent<Unit>();

            if (_player.IsMyUnit(unit))
            {
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }
    }

    private void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }

        float width = curMousePos.x - _startPos.x;
        float height = curMousePos.y - _startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = _startPos + new Vector2(width / 2, height / 2);
    }

    private void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - selectionBox.sizeDelta / 2;
        Vector2 max = selectionBox.anchoredPosition + selectionBox.sizeDelta / 2;

        foreach (Unit unit in _player.units)
        {
            Vector3 screenpos = _cam.WorldToScreenPoint(unit.transform.position);

            if (screenpos.x > min.x && screenpos.x < max.x && screenpos.y > min.y && screenpos.y < max.y)
            {
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }
    }

    public void RemoveNullUnitsFromSelection()
    {
        for (int x = 0; x < selectedUnits.Count; x++)
        {
            if (selectedUnits[x] == null)
            {
                selectedUnits.RemoveAt(x);
            }
        }
    }

    private void ToggleSelectionVisual(bool selected)
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.ToggleSelectionVisual(selected);
        }
    }

    public bool HasUnitsSelected()
    {
        return selectedUnits.Count > 0;
    }
}
