﻿using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics.Meshes;
using SharpGame.Graphics.Vaos;
using SharpGame.Objects;
using SharpGame.Objects.Components;
using SharpGame.Util;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpActors;

namespace SharpGame.Graphics
{
    internal class RenderSystem : ActorSystem
    {
        public void OnRender(ActorRegistry actorRegistry, Renderer renderer)
		{
            foreach (int actor in this.Actors)
            {
                ref MeshComponent mesh = ref actorRegistry.GetComponent<MeshComponent>(actor);
                ref TransformComponent transform = ref actorRegistry.GetComponent<TransformComponent>(actor);

                Mesh meshAsset = SharpGameWindow.Instance.AssetStorage.GetAsset<Mesh>(mesh.MeshAsset);
                Material material = SharpGameWindow.Instance.AssetStorage.GetAsset<Material>(mesh.MaterialAsset);

                renderer.DrawMesh(material, meshAsset, transform);
            }
        }

	}
}
