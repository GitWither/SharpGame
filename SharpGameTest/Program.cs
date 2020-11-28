﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpGame;
using SharpGame.Objects;
using SharpGame.Graphics.Meshes;
using OpenTK;
using System.Drawing;
using SharpGame.Objects.Components;

namespace SharpGameTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SharpGameWindow window = new SharpGameWindow(1270, 720, "SharpGame");

            Scene scene = new Scene();
            scene.AddActor(new TestActor(0.0f));

            Actor camera = new Actor();
            camera.AddComponent(new CameraComponent(70f, 0.01f, 100f));
            camera.AddComponent(new PlayerControlledComponent());

            scene.AddActor(camera);

            window.LoadScene(scene);

            #if DEBUG
            Point point =  window.Location;
            point.X -= 1920;
            window.Location = point;
            #endif
            
            window.Run();
        }
    }
}
