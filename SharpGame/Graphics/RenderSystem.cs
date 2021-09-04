using OpenTK.Audio.OpenAL;
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

namespace SharpGame.Graphics
{
	public class RenderSystem : IDisposable
	{
		private readonly Layer[] layers;

		private SkybockVertexArrayObject skybockVertexArrayObject;

		public RenderSystem()
		{
			layers = new Layer[]
			{
				new Layer(),
				new Layer()
			};

			GL.Enable(EnableCap.Multisample);
			GL.Enable(EnableCap.LineSmooth);
			GL.Enable(EnableCap.PolygonSmooth);
			GL.Enable(EnableCap.StencilTest);
		}

		~RenderSystem()
		{
			this.Dispose();
		}

		internal void AddActor(Actor actor)
		{
			//TODO: Implemented a proper layering system
			layers[0].Add(actor);
		}

		internal void RemoveActor(Actor actor)
		{
			//TODO: Implemented a proper layering system
			layers[0].Remove(actor);
		}

		public void SetSkyboxMaterial(SkyboxMaterial skyboxMaterial)
		{
			if (skyboxMaterial != null)
			{
				skybockVertexArrayObject = new SkybockVertexArrayObject();
				skybockVertexArrayObject.SetSkyboxMaterial(skyboxMaterial);
				skybockVertexArrayObject.Upload();
			}
			else
			{
				skybockVertexArrayObject = null;
			}
		} 

		public void Render()
		{
			GL.BlendFunc(BlendingFactor.Zero, BlendingFactor.Zero);
			GL.DepthFunc(DepthFunction.Less);
			GL.Enable(EnableCap.CullFace);
			GL.Enable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Blend);

			//Iterate through all world-placed objects. 
			layers[(int)LayerType.WorldLayer].Render();

			GL.Disable(EnableCap.CullFace);
			GL.DepthFunc(DepthFunction.Lequal);

			skybockVertexArrayObject?.Render();

			//Iterate through all GUI objects. This is so they render on top of everything.
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			layers[(int)LayerType.GUILayer].Render();
		}

		public void Dispose()
		{
			for (int i = 0; i < layers.Length; i++)
            {
				layers[i].Dispose();
            }
		}
	}
}
