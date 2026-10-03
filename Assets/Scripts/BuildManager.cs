using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InputActionReference fireAction;
    [SerializeField] private GameObject selectionIndicator;
    [SerializeField] private Tilemap tilemap;
    private Vector3Int currentPointerPosition;
    [SerializeField] private Transform worldCanvas;
    [SerializeField] private GameObject beeCountTextPrefab;

    // Building Selection
    [SerializeField] private Dictionary<string, GameObject> buildingPrefabs;
    private string selectedBuildingType = null;
    private GameObject currentBuildingObject;


    private void OnEnable() => fireAction.action.Enable();
    private void OnDisable() => fireAction.action.Disable();
    void Start()
    {
        fireAction.action.performed += OnFirePerformed;
    }

    private void OnDestroy()
    {
        fireAction.action.performed -= OnFirePerformed;
    }

    private void Update()
    {
        ShootRaycastFromCenter();
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) return;
        TryBuild();
    }

    private void TryBuild()
    {
        if (currentBuildingObject == null) return;

        if (currentBuildingObject.TryGetComponent(out Building building))
        {
            building.Build(worldCanvas, beeCountTextPrefab);
        }

        if (currentBuildingObject.TryGetComponent(out Capital capital))
        {
            capital.StartGame();
        }

        selectedBuildingType = null;
        currentBuildingObject = null;
    }

    void ShootRaycastFromCenter()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(mousePosition);

        currentPointerPosition = tilemap.WorldToCell(worldPoint);

        Vector3 pos = tilemap.CellToWorld(currentPointerPosition);
        selectionIndicator.transform.position = pos;

        if (currentBuildingObject != null)
        {
            currentBuildingObject.transform.position = GetBuildingPosition();
        }
    }

    private Vector3 GetBuildingPosition()
    {
        if (currentBuildingObject == null) return Vector3.zero;

        return tilemap.CellToWorld(currentPointerPosition) + new Vector3(0, currentBuildingObject.transform.localScale.y / 4, 0);
    }

#region Building Selection

    public void SelectBuildingType(string type)
    {
        selectedBuildingType = type;

        GameObject prefab = buildingPrefabs[selectedBuildingType];

        currentBuildingObject = Instantiate(prefab, GetBuildingPosition(), Quaternion.identity);

        if (currentBuildingObject.TryGetComponent(out Building building))
        {
            building.Hover();
        }
    }

    public void CancelBulding()
    {
        selectedBuildingType = null;
    }

#endregion
}
