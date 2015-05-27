using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public PlayerIndex Index;
    private PlayerInputConfig InputConfig;

    void Start()
    {
        InputConfig = new PlayerInputConfig(Index);
    }
	
	void FixedUpdate ()
    {
		var drivingState = new DrivingState();

        float horizontalInput = Input.GetAxis(InputConfig.HorizontalInput);
        float verticalInput = Input.GetAxis(InputConfig.VerticalInput);
        float boostInput = Input.GetAxis(InputConfig.BoostInput);

		drivingState.Forward = Mathf.Clamp(verticalInput, 0, 1);
		drivingState.Backward = -Mathf.Clamp(verticalInput, -1, 0);
		drivingState.Turn = horizontalInput;
		drivingState.Boost = boostInput > float.Epsilon;
		
		GetComponentInChildren<VehicleMotor>().ChangeState(drivingState);
	}

    static public int GetNumberOfGamepads()
    {
        string[] gamepads = Input.GetJoystickNames();
        if (gamepads.Length == 1 && gamepads[0] == "")
            return 0;

        return gamepads.Length;
    }

    static public PlayerIndex GeneratePlayerIndex(int playerNumber)
    {
        int numberOfGamepads = GetNumberOfGamepads();

        return playerNumber <= numberOfGamepads
            ? (PlayerIndex)((int)PlayerIndex.GamepadOne + playerNumber - 1)
            : (PlayerIndex)((int)PlayerIndex.KeyboardOne + playerNumber - numberOfGamepads - 1);
    }

    private struct PlayerInputConfig
    {
        public string HorizontalInput { get; private set; }
        public string VerticalInput { get; private set; }
        public string BoostInput { get; private set; }

        const string KeyboardKeyword = "Keyboard";
        const string GamepadKeyword = "Gamepad";

        public PlayerInputConfig(PlayerIndex index)
            : this()
        {
            string type = "";
            int id = 0;

            switch (index)
            {
                case PlayerIndex.KeyboardOne:
                    type = KeyboardKeyword;
                    id = 1;
                    break;
                case PlayerIndex.KeyboardTwo:
                    type = KeyboardKeyword;
                    id = 2;
                    break;
                case PlayerIndex.GamepadOne:
                    type = GamepadKeyword;
                    id = 1;
                    break;
                case PlayerIndex.GamepadTwo:
                    type = GamepadKeyword;
                    id = 2;
                    break;
                case PlayerIndex.GamepadThree:
                    type = GamepadKeyword;
                    id = 3;
                    break;
                case PlayerIndex.GamepadFour:
                    type = GamepadKeyword;
                    id = 4;
                    break;
            }

            HorizontalInput = string.Format("Horizontal{0}{1}", type, id);
            VerticalInput = string.Format("Vertical{0}{1}", type, id);
            BoostInput = string.Format("Boost{0}{1}", type, id);
        }
    }
}
