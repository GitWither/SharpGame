﻿using SharpGame.Input;
using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGameTest
{
    class EpicComponent : Component
    {
        Random random = new Random();
        Mesh travis = Mesh.FromOBJ("cube");
        public override void OnAwake()
        {
            this.Actor.PositionComponent.Set(0, 15, 0);
            this.Actor.RotationComponent.Set(15, 0, 0);
        }
        public override void OnUpdate(float deltaTime)
        {
            this.Actor.RotationComponent.Rotate(0, 25 * deltaTime, 0);
            
            if (InputSystem.GetKeyDown(KeyCode.C))
            {
                Actor cube = new Actor();
                cube.AddComponent(new PhysicsComponent());
                cube.AddComponent(new MeshRendererComponent(travis));
                this.Actor.RootScene.AddActor(cube);
                cube.PositionComponent.Set(random.Next(-15, 15), 15, random.Next(-15, 15));
            }
        }
    }
}
