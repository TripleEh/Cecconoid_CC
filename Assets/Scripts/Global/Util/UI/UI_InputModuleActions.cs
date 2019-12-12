// Cecconoid by Triple Eh? Ltd -- 2019
//
// Cecconoid_CC is distributed under a CC BY 4.0 license. You are free to:
//
// * Share copy and redistribute the material in any medium or format
// * Adapt remix, transform, and build upon the material for any purpose, even commercially.
//
// As long as you give credit to Gareth Noyce / Triple Eh? Ltd.
//
//

using UnityEngine;
using InControl;


[RequireComponent(typeof(InControlInputModule))]
public class UI_InputModuleActions : MonoBehaviour
{
	public class InputModuleActions : PlayerActionSet
	{
		public PlayerAction Submit;
		public PlayerAction Cancel;
		public PlayerAction Left;
		public PlayerAction Right;
		public PlayerAction Up;
		public PlayerAction Down;
		public PlayerTwoAxisAction Move;


		public InputModuleActions()
		{
			Submit = CreatePlayerAction("Submit");
			Cancel = CreatePlayerAction("Cancel");
			Left = CreatePlayerAction("Move Left");
			Right = CreatePlayerAction("Move Right");
			Up = CreatePlayerAction("Move Up");
			Down = CreatePlayerAction("Move Down");
			Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
		}
	}

	protected InputModuleActions actions;

	void OnEnable()
	{
		CreateActions();

		var inputModule = GetComponent<InControlInputModule>();
		if (inputModule != null)
		{
			inputModule.SubmitAction = actions.Submit;
			inputModule.CancelAction = actions.Cancel;
			inputModule.MoveAction = actions.Move;
		}
	}

	void OnDestroy()
	{
		DestroyActions();
	}


	void OnDisable()
	{
		DestroyActions();
	}


	void CreateActions()
	{
		actions = new InputModuleActions();

		actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
		actions.Up.AddDefaultBinding(InputControlType.DPadUp);
		actions.Up.AddDefaultBinding(Key.UpArrow);

		actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
		actions.Down.AddDefaultBinding(InputControlType.DPadDown);
		actions.Down.AddDefaultBinding(Key.DownArrow);

		actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
		actions.Left.AddDefaultBinding(InputControlType.DPadLeft);
		actions.Left.AddDefaultBinding(Key.LeftArrow);

		actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
		actions.Right.AddDefaultBinding(InputControlType.DPadRight);
		actions.Right.AddDefaultBinding(Key.RightArrow);

		actions.Submit.AddDefaultBinding(InputControlType.Action1);
		actions.Submit.AddDefaultBinding(Key.Space);
		actions.Submit.AddDefaultBinding(Key.Return);

		actions.Cancel.AddDefaultBinding(InputControlType.Action2);
		actions.Cancel.AddDefaultBinding(Key.Escape);
	}

	void DestroyActions()
	{
		actions.Destroy();
	}


	public bool AnyActionButtonPressed()
	{
		return actions.Submit.WasPressed || actions.Cancel.WasPressed;
	}
}