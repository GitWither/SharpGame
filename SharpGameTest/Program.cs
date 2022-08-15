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
using OpenTK.Mathematics;

namespace SharpGameTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SharpGameWindow window = new SharpGameWindow(1270, 780, "SharpGame");

            Scene scene = new Scene();


            //Sound sound = new Sound(@"C:\Users\Daniel\Desktop\Minecraft Bedrock Launcher\Minecraft-1.16.210.50\data\resource_packs\vanilla_music\sounds\music\game\creative\creative1.ogg");
            Texture missing = new Texture("missing");
            Texture main = new Texture("download");
            Texture buffaloTxt = new Texture("buffalo");
            Texture font = new Texture("ExportedFont");
            Texture rocketTxt = new Texture("rocket");
            Texture greenRing = new Texture("BaseMap");

            Mesh rocketMesh = Mesh.FromOBJ("cannon");

            Mesh dragon = Mesh.FromOBJ("dragon");


            Mesh simpleCube = Mesh.FromOBJ("simple_cube");


            Material rocks = new Material(Shader.Unlit, new Texture("rock"), new Texture("rock_normals"), null, 0.5f);
            Material unlit = new Material(Shader.Unlit, buffaloTxt);
            /*
            Actor field = scene.CreateActor();
            TransformComponent transform = new TransformComponent
            {
                Scale = Vector3.One
            };
            field.AddComponent(transform);
            field.AddComponent(new MeshComponent(Mesh.FromOBJ("terrain"), rocks));

            Actor rocket = scene.CreateActor();
            rocket.AddComponent(new TransformComponent(Vector3.Zero, Vector3.Zero, Vector3.One));
            rocket.AddComponent(new MeshComponent(Mesh.FromOBJ("cannon"), new Material(Shader.Unlit, rocketTxt)));

            Actor buffalo = scene.CreateActor();
            buffalo.AddComponent(new TransformComponent(Vector3.Zero, Vector3.Zero, Vector3.One));
            //buffalo.ScaleComponent.Set(1f, 0.01f, 0.01f);
            buffalo.AddComponent(new MeshComponent(Mesh.FromOBJ("dragon"), new Material(Shader.Lit, missing, 1f)));
            //buffalo.AddComponent(new AudioSourceComponent(sound, 1, 1, 15, true));
            //buffalo.AddComponent(new PointLightComponent(new Vector3(1f, 1f, 1f)));

            Actor light = scene.CreateActor();
            light.AddComponent(new PointLightComponent(new Vector3(0f, 1f, 0), 2f));
            light.AddComponent(new MeshComponent(Mesh.FromOBJ("cube"), new Material(Shader.Unlit, missing)));

            Actor light2 = scene.CreateActor();
            light2.AddComponent(new PointLightComponent(new Vector3(1f, 0.0f, 0), 2f));
            light2.AddComponent(new MeshComponent(Mesh.FromOBJ("cube"), new Material(Shader.Unlit, missing)));


            Actor text = scene.CreateActor();
            text.AddComponent(new GuiTextComponent("Hello world", font));

            Actor ring = scene.CreateActor();
            ring.AddComponent(new MeshComponent(Mesh.FromOBJ("ring"), new Material(Shader.Lit, greenRing, null, new Texture("EmmissionMap"))));

            Actor camera = scene.CreateActor();
            camera.AddComponent(new TransformComponent(new Vector3(0, 5, 15), Vector3.Zero, Vector3.One));
            camera.AddComponent(new CameraComponent(70f, 16 / 9f, 0.1f, 1000f));
            camera.AddComponent(new PlayerControlledComponent());

            Actor particles = scene.CreateActor();
            particles.AddComponent(new ParticleEmitterComponent(15, 0.1f, 150, new Material(Shader.Particle, missing)));
            */

            window.Run();
        }
    }
}
