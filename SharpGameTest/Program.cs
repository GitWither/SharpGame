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
            //Shader lit = new Shader("lit");
            //Shader unlit = new Shader("unlit");
            //Shader fun = new Shader("baby_yoda");

            Scene scene = new Scene();

            Texture missing = new Texture("missing");
            Texture main = new Texture("download");

            Actor field = new Actor();
            field.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("scene"), missing, Shader.Lit));

            Actor rocket = new Actor();
            rocket.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("cannon"), missing, Shader.Unlit));
            rocket.AddComponent(new EpicComponent());

            Actor testLol = new Actor();
            testLol.AddComponent(new GuiTextureComponent(main));
            //testLol.ScaleComponent.Set(0.5f, 0.5f, 1);

            Actor camera = new Actor();
            camera.AddComponent(new CameraComponent(70f, 0.5f, 100f));
            camera.AddComponent(new PlayerControlledComponent());
            camera.AddComponent(new AnotherComponent());

            scene.AddActor(camera);
            scene.AddActor(field);
            scene.AddActor(rocket);
            scene.AddActor(testLol);

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
