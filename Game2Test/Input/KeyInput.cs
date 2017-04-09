using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game2Test.Input
{
    public static class KeyInput
    {
        private static KeyboardState _keyState;
        private static KeyboardState _oldKeyState;

        public static void Update(GameTime gameTime)
        {
            _oldKeyState = _keyState;
            _keyState = Keyboard.GetState();
        }

        public static bool IsKeyClicked(Keys key)
        {
            return _keyState.IsKeyDown(key) && !_oldKeyState.IsKeyDown(key);
        }

        public static bool EitherKeyDown(Keys key1, Keys key2)
        {
            return _keyState.IsKeyDown(key1) || _keyState.IsKeyDown(key2);
        }

        public static bool BothKeysDown(Keys key1, Keys key2)
        {
            return _keyState.IsKeyDown(key1) && _keyState.IsKeyDown(key2);
        }
    }
}
