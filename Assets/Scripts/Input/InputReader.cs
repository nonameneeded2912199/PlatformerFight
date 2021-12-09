using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : DescriptionBaseSO, PlayerInputAction.IGameplayActions, PlayerInputAction.IDialoguesActions, PlayerInputAction.IUIActions
{
	[Space]
	[SerializeField] private GameStateSO _gameStateManager;

	// Assign delegate{} to events to initialise them with an empty delegate
	// so we can skip the null check when we use them

	// Gameplay
	public event UnityAction<Vector2> MoveEvent = delegate { };
	public event UnityAction JumpEvent = delegate { };
	public event UnityAction JumpCanceledEvent = delegate { };
	public event UnityAction NormalAttackEvent = delegate { };
	public event UnityAction NormalAttackCanceledEvent = delegate { };
	public event UnityAction Skill1Event = delegate { };
	public event UnityAction Skill2Event = delegate { };
	public event UnityAction Skill3Event = delegate { };
	public event UnityAction Skill4Event = delegate { };
	public event UnityAction DashEvent = delegate { };
	//public event UnityAction InteractEvent = delegate { }; // Used to talk, pickup objects, interact with tools like the cooking cauldron
	//public event UnityAction InventoryActionButtonEvent = delegate { };
	public event UnityAction SaveActionButtonEvent = delegate { };
	public event UnityAction ResetActionButtonEvent = delegate { };
	//public event UnityAction<Vector2, bool> CameraMoveEvent = delegate { };
	public event UnityAction EnableMouseControlCameraEvent = delegate { };
	public event UnityAction DisableMouseControlCameraEvent = delegate { };

	// Shared between menus and dialogues
	public event UnityAction MoveSelectionEvent = delegate { };

	// Dialogues
	public event UnityAction AdvanceDialogueEvent = delegate { };

	// Menus
	public event UnityAction UIMouseMoveEvent = delegate { };
	public event UnityAction UIClickButtonEvent = delegate { };
	public event UnityAction UIUnpauseEvent = delegate { };
	public event UnityAction UIPauseEvent = delegate { };
	public event UnityAction UICancelEvent = delegate { };
	public event UnityAction<float> TabSwitched = delegate { };

	// Cheats (has effect only in the Editor)
	public event UnityAction CheatMenuEvent = delegate { };

	private PlayerInputAction _gameInput;

	private void OnEnable()
	{
		if (_gameInput == null)
		{
			_gameInput = new PlayerInputAction();

			_gameInput.UI.SetCallbacks(this);
			_gameInput.Gameplay.SetCallbacks(this);
			_gameInput.Dialogues.SetCallbacks(this);
		}
	}

	private void OnDisable()
	{
		DisableAllInput();
	}

	#region Gameplay

	public void OnMove(InputAction.CallbackContext context)
	{
		MoveEvent.Invoke(context.ReadValue<Vector2>());
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			JumpEvent.Invoke();

		if (context.phase == InputActionPhase.Canceled)
			JumpCanceledEvent.Invoke();
	}

	public void OnNormalATK(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				NormalAttackEvent.Invoke();
				break;
			case InputActionPhase.Canceled:
				NormalAttackCanceledEvent.Invoke();
				break;
		}
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			DashEvent.Invoke();
	}

	public void OnSkill1(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				Skill1Event.Invoke();
				break;
			case InputActionPhase.Canceled:
				Skill1Event.Invoke();
				break;
		}
	}

	public void OnSkill2(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				Skill2Event.Invoke();
				break;
			case InputActionPhase.Canceled:
				Skill2Event.Invoke();
				break;
		}
	}

	public void OnSkill3(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				Skill3Event.Invoke();
				break;
			case InputActionPhase.Canceled:
				Skill3Event.Invoke();
				break;
		}
	}

	public void OnSkill4(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				Skill4Event.Invoke();
				break;
			case InputActionPhase.Canceled:
				Skill4Event.Invoke();
				break;
		}
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			UIPauseEvent.Invoke();
	}

	#endregion

	#region UI

	public void OnMoveSelection(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			MoveSelectionEvent.Invoke();
	}

	public void OnClick(InputAction.CallbackContext context)
	{

	}

	public void OnSubmit(InputAction.CallbackContext context)
	{

	}

	public void OnPoint(InputAction.CallbackContext context)
	{

	}

	public void OnRightClick(InputAction.CallbackContext context)
	{

	}

	public void OnMiddleClick(InputAction.CallbackContext context)
	{

	}

	public void OnNavigate(InputAction.CallbackContext context)
	{

	}


	public void OnCancel(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			UICancelEvent.Invoke();

		Debug.LogWarning("Kike");
	}

	public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {

    }

	public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {

    }

	public void OnScrollWheel(InputAction.CallbackContext context)
    {

    }

	public void OnConfirm(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			UIClickButtonEvent.Invoke();		
	}

	public void OnUnpause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			UIUnpauseEvent.Invoke();
	}

	#endregion

	public void OnSaveActionButton(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			SaveActionButtonEvent.Invoke();
	}

	public void OnResetActionButton(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			ResetActionButtonEvent.Invoke();
	}

    //private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

    #region Dialogues

    public void OnAdvanceDialogue(InputAction.CallbackContext context)
	{

		if (context.phase == InputActionPhase.Performed)
			AdvanceDialogueEvent.Invoke();
	}

    #endregion

	public void EnableDialogueInput()
	{
		_gameInput.UI.Enable();
		_gameInput.Gameplay.Disable();
		_gameInput.Dialogues.Enable();
	}

	public void EnableGameplayInput()
	{
		_gameInput.UI.Disable();
		_gameInput.Dialogues.Disable();
		_gameInput.Gameplay.Enable();
	}

	public void EnableUIInput()
	{
		_gameInput.Dialogues.Disable();
		_gameInput.Gameplay.Disable();

		_gameInput.UI.Enable();
	}

	public void DisableAllInput()
	{
		_gameInput.Gameplay.Disable();
		_gameInput.UI.Disable();
		_gameInput.Dialogues.Disable();
	}

	public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;

	public void OnChangeTab(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			TabSwitched.Invoke(context.ReadValue<float>());
	}
}
