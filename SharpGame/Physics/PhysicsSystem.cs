using OpenTK;

using SharpGame.Objects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Physics
{
    public class PhysicsSystem
    {
        /*
        public DiscreteDynamicsWorld World { get; set; }
        CollisionDispatcher dispatcher;
        DbvtBroadphase broadphase;

        CollisionConfiguration collisionConfiguration;
        private ConstraintSolver solver;
        */

        public void OnAwake()
        {
            /*
            collisionConfiguration = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConfiguration);
            broadphase = new DbvtBroadphase();
            solver = new SequentialImpulseConstraintSolver();
            World = new DiscreteDynamicsWorld(dispatcher, broadphase, solver, collisionConfiguration)
            {
                Gravity = new Vector3(0, -9.81f, 0)
            };
            */
        }

        public void AddRigidbody(/*RigidBody rigidBody*/)
        {
            /*
            World.AddRigidBody(rigidBody);
            */
        }

        public void OnUpdate(float deltaTime)
        {
            /*
            World.StepSimulation(deltaTime);
            */
        }

        public void RemoveRigidbody(/*RigidBody rigidBody*/)
        {
            /*
            World.RemoveRigidBody(rigidBody);
            */
        }

        public void OnShutdown()
        {
            /*
            World.Dispose();
            broadphase.Dispose();
            dispatcher.Dispose();
            collisionConfiguration.Dispose();
            */
        }
    }
}
