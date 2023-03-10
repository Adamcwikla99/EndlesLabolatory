using UnityEngine;

namespace TMPro.Examples
{

	public class CameraController : MonoBehaviour
	{
		#region Enums

		public enum CameraModes { Follow, Isometric, Free }

		#endregion
		#region Private Fields

		private Transform cameraTransform;

		private Vector3 currentVelocity = Vector3.zero;
		private Vector3 desiredPosition;
		private Transform dummyTarget;
		private float mouseWheel;
		private float mouseX;
		private float mouseY;
		private Vector3 moveVector;
		private bool previousSmoothing;

		#endregion
		#region Constants

		// Controls for Touches on Mobile devices
		//private float prev_ZoomDelta;

		private const string event_SmoothingValue = "Slider - Smoothing Value";
		private const string event_FollowDistance = "Slider - Camera Zoom";

		#endregion

		public CameraModes CameraMode = CameraModes.Follow;

		public Transform CameraTarget;

		public float ElevationAngle = 30.0f;

		public float FollowDistance = 30.0f;
		public float MaxElevationAngle = 85.0f;
		public float MaxFollowDistance = 100.0f;
		public float MinElevationAngle;
		public float MinFollowDistance = 2.0f;

		public bool MovementSmoothing = true;

		public float MovementSmoothingValue = 25f;

		public float MoveSensitivity = 2.0f;

		public float OrbitalAngle;
		public bool RotationSmoothing;
		public float RotationSmoothingValue = 5.0f;
		#region Unity Callbacks

		private void Awake()
		{
			Application.targetFrameRate = QualitySettings.vSyncCount > 0 ? 60 : -1;

			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				Input.simulateMouseWithTouches = false;
			}

			this.cameraTransform = this.transform;
			this.previousSmoothing = this.MovementSmoothing;
		}

		// Use this for initialization
		private void Start()
		{
			if (this.CameraTarget == null)
			{
				// If we don't have a target (assigned by the player, create a dummy in the center of the scene).
				this.dummyTarget = new GameObject("Camera Target").transform;
				this.CameraTarget = this.dummyTarget;
			}
		}

		// Update is called once per frame
		private void LateUpdate()
		{
			this.GetPlayerInput();

			// Check if we still have a valid target
			if (this.CameraTarget != null)
			{
				if (this.CameraMode == CameraModes.Isometric)
				{
					this.desiredPosition = this.CameraTarget.position + Quaternion.Euler(this.ElevationAngle, this.OrbitalAngle, 0f) * new Vector3(0, 0, -this.FollowDistance);
				}
				else if (this.CameraMode == CameraModes.Follow)
				{
					this.desiredPosition = this.CameraTarget.position +
										   this.CameraTarget.TransformDirection(Quaternion.Euler(this.ElevationAngle, this.OrbitalAngle, 0f) * new Vector3(0, 0, -this.FollowDistance));
				}

				// Free Camera implementation
				if (this.MovementSmoothing)
				{
					// Using Smoothing
					this.cameraTransform.position =
						Vector3.SmoothDamp(this.cameraTransform.position, this.desiredPosition, ref this.currentVelocity, this.MovementSmoothingValue * Time.fixedDeltaTime);
					//cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * 5.0f);
				}
				else
				{
					// Not using Smoothing
					this.cameraTransform.position = this.desiredPosition;
				}

				if (this.RotationSmoothing)
				{
					this.cameraTransform.rotation = Quaternion.Lerp(this.cameraTransform.rotation, Quaternion.LookRotation(this.CameraTarget.position - this.cameraTransform.position),
																	this.RotationSmoothingValue * Time.deltaTime);
				}
				else
				{
					this.cameraTransform.LookAt(this.CameraTarget);
				}

			}

		}

		#endregion
		#region Private Methods

