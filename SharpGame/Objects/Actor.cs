using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics.Meshes;
using SharpGame.Objects.Components;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects
{
    public class Actor
    {
        public Scene RootScene { get; set; }

        private Component[] components = new Component[SharedConstants.MaxComponents];
        public PositionComponent PositionComponent { get; private set; }

        public T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < SharedConstants.MaxComponents; i++)
            {
                if (this.components[i] is T)
                {
                    return (T)this.components[i];
                }
            }
            return default;
        }
        
        public T AddComponent<T>(T component) where T : Component
        {
            for (int i = 0; i < SharedConstants.MaxComponents; i++)
            {
                if (this.components[i] == null)
                {
                    this.components[i] = component;
                    component.Actor = this;
                    return component;
                }
            }
            return default;
        }
        public bool HasComponent<T>() where T : Component
        {
            for (int i = 0; i < SharedConstants.MaxComponents; i++)
            {
                if (this.components[i] is T)
                {
                    return true;
                }
            }
            return false;
        }
        public void RemoveComponent<T>(T component) where T : Component
        {
            for (int i = 0; i < SharedConstants.MaxComponents; i++)
            {
                if (this.components[i] == component)
                {
                    this.components[i] = null;
                    component.OnShutdown();
                    break;
                }
            }
        }
        public virtual void OnAwake()
        {
            this.PositionComponent = this.AddComponent(new PositionComponent(0, 0, 0));
            this.AddComponent(new RotationComponent(0, 0, 0));

            for (int i = 0; i < SharedConstants.MaxComponents; i++)
            {
                components[i]?.OnAwake();
            }
        }

        public virtual void OnUpdate()
        {
            for (int i = 0; i < SharedConstants.MaxComponents; i++)
            {
                components[i]?.OnUpdate();
            }
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnShutdown()
        {
            for (int i = 0; i < SharedConstants.MaxComponents; i++)
            {
                components[i]?.OnShutdown();
            }
        }

        public virtual void OnDraw()
        {
            /*
            MeshRendererComponent mesh = this.GetComponent<MeshRendererComponent>();
            if (mesh != null)
            {
                mesh.Mesh.Draw();
            }
            */
        }
    }
}
