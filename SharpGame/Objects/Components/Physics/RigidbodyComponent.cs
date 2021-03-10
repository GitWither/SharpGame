using BulletSharp;

using OpenTK;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class RigidbodyComponent : Component
    {
        public float Mass { get; set; }
        float size;
        private RigidBody rigidBody;
        private CollisionShape collider;
        private MotionState motion;
        private RigidBodyConstructionInfo info;

        public RigidbodyComponent(float mass, float size)
        {
            this.Mass = mass;
            this.size = size;
        }

        public override void OnAwake()
        {
            this.collider = new SphereShape(size);
            Vector3 intertia = collider.CalculateLocalInertia(this.Mass);
            motion = new DefaultMotionState(MathUtil.CreateTransformationMatrix(this.Actor));
            info = new RigidBodyConstructionInfo(this.Mass, motion, this.collider, intertia);
            rigidBody = new RigidBody(info);
            this.Actor.RootScene.PhysicsSystem.AddRigidbody(rigidBody);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (motion != null)
            {
                Vector3 position = motion.WorldTransform.ExtractTranslation();
                Vector3 rotation = motion.WorldTransform.ExtractRotation().ToAxisAngle().Xyz;
                Vector3 scale = motion.WorldTransform.ExtractScale();
                this.Actor.PositionComponent.Set(position.X, position.Y, position.Z);
                this.Actor.ScaleComponent.Set(scale.X, scale.Y, scale.Z);
                this.Actor.RotationComponent.Set(rotation.X, rotation.Y, rotation.Z);
            }
        }

        public override void OnShutdown()
        {
            this.Actor.RootScene.PhysicsSystem.RemoveRigidbody(rigidBody);
            rigidBody.Dispose();
            collider.Dispose();
            motion.Dispose();
            info.Dispose();
        }
    }
}
