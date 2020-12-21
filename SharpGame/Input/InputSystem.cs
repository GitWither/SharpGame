using OpenTK;
using OpenTK.Input;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Input
{
    public static class InputSystem
    {
        private static KeyboardState keyboardState;
        private static MouseState mouseState = Mouse.GetState();

        /// <summary>
        /// Checks whether a key is held down
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <returns>Returns true if a key is held down</returns>
        public static bool GetKeyDown(KeyCode key)
        {
            keyboardState = Keyboard.GetState();
            return keyboardState.IsKeyDown((Key)key);
        }

        public static bool GetKeyUp(KeyCode key)
        {
            keyboardState = Keyboard.GetState();
            return keyboardState.IsKeyUp((Key)key);
        }

        public static bool GetKey(KeyCode key)
        {
            keyboardState = Keyboard.GetState();
            return keyboardState.IsKeyUp((Key)key) || keyboardState.IsKeyDown((Key)key);
        }

        public static Vector2 GetMouseAxis()
        {
            MouseState newMouseState = Mouse.GetState();

            float Xvalue = mouseState.X - newMouseState.X;
            float Yvalue = newMouseState.Y - mouseState.Y;

            mouseState = newMouseState;

            return new Vector2(Xvalue, Yvalue);
        }
    }
}
