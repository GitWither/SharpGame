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
		private readonly List<VertexArrayObject> worldObjects;
		private readonly List<VertexArrayObject> guiObjects;
		private SkybockVertexArrayObject skybockVertexArrayObject;

		public RenderSystem()
		{
			worldObjects = new List<VertexArrayObject>();
			guiObjects = new List<VertexArrayObject>();

			GL.Enable(EnableCap.Multisample);
			GL.Enable(EnableCap.LineSmooth);
			GL.Enable(EnableCap.PolygonSmooth);
			GL.Enable(EnableCap.StencilTest);
		}

		~RenderSystem()
		{
			this.Dispose();
		}

		internal void AddVertexArrayObject(VertexArrayObject vao)
		{
			//TODO: Implemented a proper layering system
			vao.Upload();

			switch (vao.LayerType)
			{
				case LayerType.WorldLayer:
					worldObjects.Add(vao);
					break;
				case LayerType.GUILayer:
					guiObjects.Add(vao);
					break;
			}
		}

		internal void RemoveVertexArrayObject(VertexArrayObject vao)
		{
			//TODO: Implemented a proper layering system

			switch (vao.LayerType)
			{
				case LayerType.WorldLayer:
					worldObjects.Remove(vao);
					break;
				case LayerType.GUILayer:
					guiObjects.Remove(vao);
					break;
			}
			vao.Dispose();
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
			GL.DepthFunc(DepthFunction.Less);
			GL.Enable(EnableCap.CullFace);
			GL.Enable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Blend);

			//Iterate through all world-placed objects. 
			for (int i = 0; i < worldObjects.Count; i++)
			{
				worldObjects[i].Render();
			}

			GL.Disable(EnableCap.CullFace);
			GL.DepthFunc(DepthFunction.Lequal);

			skybockVertexArrayObject?.Render();

			//Iterate through all GUI objects. This is so they render on top of everything.
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			for (int i = 0; i < guiObjects.Count; i++)
			{
				guiObjects[i].Render();
			}
		}

		public void Dispose()
		{
			guiObjects.Clear();
			worldObjects.Clear();
		}
	}
}
