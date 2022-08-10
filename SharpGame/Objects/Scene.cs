using OpenTK.Mathematics;

using SharpGame.Events;
using SharpGame.Graphics;
using SharpGame.Objects.Components;
using SharpGame.Physics;
using SharpGame.Util;


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SharpActors;
using SharpGame.Input;
using SharpGame.Objects.Components.Transform;

namespace SharpGame.Objects
{
    public class Scene
    {
        public event EventHandler<ScreenEventArgs> ResolutionChanged;
        private readonly ActorRegistry m_ActorRegistry;

        internal ActorRegistry ActorRegistry => m_ActorRegistry;


        public CameraComponent Camera { get; set; }
        public Matrix4 Projection { get; set; }
        public Matrix4 View { get; set; }
        internal PointLightComponent[] PointLights { get; set; }
        public int ActorCount => m_ActorRegistry.Count;


        public bool Running { get; set; }


        internal RenderSystem RenderSystem { get; private set; }
        internal PhysicsSystem PhysicsSystem { get; private set; }


        public Scene()
        {
            Running = false;
            this.m_ActorRegistry = new ActorRegistry(SharedConstants.MaxActors);
            this.m_ActorRegistry.RegisterComponent<MeshComponent>();
            this.m_ActorRegistry.RegisterComponent<TransformComponent>();
            this.m_ActorRegistry.RegisterComponent<GuiTextComponent>();
            this.m_ActorRegistry.RegisterComponent<ParticleEmitterComponent>();
            this.m_ActorRegistry.RegisterComponent<CameraComponent>();
            this.m_ActorRegistry.RegisterComponent<PointLightComponent>();
            this.m_ActorRegistry.RegisterComponent<PlayerControlledComponent>();

            PointLights = new PointLightComponent[SharedConstants.MaxLights];
        }

        internal void OnAwake()
        {
            if (PhysicsSystem == null && RenderSystem == null)
            {
                Logger.Error("Render and physics systems have not been initialized. A register is required before using them!");
                return;
            }
            this.Running = true;
        }


        /// <summary>
        /// Registers a physics system.
        /// </summary>
        /// <param name="physicsSystem">An instance of a physics system</param>
        public void RegisterPhysicsSystem(PhysicsSystem physicsSystem)
        {
            if (!this.Running)
            {
                this.PhysicsSystem = physicsSystem;
                this.PhysicsSystem.OnAwake();
            }
            else
            {
                Logger.Error("Cannot register system while scene is running");
            }
        }

        /// <summary>
        /// Adds an actor to the scene
        /// </summary>
        /// <param name="actor">An instance of the actor to add to the scene</param>
        public Actor CreateActor()
        {
            return new Actor(m_ActorRegistry.CreateActor(), this);
            //ActorAdded?.Invoke(this, new ActorEventArgs(actor));
        }

        internal void OnRender()
        {
            ActorView cameras = m_ActorRegistry.CreateView<TransformComponent, CameraComponent>();
            foreach (int actor in cameras)
            {

                ref CameraComponent camera = ref m_ActorRegistry.GetComponent<CameraComponent>(actor);
                ref TransformComponent transform = ref m_ActorRegistry.GetComponent<TransformComponent>(actor);

                this.Projection = Matrix4.CreatePerspectiveFieldOfView(camera.FieldOfView, camera.AspectRatio,
                    camera.NearClipPlane, camera.FarClipPlane);

                float forwardX = (float)(Math.Sin(MathHelper.DegreesToRadians(transform.Rotation.Y)) * Math.Cos(MathHelper.DegreesToRadians(transform.Rotation.X)));
                float forwardY = (float)Math.Sin(MathHelper.DegreesToRadians(transform.Rotation.X));
                float forwardZ = (float)(Math.Cos(MathHelper.DegreesToRadians(transform.Rotation.Y)) * Math.Cos(MathHelper.DegreesToRadians(transform.Rotation.X)));

                Vector3 Forward = new Vector3(forwardX, forwardY, forwardZ);
                Vector3 Right = Vector3.Cross(Vector3.UnitY, Forward);
                Right.NormalizeFast();

                Vector3 delta = Vector3.Zero;

                float factor = 5;

                if (InputSystem.GetKeyDown(KeyCode.W))
                {
                    delta.Z += factor;
                }
                if (InputSystem.GetKeyDown(KeyCode.A))
                {
                    delta.X += factor;
                }
                if (InputSystem.GetKeyDown(KeyCode.S))
                {
                    delta.Z -= factor;
                }
                if (InputSystem.GetKeyDown(KeyCode.D))
                {
                    delta.X -= factor;
                }
                if (InputSystem.GetKeyDown(KeyCode.LeftShift))
                {
                    delta.Y -= factor;
                }
                if (InputSystem.GetKeyDown(KeyCode.Space))
                {
                    delta.Y += factor;
                }

                if (delta.LengthSquared > 0.0001f)
                {
                    transform.Position += new Vector3(((Right.X * delta.X) + (Forward.X * delta.Z)), delta.Y, ((Forward.Z * delta.Z) + (Right.Z * delta.X)));
                }

                Vector2 mouseDelta = InputSystem.GetMouseDelta() * 0.05f;
                transform.Rotation += new Vector3(-mouseDelta.Y, -mouseDelta.X, 0);

                this.View = Matrix4.LookAt(
                    transform.Position.X, transform.Position.Y, transform.Position.Z,
                    transform.Position.X + forwardX, transform.Position.Y + forwardY, transform.Position.Z + forwardZ,
                    0, 1, 0
                );
            }

            ActorView drawables = m_ActorRegistry.CreateView<TransformComponent, MeshComponent>();
            foreach (int actor in drawables)
            {
                ref MeshComponent mesh = ref m_ActorRegistry.GetComponent<MeshComponent>(actor);
                ref TransformComponent transform = ref m_ActorRegistry.GetComponent<TransformComponent>(actor);

                mesh.Mesh.Render(MathUtil.CreateTransformationMatrix(transform), mesh.Material);
            }
            //RenderSystem.Render();
        }

        internal void OnUpdate(float deltaTime)
        {
        }

        internal void OnShutdown()
        {
            this.Running = false;
            PhysicsSystem.OnShutdown();
        }

        /// <summary>
        /// Retuns an array of actors that have the component specified
        /// </summary>
        /// <typeparam name="T">Component to search for</typeparam>
        /// <returns></returns>

        /// <summary>
        /// Removes an actor from the scene
        /// </summary>
        /// <param name="actor">An instance of the actor to remove from the scene</param>
        public void RemoveActor(Actor actor)
        {
            //ActorRemoved?.Invoke(this, new ActorEventArgs(actor));
            //actor.OnShutdown();
            //actors.Remove(actor);
        }

        internal void ScreenResized(Vector2i newSize)
        {
            ResolutionChanged?.Invoke(this, new ScreenEventArgs(newSize));
        }
    }
}
