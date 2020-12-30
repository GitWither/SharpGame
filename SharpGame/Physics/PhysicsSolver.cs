﻿using BulletSharp;

using OpenTK;

using SharpGame.Objects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Physics
{
    class PhysicsSolver
    {
        public DiscreteDynamicsWorld World { get; set; }
        CollisionDispatcher dispatcher;
        DbvtBroadphase broadphase;

        List<CollisionShape> collisionShapes = new List<CollisionShape>();
        CollisionConfiguration collisionConfiguration;

        public void OnAwake()
        {
            collisionConfiguration = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConfiguration);

            broadphase = new DbvtBroadphase();
            World = new DiscreteDynamicsWorld(dispatcher, broadphase, null, collisionConfiguration)
            {
                Gravity = new BulletSharp.Math.Vector3(0, -10, 0)
            };


        }

        public void AddActor(Actor actor)
        {
            //CollisionShape collisionShape 
        }

        public void OnUpdate(float deltaTime)
        {
            //World.StepSimulation(deltaTime);
        }

        public void OnShutdown()
        {

        }
    }
}