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

using InControl;
using UnityEngine;


// This class is responsible for defining the Input Actions
// the player will have, and initialise InControl with them
//
public class PlayerActions : PlayerActionSet
{
	public PlayerAction Move_Left;
	public PlayerAction Move_Right;
	public PlayerAction Move_Up;
	public PlayerAction Move_Down;
	public PlayerTwoAxisAction GE_Move;

	public PlayerAction Fire_Left;
	public PlayerAction Fire_Right;
	public PlayerAction Fire_Up;
	public PlayerAction Fire_Down;
	public PlayerTwoAxisAction GE_Fire;

	public PlayerAction GE_Blink;
	public PlayerAction GE_Bomb;

	public PlayerAction UI_Pause;
	public PlayerAction UI_Back;
	public PlayerAction UI_Confirm;

	public PlayerAction UI_DevMenu;



	public PlayerActions()
	{
		Move_Left = CreatePlayerAction("Move Left");
		Move_Right = CreatePlayerAction("Move Right");
		Move_Up = CreatePlayerAction("Move Up");
		Move_Down = CreatePlayerAction("Move Down");
		GE_Move = CreateTwoAxisPlayerAction(Move_Left, Move_Right, Move_Down, Move_Up);

		Fire_Left = CreatePlayerAction("Fire Left");
		Fire_Right = CreatePlayerAction("Fire Right");
		Fire_Up = CreatePlayerAction("Fire Up");
		Fire_Down = CreatePlayerAction("Fire Down");
		GE_Fire = CreateTwoAxisPlayerAction(Fire_Left, Fire_Right, Fire_Down, Fire_Up);

		GE_Blink = CreatePlayerAction("Ship blink");
		GE_Bomb = CreatePlayerAction("Bomb");

		UI_Pause = CreatePlayerAction("Pause");
		UI_Back = CreatePlayerAction("Back");
		UI_Confirm = CreatePlayerAction("Confirm");

		UI_DevMenu = CreatePlayerAction("DevMenu");

		SetDefaultBindings();
	}



	public void SetDefaultBindings()
	{
		Debug.Log("Setting input default bindings...");

		// Movement
		//
		this.Move_Left.ClearBindings();
		this.Move_Left.AddDefaultBinding(Key.A);
		this.Move_Left.AddDefaultBinding(InputControlType.LeftStickLeft);

		this.Move_Right.ClearBindings();
		this.Move_Right.AddDefaultBinding(Key.D);
		this.Move_Right.AddDefaultBinding(InputControlType.LeftStickRight);

		this.Move_Up.ClearBindings();
		this.Move_Up.AddDefaultBinding(Key.W);
		this.Move_Up.AddDefaultBinding(InputControlType.LeftStickUp);

		this.Move_Down.ClearBindings();
		this.Move_Down.AddDefaultBinding(Key.S);
		this.Move_Down.AddDefaultBinding(InputControlType.LeftStickDown);


		// Fire
		//
		this.Fire_Left.ClearBindings();
		this.Fire_Left.AddDefaultBinding(Key.LeftArrow);
		this.Fire_Left.AddDefaultBinding(InputControlType.RightStickLeft);

		this.Fire_Right.ClearBindings();
		this.Fire_Right.AddDefaultBinding(Key.RightArrow);
		this.Fire_Right.AddDefaultBinding(InputControlType.RightStickRight);

		this.Fire_Up.ClearBindings();
		this.Fire_Up.AddDefaultBinding(Key.UpArrow);
		this.Fire_Up.AddDefaultBinding(InputControlType.RightStickUp);

		this.Fire_Down.ClearBindings();
		this.Fire_Down.AddDefaultBinding(Key.DownArrow);
		this.Fire_Down.AddDefaultBinding(InputControlType.RightStickDown);


		// Action buttons
		//
		this.GE_Blink.ClearBindings();
		this.GE_Blink.AddDefaultBinding(Key.Space);
		this.GE_Blink.AddDefaultBinding(InputControlType.RightBumper);

		this.GE_Bomb.ClearBindings();
		this.GE_Bomb.AddDefaultBinding(Key.E);
		this.GE_Bomb.AddDefaultBinding(InputControlType.LeftBumper);


		// UI Specific, will not be rebound...
		//
		this.UI_Pause.ClearBindings();
		this.UI_Pause.AddDefaultBinding(Key.Escape);
		this.UI_Pause.AddDefaultBinding(InputControlType.Start);
		this.UI_Pause.AddDefaultBinding(InputControlType.Pause);
		this.UI_Pause.RepeatDelay = 9999;
		this.UI_Pause.FirstRepeatDelay = 9999;

		this.UI_Back.ClearBindings();
		this.UI_Back.AddDefaultBinding(Key.Backspace);
		this.UI_Back.AddDefaultBinding(InputControlType.Back);
		this.UI_Back.AddDefaultBinding(InputControlType.Action2);
		this.UI_Back.RepeatDelay = 9999;
		this.UI_Back.FirstRepeatDelay = 9999;

		this.UI_Confirm.ClearBindings();
		this.UI_Confirm.AddDefaultBinding(Key.Return);
		this.UI_Confirm.AddDefaultBinding(InputControlType.Action1); 
		this.UI_Confirm.RepeatDelay = 9999;
		this.UI_Confirm.FirstRepeatDelay = 9999;

		this.UI_DevMenu.ClearBindings();
		this.UI_DevMenu.AddDefaultBinding(InputControlType.DPadUp);
	}



	public string GetCurrentBindingsString()
	{
		return this.Save();
	}



	public void LoadBindingsFromString(string sBindings)
	{
		this.Load(sBindings);
	}



	public void SaveCurrentBindings()
	{
		PlayerPrefs.SetString("InputBindings", this.Save());
		PlayerPrefs.Save();
	}
}
