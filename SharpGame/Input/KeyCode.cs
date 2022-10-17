using OpenTK.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Input
{
    public enum KeyCode
    {
        W = 87,
        A = 65,
        S = 83,
        Z = 90,
        D = 68,
        C = 67,
        Space = 32,
        LeftShift = 340,

        UpArrow = 265,
        DownArrow = 264,
        LeftArrow = 263,
        RightArrow = 262,
    }

    public enum MouseCode
    {
        Button0 = 0,
        Button1 = 1,
        Button2 = 2,
        Button3 = 3,
        Button4 = 4,
        Button5 = 5,
        Button6 = 6,
        Button7 = 7,

        ButtonLast = Button7,
        ButtonLeft = Button0,
        ButtonRight = Button1,
        ButtonMiddle = Button2
    }
}
