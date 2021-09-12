using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Input
{
    public static class InputSystem
    {
        private static KeyboardState keyboardState = null;
        private static MouseState mouseState = null;

        public static void Init(KeyboardState ks, MouseState ms)
        {
            keyboardState = ks;
            mouseState = ms;
        }

        /// <summary>
        /// Checks whether a key is held down
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <returns>Returns true if a key is held down</returns>
        public static bool GetKeyDown(KeyCode key)
        {
            return keyboardState.IsKeyDown((Keys)key);
        }

        public static bool GetKeyUp(KeyCode key)
        {
            return keyboardState.IsKeyReleased((Keys)key);
        }

        public static bool GetKey(KeyCode key)
        {
            return keyboardState.IsKeyReleased((Keys)key) || keyboardState.IsKeyDown((Keys)key);
        }

        public static Vector2 GetMouseDelta()
        {
            return mouseState.Delta;
        }
    }
}
