using Events.RoomEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MapGenerator.RoomLeyout
{
	/// <summary>
	///     class responsible for reading user input in room grid editor
	/// </summary>
	public class RoomEditorInput : MonoBehaviour
	{
		#region Private Propertis
		
		[field: SerializeField]
		private PlayerEditor playerInput { get; set; }

		#endregion
		#region Private Variables

		private InputAction playerMouseClick;
		private InputAction playerNextCell;
		private InputAction playerPreviousCell;
		private InputAction playerMousePosition;
		private PlayerInputAction inputAction;

		private bool testHold;
		private RaycastHit hitData;

		#endregion
		#region Unity Callbacks

		private void OnEnable() => EnableInputEvents();

		private void OnDisable() => DisableInputEvents();

		private void Awake() => this.inputAction = new PlayerInputAction();

		private void Update()
		{
			this.ReadMovementInput();
			ReadZoomInput();
			this.MousePressedAction();
		}

		#endregion
		#region Private Methods

		private void EnableInputEvents()
		{
			EnableMouseClickEvents();
			EnableMousePositionEvents();
			EnableChangeCellTypeEvents();
			EnableCameraZoomEvents();
		}

		private void EnableMouseClickEvents()
		{
			this.playerInput.MouseClick += MouseClick;
			this.playerMouseClick = this.inputAction.Player.Fire;

			this.inputAction.Player.Fire.performed += MouseButtonDown;
			this.inputAction.Player.Fire.canceled += MouseButtonUp;
			this.inputAction.Player.Fire.Enable();
			this.inputAction.Player.PointerPosition.Enable();
			this.playerMouseClick.Enable();
		}

		private void EnableMousePositionEvents()
		{
			this.playerMousePosition = this.inputAction.Player.PointerPosition;
			this.playerMousePosition.Enable();
		}

		private void EnableChangeCellTypeEvents()
		{
			this.playerNextCell = this.inputAction.Player.NextCellType;
			this.playerPreviousCell = this.inputAction.Player.PreviousCellType;
			this.inputAction.Player.NextCellType.performed += NextCellType;
			this.inputAction.Player.PreviousCellType.performed += PreviousCellType;
			this.playerNextCell.Enable();
			this.playerPreviousCell.Enable();
		}

		private void EnableCameraZoomEvents()
		{
			this.inputAction.Player.Move.Enable();
			this.inputAction.Player.Zoom.Enable();
		}

		private void DisableInputEvents()
		{
			DisableZoomEvents();
			DisableMouseButtonEvents();
			DisableMousePositionEvents();
			DisableChangeCellTypeEvents();
		}

		private void DisableZoomEvents()
		{
			this.inputAction.Player.Move.Disable();
			this.inputAction.Player.Zoom.Disable();
		}

		private void DisableMouseButtonEvents()
		{
			this.playerInput.MouseClick -= MouseClick;
			this.inputAction.Player.Fire.performed -= MouseButtonDown;
			this.inputAction.Player.Fire.canceled -= MouseButtonUp;
			this.playerMouseClick.Disable();
			this.inputAction.Player.Fire.Disable();
		}

		private void DisableMousePositionEvents()
		{
			this.playerMousePosition.Disable();
			this.inputAction.Player.PointerPosition.Disable();
		}

		private void DisableChangeCellTypeEvents()
		{
			this.inputAction.Player.NextCellType.performed -= NextCellType;
			this.inputAction.Player.PreviousCellType.performed -= PreviousCellType;
			this.playerNextCell.Disable();
			this.playerPreviousCell.Disable();
		}

		private void MouseClick() => this.playerInput.MouseClick?.Invoke();

		private void MouseButtonDown(InputAction.CallbackContext obj) => this.testHold = true;

		private void MouseButtonUp(InputAction.CallbackContext obj) => this.testHold = false;

		private void NextCellType(InputAction.CallbackContext obj) => this.playerInput.NextCell?.Invoke();

		private void PreviousCellType(InputAction.CallbackContext obj) => this.playerInput.PreviousCell?.Invoke();

		private void ReadMovementInput()
		{
			Vector2 inputValue = this.inputAction.Player.Move.ReadValue<Vector2>();

			if (inputValue.x != 0 || inputValue.y != 0)
			{
				this.playerInput.ReadMove?.Invoke(inputValue);
			}
		}

		private void ReadZoomInput()
		{
			Vector2 inputValue = this.inputAction.Player.Zoom.ReadValue<Vector2>();

			if (inputValue.y != 0)
			{
				this.playerInput.ReadZoom?.Invoke(inputValue.y);
			}
		}

		private void MousePressedAction()
		{
			if (this.testHold == false)
			{
				return;
			}

			Vector3 pos = this.inputAction.Player.PointerPosition.ReadValue<Vector2>();
			Ray ray = Camera.main.ScreenPointToRay(pos);

			if (Physics.Raycast(ray, out this.hitData, 1000, 1 << 6))
			{
				this.playerInput.ChangeMarkerType?.Invoke(this.hitData);
			}
		}

		#endregion
	}
}
