﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SharpGame.Input;
using SharpGame.Objects.Components;
using SharpGame.Scripting;
using SharpGame.Util;

namespace PlaygroundProject.Behaviors
{
    public class TestBehavior : ActorBehavior
    {
        public int CoolField;

        public void Test()
        {
            Logger.Info("this is coming from the loaded DLL!");
        }

        public override void OnAwake()
        {
            Logger.Info("Called from onawake (reloaded)!!!");
        }

        public override void OnUpdate()
        {

            ref TransformComponent transform = ref this.GetComponent<TransformComponent>();
            if (InputSystem.GetKey(KeyCode.W))
            {
                transform.Position = Vector3.Add(transform.Position, Vector3.UnitZ);
            }
            if (InputSystem.GetKey(KeyCode.S))
            {
                transform.Position = Vector3.Add(transform.Position, -Vector3.UnitZ);
            }
            if (InputSystem.GetKey(KeyCode.A))
            {
                transform.Position = Vector3.Add(transform.Position, Vector3.UnitX);
            }
            if (InputSystem.GetKey(KeyCode.D))
            {
                transform.Position = Vector3.Add(transform.Position, -Vector3.UnitX);
            }
        }

        public override void OnSleep()
        {
            Logger.Info("going to sleep");
        }
    }
}
