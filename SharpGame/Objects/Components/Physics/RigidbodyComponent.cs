using BulletSharp;
using BulletSharp.Math;

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
        private RigidBody rigidBody;
        private CollisionShape collider;
        private MotionState motion;
        RigidBodyConstructionInfo info;

        public RigidbodyComponent(float mass)
        {
            this.Mass = mass;
        }

        public override void OnAwake()
        {
            this.collider = new CapsuleShape(3, 3);
            Vector3 intertia = collider.CalculateLocalInertia(this.Mass);
            Matrix trans = Matrix.Translation(this.Actor.PositionComponent.X, this.Actor.PositionComponent.Y, this.Actor.PositionComponent.Z) *
                Matrix.RotationX(this.Actor.RotationComponent.Pitch) *
                Matrix.RotationY(this.Actor.RotationComponent.Yaw) *
                Matrix.RotationZ(this.Actor.RotationComponent.Roll) *
                Matrix.Scaling(this.Actor.ScaleComponent.X, this.Actor.ScaleComponent.Y, this.Actor.ScaleComponent.Z);
            motion = new DefaultMotionState(trans);
            info = new RigidBodyConstructionInfo(this.Mass, motion, this.collider, intertia);
            rigidBody = new RigidBody(info);
            rigidBody.Translate(new Vector3(this.Actor.PositionComponent.X, this.Actor.PositionComponent.Y, this.Actor.PositionComponent.Z));
            this.Actor.RootScene.PhysicsSystem.AddRigidbody(rigidBody);
        }

        public override void OnUpdate(float deltaTime)
        {
            this.Actor.PositionComponent.Set(motion.WorldTransform.M41, motion.WorldTransform.M42, motion.WorldTransform.M43);
        }

        public override void OnShutdown()
        {
        }
    }
}
