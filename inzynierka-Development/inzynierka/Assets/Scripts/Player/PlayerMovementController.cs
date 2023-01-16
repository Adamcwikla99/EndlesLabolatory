using System;
using Events.Menu;
using Events.Player;
using Events.Room;
using ScenesManager;
using Structures.Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Player
{
    /// <summary>
    ///  Class responsible for interpreting user input and performing required movement action - showing UI, player map movement or event actions like jump/buy/weapon fire
    /// </summary>
    public class PlayerMovementController : MonoBehaviour
    {
        #region properties
        
        [field: SerializeField]
        private float BonusSpeedUp { get; set; }

        [field: SerializeField]
        private float BonusSpeedDown { get; set; }

        [field: SerializeField]
        private Rigidbody GamePlayer { get; set; }

        [field: SerializeField]
        private Camera PlayerCamera { get; set; }

        [field: SerializeField]
        private float AddMovementForce { get; set; } = 1f;
        
        [field: SerializeField]
        private float SprintSpeed { get; set; } = 5f;
        
        [field: SerializeField]
        private float MaxMovementSpeed { get; set; } = 10f;

        [field: SerializeField]
        private float CameraSensibility { get; set; } = 0.05f;

        [field: SerializeField]
        private float CameraMovementDeny { get; set; } = 5f;

        [field: SerializeField]
        private float CameraSped { get; set; } = 0.5f;
        
        [field: SerializeField]
        private PlayerMovementEvents MovementEvents { get; set; }
        
        [field: SerializeField]
        private PlayerFireEvents FireEvents { get; set; }
        
        [field: SerializeField]
        private PlayerChangeWeaponEvents ChangeWeaponEvents { get; set; }

        [field: SerializeField]
        private PlayerSpeedEvents PlayerSpeedController { get; set; }    
        
        [field: SerializeField]
        private BuyItemEvent BuyEvent { get; set; }
        
        [field: SerializeField]
        private ToggleMinimapEvent Toggle { get; set; }
        
        [field: SerializeField]
        private SwitchMenu SwitchMenu { get; set; }
        
        [field: SerializeField]
        private GameProgressManager ProgressManager { get; set; } 
        
        #endregion
        #region variables

        private PlayerInputAction inputAction;
        private PlayerInputAction.PlayerControlerActions inputActionPlayerController;
        
        private InputAction playerPreviousWeapon;
        private InputAction playerNextWeapon;
        private InputAction playerBuyItem;
        private InputAction playerJump;
        private InputAction playerSprint;
        private InputAction showMinimap;
        private InputAction pausePopup;

        private bool sprint = false;
        private bool canJump = true;
        private bool isSpeedUpActive = false;
        private bool isSpeedDownActive = false;
        
        
        #endregion
        #region unityCallbacks

        private void Awake()
        {
            this.inputAction = new PlayerInputAction();
            this.inputActionPlayerController = this.inputAction.PlayerControler;
            this.playerPreviousWeapon = this.inputActionPlayerController.PreviousWepon;
            this.playerNextWeapon = this.inputActionPlayerController.NextWepon;
            this.playerJump = this.inputActionPlayerController.Jump;
            this.playerSprint = this.inputActionPlayerController.SprintToggle;
            this.playerBuyItem = this.inputActionPlayerController.BuyItem;
            this.showMinimap = this.inputActionPlayerController.ToggleMinimap;
            this.pausePopup = this.inputActionPlayerController.Pause;
        }

        private void Update()
        {
            this.ReadMovementInput();
            this.ReadCameraMovementInput();
            if (inputActionPlayerController.Fire.IsPressed())
            {
                PlayerFire();
            }
        }

        private void LateUpdate()
        {
            var localEulerAngles = this.GamePlayer.transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(0, localEulerAngles.y, 0);
        }

        private void OnEnable() => this.EnableInputEvents();

        private void OnDisable() => this.DisableInputEvents();

        #endregion
        #region methods

        /// <summary>
        /// methode that enables input events
        /// </summary>
        private void EnableInputEvents()
        {
            this.inputActionPlayerController.Enable();
            EnablePlayerPreviousWeapon();
            EnablePlayerNextWeapon();
            EnablePlayerJump();
            EnablePlayerSprint();
            EnablePlayerBuy();
            EnableMinimap();
            EnablePausePopup();
            
            this.MovementEvents.PlayerCanJump += TryReanableJump;
            this.PlayerSpeedController.AddSpeedDown += (b => this.isSpeedDownActive = b);
            this.PlayerSpeedController.AddSpeedUp += (b => this.isSpeedUpActive = b);
            
        }

        /// <summary>
        /// methode that enables pause popup
        /// </summary>
        private void EnablePausePopup()
        {
            this.pausePopup.performed += TogglePausePopup;
        }

        /// <summary>
        /// methode that disables pause popup
        /// </summary>
        private void DisablePausePopup()
        {
            this.pausePopup.performed -= TogglePausePopup;
        }

        /// <summary>
        /// methode that shows or hides pause popup
        /// </summary>
        private void TogglePausePopup(InputAction.CallbackContext obj)
        {
            this.Toggle.ChangeMinimapState?.Invoke(false);
            this.SwitchMenu.switchMenu?.Invoke(MenuType.PauseMenu);
            this.ProgressManager.RelayNewPauseData();
        }
        
        /// <summary>
        /// methode that enables minimap popup
        /// </summary>
        private void EnableMinimap()
        {
            this.showMinimap.performed += ShowMinimap;
        }

        /// <summary>
        /// methode that disables minimap popup
        /// </summary>
        private void DisableMinimap()
        {
            this.showMinimap.performed -= ShowMinimap;
        }

        /// <summary>
        /// methode that shows minimap
        /// </summary>
        private void ShowMinimap(InputAction.CallbackContext obj)
        {
            Toggle.ToggleMinimap?.Invoke();
        }

        /// <summary>
        /// methode that enables player buy event
        /// </summary>
        private void EnablePlayerBuy()
        {
            this.playerBuyItem.performed += OnPlayerBuyItemPerformed;
        }
        
        /// <summary>
        /// methode that calls player buy event
        /// </summary>
        private void OnPlayerBuyItemPerformed(InputAction.CallbackContext obj)
        {
            this.BuyEvent.TryBuyItem?.Invoke();
        }

        /// <summary>
        /// methode that disables player buy event
        /// </summary>
        private void DisablePlayerBuy()
        {
            this.playerBuyItem.performed -= OnPlayerBuyItemPerformed;
        }
        
        /// <summary>
        /// methode that enables player jump event
        /// </summary>
        private void EnablePlayerJump()
        {
            this.playerJump.performed += PlayerJump;
        }

        /// <summary>
        /// methode that tries to perform jump action
        /// </summary>
        private void TryReanableJump(GameObject objectToCheck)
        {
            if (objectToCheck != this.gameObject)
            {
                return;
            }

            this.canJump = true;
        }
        
        /// <summary>
        /// methode that enables player sprint event
        /// </summary>
        private void EnablePlayerSprint()
        {
            this.playerSprint.performed += PlayerSprint;
        }

        /// <summary>
        /// methode that performers player sprint event
        /// </summary>
        private void PlayerSprint(InputAction.CallbackContext obj)
        {
            sprint = !this.sprint;
        }
        
        /// <summary>
        /// methode that disables player jump event
        /// </summary>
        private void DisablePlayerJump()
        {
            this.playerSprint.performed -= PlayerSprint;
        }
        
        /// <summary>
        /// methode that disables player sprint event
        /// </summary>
        private void DisablePlayerSprint()
        {
            this.playerSprint.performed -= PlayerSprint;
        }
        
        /// <summary>
        /// methode that disables player input methodes
        /// </summary>
        private void DisableInputEvents()
        {
            DisablePlayerPreviousWeapon();
            DisablePlayerNextWeapon();
            DisablePlayerJump();
            DisablePlayerSprint();
            DisablePlayerBuy();
            DisableMinimap();
            DisablePausePopup();
            
            this.inputActionPlayerController.Disable();
            this.MovementEvents.PlayerCanJump -= TryReanableJump;
            this.PlayerSpeedController.AddSpeedDown -= (b => this.isSpeedDownActive = b);
            this.PlayerSpeedController.AddSpeedUp -= (b => this.isSpeedUpActive = b);
        }
        
        /// <summary>
        /// methode that enables player change weapon to next type event
        /// </summary>
        private void EnablePlayerPreviousWeapon()
        {
            this.playerPreviousWeapon.performed += ChangeToPreviousWeapon;
        }
        
        /// <summary>
        /// methode that disbales player change weapon to next type event
        /// </summary>
        private void EnablePlayerNextWeapon()
        {
           this.playerNextWeapon.performed += ChangeToNextWeapon;
        }
        
        /// <summary>
        /// methode that enables player change weapon to previous type event
        /// </summary>
        private void DisablePlayerPreviousWeapon()
        {
            this.playerPreviousWeapon.performed -= ChangeToPreviousWeapon;
        }
        
        /// <summary>
        /// methode that disables player change weapon to previous type event
        /// </summary>
        private void DisablePlayerNextWeapon()
        {
            this.playerNextWeapon.performed -= ChangeToNextWeapon;
        }

        /// <summary>
        /// methode that reads player constant input
        /// </summary>
        private void ReadMovementInput()
        {
            Vector2 movementValue = inputActionPlayerController.Move.ReadValue<Vector2>();
            this.PlayerCamera.transform.position = this.transform.position;    
            movementValue *= AddMovementForce;
            
            if (movementValue.x != 0 || movementValue.y != 0)
            {
                MovePlayer(movementValue);
            }
        }

        /// <summary>
        /// methode that moves player on map
        /// </summary>
        private void MovePlayer(Vector2 movementValue)
        {
            if (!(this.GamePlayer.velocity.magnitude < this.MaxMovementSpeed))
            {
                return;
            }

            Vector3 direction = new Vector3(movementValue.x, 0, movementValue.y);
            Vector3 camDirection = this.PlayerCamera.transform.rotation * direction;
            float yVelocyty = this.GamePlayer.velocity.y;
            Vector3 targetDirection = new Vector3(camDirection.x, yVelocyty, camDirection.z);
     
            if (direction != Vector3.zero) { 
                this.transform.rotation = Quaternion.Slerp(
                                                           this.transform.rotation,
                                                           Quaternion.LookRotation(targetDirection),
                                                           Time.deltaTime * AddMovementForce
                                                          );
            }

            float forceToAdd = this.sprint == true ? CalculateMovementForce() + this.SprintSpeed : CalculateMovementForce();
            this.GamePlayer.velocity = targetDirection.normalized * forceToAdd; 
        }

        /// <summary>
        /// methode that calculates movement force 
        /// </summary>
        private float CalculateMovementForce()
        {
            float finalForce = AddMovementForce;

            if (this.isSpeedUpActive == true)
            {
                finalForce += this.BonusSpeedUp;
            }

            if (this.isSpeedDownActive == true)
            {
                finalForce -= this.BonusSpeedDown;
            }

            return finalForce;
        }
        
        /// <summary>
        /// methode that read camera movement input
        /// </summary>
        private void ReadCameraMovementInput()
        {
            if (Mathf.Approximately(Time.timeScale,0f) == true)
            {
                return;
            }
            
            Vector2 screenDimensions = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            Vector2 cameraMovementValue = inputActionPlayerController.Look.ReadValue<Vector2>();

            if (Mathf.Approximately(cameraMovementValue.x ,screenDimensions.x/2) && Mathf.Approximately(cameraMovementValue.y ,screenDimensions.y/2))
            {
                return;
            }
            
            if (cameraMovementValue.x != 0 || cameraMovementValue.y != 0)
            {
                RotateCamera(cameraMovementValue);
            }
        }
        
        /// <summary>
        /// methode that rotates cammera
        /// </summary>
        /// <param name="cameraMovementValue"></param>
        private void RotateCamera(Vector2 cameraMovementValue)
        {
            Vector3 mousePointer = PlayerCamera.ScreenToViewportPoint(new Vector3(cameraMovementValue.x, cameraMovementValue.y, PlayerCamera.nearClipPlane));
            
            mousePointer.x -= CameraSped;
            mousePointer.y -= CameraSped;
            mousePointer.x *= CameraSensibility;
            mousePointer.y *= CameraSensibility;
            mousePointer.x += CameraSped;
            mousePointer.y += CameraSped;
                
            Vector3 screenPointer = PlayerCamera.ViewportToScreenPoint(mousePointer);
            Vector3 v = PlayerCamera.ScreenToWorldPoint(screenPointer);
            PlayerCamera.transform.LookAt(v, Vector3.up);
        }
        
        /// <summary>
        /// methode that fires players weapon
        /// </summary>
        private void PlayerFire()
        {
            FireEvents.PlayerFire?.Invoke();
        }

        /// <summary>
        /// methode that makes player jump
        /// </summary>
        /// <param name="obj"></param>
        private void PlayerJump(InputAction.CallbackContext obj)
        {
            if (this.canJump)
            {
                this.GamePlayer.AddForce(Vector3.up*500f);
                canJump = false;
            }
        }
        
        /// <summary>
        /// methode that changes player weapon type to next type
        /// </summary>
        private void ChangeToNextWeapon(InputAction.CallbackContext obj)
        {
            this.ChangeWeaponEvents.ChangeWeaponType?.Invoke(true);
        }
        
        /// <summary>
        /// methode that changes player weapon type to previous type
        /// </summary>
        private void ChangeToPreviousWeapon(InputAction.CallbackContext obj)
        {
            this.ChangeWeaponEvents.ChangeWeaponType?.Invoke(false);
        }
        
        #endregion
        
    }
}
