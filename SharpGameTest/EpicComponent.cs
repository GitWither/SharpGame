using SharpGame.Input;
using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Graphics;
using SharpGame.Util;

namespace SharpGameTest
{
    class EpicComponent : Component
    {
        Random random = new Random();
        Mesh travis = Mesh.FromOBJ("dragon_big");
        Texture travisTxt = new Texture("T_Cyclone_Body_D.tga");
        public override void OnAwake()
        {
            this.Actor.PositionComponent.Set(0, 15, 0);
            this.Actor.RotationComponent.Set(25, 0, 0);
        }
        public override void OnUpdate(float deltaTime)
        {
            this.Actor.RotationComponent.Rotate(0, 25 * deltaTime, 0);
            
            if (InputSystem.GetKeyDown(KeyCode.C))
            {
                Actor cube = new Actor();
                cube.PositionComponent.Set(random.Next(-15, 15), 250, random.Next(-15, 15));
                cube.AddComponent(new RigidbodyComponent(55f, 2));
                cube.AddComponent(new MeshRendererComponent(travis, new Material(Shader.Unlit, travisTxt, 5)));
                //cube.AddComponent(new DeathComponent());

                //MeshRendererComponent mrc = this.Actor.GetComponent<MeshRendererComponent>();

                this.Actor.RootScene.AddActor(cube);

                //Logger.Info(this.Actor.RootScene.GetActorsByComponent<MeshRendererComponent>().Length);
            }
        }
    }
}
