﻿using SharpGame;
using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Objects.Components;
using SharpGame.Input;

using System;

namespace SharpGameTest
{
    class TestActor : Actor
    {
        float speed;
        Random random = new Random();
        public TestActor(float speed)
        {
            this.speed = speed;
        }
        public override void OnAwake()
        {
            base.OnAwake();
            //AddComponent(new MeshRendererComponent(Mesh.FromOBJ("test")));
            GetComponent<PositionComponent>().Set(0, 5, 5 );
        }
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (InputSystem.GetKeyDown(KeyCode.Z))
            {
                GetComponent<PositionComponent>().Set(0, 50, 5);
            }
            //speed += .01f;
            //GetComponent<RotationComponent>().Set((float)(Math.Cos(speed) * 5f), ((float)((float)(Math.Cos(speed) * 5f) * (Math.Sin(speed) * 5f))), (float)(Math.Sin(speed) * 5f));
            //GetComponent<PositionComponent>().Set((float)(Math.Cos(speed) * 5f), 0, (float)(Math.Sin(speed) * 5f));
        }
    }
}
