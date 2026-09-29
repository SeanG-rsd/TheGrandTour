using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Capital capital;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InputActionReference fireAction;


    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private GameObject optionWindowPrefab;

    private Building currentBuilding;
    private float currentMoveAmount;

    private GameObject optionWindowObj;

    private void OnEnable() => fireAction.action.Enable();
    private void OnDisable() => fireAction.action.Disable();
    void Start()
    {
        fireAction.action.performed += OnFirePerformed;

        capital.StartGame();

    }

    private void OnDestroy()
    {
        fireAction.action.performed -= OnFirePerformed;
    }

    private void SelectBuilding(Building building)
    {
        if (currentBuilding == null)
        {
            currentBuilding = building;
            DisplayMoveOptions(building.gameObject);
        } 
        else if (currentBuilding == building)
        {
            currentBuilding = null;
            HideMoveOptions();
        } 
        else
        {
            currentBuilding.MoveBees(currentMoveAmount, building);
            currentBuilding = null;
        }
    }

    private void DisplayMoveOptions(GameObject buildingObj)
    {
        optionWindowObj = Instantiate(optionWindowPrefab, buildingObj.transform.position, Quaternion.identity, worldCanvas.transform);

        if (optionWindowObj.TryGetComponent(out OptionWindow optionWindow))
        {
            optionWindow.SetupOptions(SelectMoveOption);
        }
    }

    private void HideMoveOptions()
    {
        if (optionWindowObj != null)
        {
            Destroy(optionWindowObj);
        }
    }

    public void SelectMoveOption(float choice)
    {
        Debug.Log(choice);
        currentMoveAmount = choice;

        HideMoveOptions();
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) return;
        ShootRaycastFromCenter();
    }

    void ShootRaycastFromCenter()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // 4. Turn that screen position into a 3D Ray
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(mousePosition);

        Collider2D hit = Physics2D.OverlapPoint(worldPoint);
        if (hit != null)
        {
            Debug.Log($"Clicked on: {hit.name} at {worldPoint}");

            if (hit.TryGetComponent(out Building building))
            {
                SelectBuilding(building);
            }
        }
    }
}
