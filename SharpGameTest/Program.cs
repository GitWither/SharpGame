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
using OpenTK.Graphics.OpenGL4;

namespace SharpGameTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SharpGameWindow window = new SharpGameWindow(1270, 780, "SharpGame");

            Scene scene = new Scene();

            Texture missing = new Texture("missing");
            Texture main = new Texture("download");
            Texture buffaloTxt = new Texture("buffalo");
            Texture font = new Texture("ExportedFont");
            Texture rocketTxt = new Texture("rocket");

            Material rocks = new Material(Shader.Lit, new Texture("rock"), new Texture("rock_normals"));

            Actor field = new Actor();
            field.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("scene"), rocks));

            Actor rocket = new Actor();
            rocket.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("cannon"), new Material(Shader.Unlit, rocketTxt)));
            rocket.AddComponent(new EpicComponent());

            Actor buffalo = new Actor();
            buffalo.ScaleComponent.Set(5f, 5f, 5f);
            buffalo.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("buffalo"), new Material(Shader.Unlit, buffaloTxt)));
            //buffalo.AddComponent(new PointLightComponent(new Vector3(1f, 1f, 1f)));

            Actor light = new Actor();
            light.PositionComponent.Set(0, 15, 0);
            light.AddComponent(new PointLightComponent(new Vector3(0f, 1f, 0)));
            light.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("cube"), new Material(Shader.Unlit, missing)));
            light.AddComponent(new CoolComponent(1, -1));

            Actor light2 = new Actor();
            light2.PositionComponent.Set(0, 15, 0);
            light2.AddComponent(new PointLightComponent(new Vector3(1f, 0.0f, 0)));
            light2.AddComponent(new MeshRendererComponent(Mesh.FromOBJ("cube"), new Material(Shader.Unlit, missing)));
            light2.AddComponent(new CoolComponent(-1, 1));

            Actor testLol = new Actor();
            testLol.AddComponent(new GuiTextureComponent(main));

            Actor text = new Actor();
            //text.AddComponent(new GuiTextComponent("penis", font));
            //text.AddComponent(new CoolTestComponent());
            text.ScaleComponent.Set(0.05f, 0.07f, 1);
            text.PositionComponent.Set(-20, 13, 0);

            Actor camera = new Actor();
            camera.AddComponent(new CameraComponent(70f, 16/9f, 0.1f, 100f));
            camera.AddComponent(new PlayerControlledComponent());
            camera.PositionComponent.Set(0, 10, -15);

            scene.AddActor(buffalo);
            scene.AddActor(camera);
            scene.AddActor(field);
            scene.AddActor(light2);
            scene.AddActor(rocket);
            scene.AddActor(text);
            scene.AddActor(testLol);
            scene.AddActor(light);

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
