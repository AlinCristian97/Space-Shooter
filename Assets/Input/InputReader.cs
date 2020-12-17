using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputActions.IGameplayActions
{
	// Gameplay
	public event UnityAction attackEvent;
	public event UnityAction attackCanceledEvent;
	public event UnityAction<Vector2> moveEvent;
	public event UnityAction restartEvent;

	private PlayerInputActions _playerInputActions;

	private void OnEnable()
	{
		if (_playerInputActions == null)
		{
			_playerInputActions = new PlayerInputActions();
			_playerInputActions.Gameplay.SetCallbacks(this);
		}

		_playerInputActions.Gameplay.Enable();
	}

	private void OnDisable()
	{
		_playerInputActions.Gameplay.Disable();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if (moveEvent != null)
		{
			moveEvent.Invoke(context.ReadValue<Vector2>());
		}
	}

    public void OnFire(InputAction.CallbackContext context)
    {
		if (attackEvent != null
			&& context.phase == InputActionPhase.Performed)
			attackEvent.Invoke();

		if (attackCanceledEvent != null
			&& context.phase == InputActionPhase.Canceled)
			attackCanceledEvent.Invoke();
	}

    public void OnRestart(InputAction.CallbackContext context)
    {
		if (restartEvent != null
			&& context.phase == InputActionPhase.Performed)
			restartEvent.Invoke();
    }
}
