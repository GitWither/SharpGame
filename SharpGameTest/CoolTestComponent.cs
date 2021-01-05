﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame;
using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGameTest
{
    class CoolTestComponent : Component
    {
        float frameTime;
        float fps;
        string fpstext = "FPS: 0";
        GuiTextComponent text;
        string gpu;
        public override void OnAwake()
        {
            text = this.Actor.GetComponent<GuiTextComponent>();
            gpu = GL.GetString(StringName.Renderer);
        }
        public override void OnUpdate(float deltaTime)
        {
            frameTime += deltaTime;
            fps++;
            if (frameTime >= 1)
            {
                fpstext = "FPS: " + fps.ToString();
                frameTime = 0;
                fps = 0;
            }
            text.Text = $"{ fpstext} Actors: {SharpGameWindow.ActiveScene.GetActorAmount()}\nGPU: {gpu}\nXYZ: {(Vector3)SharpGameWindow.ActiveScene.Camera.Actor.PositionComponent}";

            //text.Text = deltaTime.ToString();

            //this.Actor.RotationComponent.Rotate(0, 1, 0);
        }
    }
}