		private void GetPlayerInput()
		{
			this.moveVector = Vector3.zero;

			// Check Mouse Wheel Input prior to Shift Key so we can apply multiplier on Shift for Scrolling
			this.mouseWheel = Input.GetAxis("Mouse ScrollWheel");

			float touchCount = Input.touchCount;

			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || touchCount > 0)
			{
				this.mouseWheel *= 10;

				if (Input.GetKeyDown(KeyCode.I))
				{
					this.CameraMode = CameraModes.Isometric;
				}

				if (Input.GetKeyDown(KeyCode.F))
				{
					this.CameraMode = CameraModes.Follow;
				}

				if (Input.GetKeyDown(KeyCode.S))
				{
					this.MovementSmoothing = !this.MovementSmoothing;
				}

				// Check for right mouse button to change camera follow and elevation angle
				if (Input.GetMouseButton(1))
				{
					this.mouseY = Input.GetAxis("Mouse Y");
					this.mouseX = Input.GetAxis("Mouse X");

					if (this.mouseY > 0.01f || this.mouseY < -0.01f)
					{
						this.ElevationAngle -= this.mouseY * this.MoveSensitivity;
						// Limit Elevation angle between min & max values.
						this.ElevationAngle = Mathf.Clamp(this.ElevationAngle, this.MinElevationAngle, this.MaxElevationAngle);
					}

					if (this.mouseX > 0.01f || this.mouseX < -0.01f)
					{
						this.OrbitalAngle += this.mouseX * this.MoveSensitivity;

						if (this.OrbitalAngle > 360)
						{
							this.OrbitalAngle -= 360;
						}

						if (this.OrbitalAngle < 0)
						{
							this.OrbitalAngle += 360;
						}
					}
				}

				// Get Input from Mobile Device
				if (touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;

					// Handle elevation changes
					if (deltaPosition.y > 0.01f || deltaPosition.y < -0.01f)
					{
						this.ElevationAngle -= deltaPosition.y * 0.1f;
						// Limit Elevation angle between min & max values.
						this.ElevationAngle = Mathf.Clamp(this.ElevationAngle, this.MinElevationAngle, this.MaxElevationAngle);
					}

					// Handle left & right 
					if (deltaPosition.x > 0.01f || deltaPosition.x < -0.01f)
					{
						this.OrbitalAngle += deltaPosition.x * 0.1f;

						if (this.OrbitalAngle > 360)
						{
							this.OrbitalAngle -= 360;
						}

						if (this.OrbitalAngle < 0)
						{
							this.OrbitalAngle += 360;
						}
					}

				}

				// Check for left mouse button to select a new CameraTarget or to reset Follow position
				if (Input.GetMouseButton(0))
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;

					if (Physics.Raycast(ray, out hit, 300, 1 << 10 | 1 << 11 | 1 << 12 | 1 << 14))
					{
						if (hit.transform == this.CameraTarget)
						{
							// Reset Follow Position
							this.OrbitalAngle = 0;
						}
						else
						{
							this.CameraTarget = hit.transform;
							this.OrbitalAngle = 0;
							this.MovementSmoothing = this.previousSmoothing;
						}

					}
				}

				if (Input.GetMouseButton(2))
				{
					if (this.dummyTarget == null)
					{
						// We need a Dummy Target to anchor the Camera
						this.dummyTarget = new GameObject("Camera Target").transform;
						this.dummyTarget.position = this.CameraTarget.position;
						this.dummyTarget.rotation = this.CameraTarget.rotation;
						this.CameraTarget = this.dummyTarget;
						this.previousSmoothing = this.MovementSmoothing;
						this.MovementSmoothing = false;
					}
					else if (this.dummyTarget != this.CameraTarget)
					{
						// Move DummyTarget to CameraTarget
						this.dummyTarget.position = this.CameraTarget.position;
						this.dummyTarget.rotation = this.CameraTarget.rotation;
						this.CameraTarget = this.dummyTarget;
						this.previousSmoothing = this.MovementSmoothing;
						this.MovementSmoothing = false;
					}

					this.mouseY = Input.GetAxis("Mouse Y");
					this.mouseX = Input.GetAxis("Mouse X");

					this.moveVector = this.cameraTransform.TransformDirection(this.mouseX, this.mouseY, 0);

					this.dummyTarget.Translate(-this.moveVector, Space.World);

				}

			}

			// Check Pinching to Zoom in - out on Mobile device
			if (touchCount == 2)
			{
				Touch touch0 = Input.GetTouch(0);
				Touch touch1 = Input.GetTouch(1);

				Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
				Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

				float prevTouchDelta = (touch0PrevPos - touch1PrevPos).magnitude;
				float touchDelta = (touch0.position - touch1.position).magnitude;

				float zoomDelta = prevTouchDelta - touchDelta;

				if (zoomDelta > 0.01f || zoomDelta < -0.01f)
				{
					this.FollowDistance += zoomDelta * 0.25f;
					// Limit FollowDistance between min & max values.
					this.FollowDistance = Mathf.Clamp(this.FollowDistance, this.MinFollowDistance, this.MaxFollowDistance);
				}

			}

			// Check MouseWheel to Zoom in-out
			if (this.mouseWheel < -0.01f || this.mouseWheel > 0.01f)
			{

				this.FollowDistance -= this.mouseWheel * 5.0f;
				// Limit FollowDistance between min & max values.
				this.FollowDistance = Mathf.Clamp(this.FollowDistance, this.MinFollowDistance, this.MaxFollowDistance);
			}

		}

		#endregion
	}
}
