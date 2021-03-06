using System;
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
using SharpGame.Graphics;
using OpenTK.Audio;
using SharpGame.Audio;
using OpenTK.Audio.OpenAL;
using SharpGame.Util;
using SharpGame.Physics;
using System.Runtime.InteropServices;

namespace SharpGameTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SharpGameWindow window = new SharpGameWindow(1270, 780, "SharpGame");

            Scene scene = new Scene();

            scene.RegisterPhysicsSystem(new PhysicsSystem());
            scene.RegisterRenderSystem(new RenderSystem());

            Sound sound = new Sound(@"C:\Users\Daniel\Desktop\Minecraft Bedrock Launcher\Minecraft-1.16.210.50\data\resource_packs\vanilla_music\sounds\music\game\creative\creative1.ogg");
            Texture missing = new Texture("missing");
            Texture main = new Texture("download");
            Texture buffaloTxt = new Texture("buffalo");
            Texture font = new Texture("ExportedFont");
            Texture rocketTxt = new Texture("rocket");
            Texture greenRing = new Texture("BaseMap");

            Mesh rocketMesh = Mesh.FromOBJ("cannon");

            Mesh dragon = Mesh.FromOBJ("dragon");


            Mesh simpleCube = Mesh.FromOBJ("simple_cube");

            SkyboxMaterial skybox = new SkyboxMaterial(new Cubemap("skybox/test"), Shader.Skybox);
            scene.SetSkyboxMaterial(skybox);

            Material rocks = new Material(Shader.Lit, new Texture("rock"), new Texture("rock_normals"), null, 0.5f);
            Material unlit = new Material(Shader.Unlit, buffaloTxt);

            Actor field = new Actor();
            field.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("scene"), rocks));
            //field.AddComponent(new RigidbodyComponent(0));

            Actor rocket = new Actor();
            rocket.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("cannon"), new Material(Shader.Unlit, rocketTxt)));
            rocket.AddComponent(new EpicComponent());

            Actor buffalo = new Actor();
            //buffalo.ScaleComponent.Set(1f, 0.01f, 0.01f);
            buffalo.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("dragon"), new Material(Shader.Lit, missing, 1f)));
            //buffalo.AddComponent(new AudioSourceComponent(sound, 1, 1, 15, true));
            //buffalo.AddComponent(new PointLightComponent(new Vector3(1f, 1f, 1f)));

            Actor light = new Actor();
            light.PositionComponent.Set(0, 15, 0);
            light.AddComponent(new PointLightComponent(new Vector3(0f, 1f, 0), 2));
            light.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("cube"), new Material(Shader.Unlit, missing)));
            light.AddComponent(new CoolComponent(1, -1));

            Actor light2 = new Actor();
            light2.PositionComponent.Set(0, 15, 0);
            light2.AddComponent(new PointLightComponent(new Vector3(1f, 0.0f, 0), 2));
            light2.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("cube"), new Material(Shader.Unlit, missing)));
            light2.AddComponent(new CoolComponent(-1, 1));

            Actor testLol = new Actor();
            testLol.AddComponent(new GuiTextureComponent(main));

            Actor text = new Actor();
            text.AddComponent(new GuiTextComponent("no", font));
            text.AddComponent(new CoolTestComponent());
            text.ScaleComponent.Set(0.05f, 0.07f, 1);
            text.PositionComponent.Set(-20, 13, 0);

            Actor ring = new Actor();
            ring.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("ring"), new Material(Shader.Lit, greenRing, null, new Texture("EmmissionMap"))));
            ring.ScaleComponent.Set(4, 4, 4);
            ring.PositionComponent.Set(0, 10, 0);

            Actor camera = new Actor();
            camera.AddComponent(new CameraComponent(70f, 16 / 9f, 0.1f, 1000f));
            camera.AddComponent(new PlayerControlledComponent());
            camera.PositionComponent.Set(0, 10, -15);

            Actor particles = new Actor();
            particles.AddComponent(new ParticleEmitterComponent(3, 2, 2, new Material(Shader.Particle, missing)));

            /*
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    Actor obj = new Actor();
                    obj.PositionComponent.Set(x * 15, y * 15, 0);
                    obj.AddComponent(new MeshRendererComponent(dragon, rocks));
                    scene.AddActor(obj);
                }
            }
            */

            scene.AddActor(buffalo);
            scene.AddActor(camera);
            scene.AddActor(field);
            scene.AddActor(light2);
            scene.AddActor(rocket);
            scene.AddActor(testLol);
            scene.AddActor(text);
            scene.AddActor(light);
            scene.AddActor(particles);
            scene.AddActor(ring);

            window.LoadScene(scene);

            #if DEBUG
            Point point = window.Location;
            point.X -= 1920;
            window.Location = point;
            #endif

            window.Run();
        }
    }
}
