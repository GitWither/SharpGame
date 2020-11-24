using SharpGame;
using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Objects.Components;

using System;

namespace SharpGameTest
{
    class TestActor : Actor
    {
        float speed;
        public TestActor(float speed)
        {
            this.speed = speed;
        }
        public override void OnAwake()
        {
            base.OnAwake();
            Random random = new Random();
            AddComponent(new MeshRendererComponent(Mesh.FromOBJ("test")));
            GetComponent<PositionComponent>().Translate(random.Next(5), random.Next(5), random.Next(5));
        }
        public override void OnUpdate()
        {
        }
    }
}
