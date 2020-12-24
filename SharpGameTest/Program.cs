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

namespace SharpGameTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SharpGameWindow window = new SharpGameWindow(1270, 720, "SharpGame");
            Shader lit = new Shader("lit");
            Shader unlit = new Shader("unlit");

            Scene scene = new Scene();

            Texture missing = new Texture("missing");

            Actor field = new Actor();
            field.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("scene"), missing, lit));

            Actor rocket = new Actor();
            rocket.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("cannon"), missing, unlit));
            rocket.AddComponent(new EpicComponent());

            Actor camera = new Actor();
            camera.AddComponent(new CameraComponent(70f, 0.5f, 100f));
            camera.AddComponent(new PlayerControlledComponent());
            camera.AddComponent(new AnotherComponent());

            scene.AddActor(camera);
            scene.AddActor(field);
            scene.AddActor(rocket);

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
