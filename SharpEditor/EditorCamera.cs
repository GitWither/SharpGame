using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SharpGame.Graphics;
using SharpGame.Input;
using SharpGame.Util;

namespace SharpEditor
{
    public class EditorCamera : Camera
    {
        private Vector3 m_Position = Vector3.Zero;
        public Vector3 m_FocalPoint = Vector3.Zero;

        private float m_Distance = 10f;

        private float m_Pitch, m_Yaw;
        private Vector2 m_Viewport = new Vector2(1280, 720);

        public Quaternion Orientation => new(new Vector3(-m_Pitch, -m_Yaw, 0));
        public Vector3 Up => Orientation * Vector3.UnitY;
        public Vector3 Right => Orientation * Vector3.UnitX;
        public Vector3 Forward => Orientation * -Vector3.UnitZ;

        public EditorCamera(float fov, float aspectRatio, float near, float far) : base(fov, aspectRatio, near, far)
        {
            UpdateViewMatrix();
            UpdateProjectionMatrix();
        }

        private void UpdateViewMatrix()
        {
            m_Position = m_FocalPoint - Forward * m_Distance;

            //View = Matrix4.LookAt(m_Position, Forward, Up);
            View = Matrix4.CreateTranslation(m_Position) * Matrix4.CreateFromQuaternion(Orientation);
            View.Invert();
        }

        private void UpdateProjectionMatrix()
        {
            AspectRatio = m_Viewport.X / m_Viewport.Y;
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio, Near, Far);
        }

        public void UpdateViewportSize(float width, float height)
        {
            this.m_Viewport = new Vector2(width, height);
            UpdateProjectionMatrix();
        }

        public void OnUpdate(float deltaTime)
        {
            Vector2 delta = InputSystem.GetMouseDelta() * 0.003f;

            if (InputSystem.GetMouseButtonDown(MouseCode.ButtonLeft))
            {
                MousePan(delta);
            }
            else if (InputSystem.GetMouseButtonDown(MouseCode.ButtonRight))
            {
                MouseRotate(delta);
            }
            else if (InputSystem.GetMouseButtonDown(MouseCode.ButtonMiddle))
            {
                MouseZoom(delta.Y);
            }

            UpdateViewMatrix();
        }

        public void OnMouseScroll(float yOffset)
        {
            float delta = yOffset * 0.1f;
            MouseZoom(delta);
            UpdateViewMatrix();
        }

        private void MousePan(Vector2 delta)
        {
            float x = Math.Min(m_Viewport.X / 1000.0f, 2.4f); // max = 2.4f
            float xFactor = 0.0366f * (x * x) - 0.1778f * x + 0.3021f;

            float y = Math.Min(m_Viewport.Y / 1000.0f, 2.4f); // max = 2.4f
            float yFactor = 0.0366f * (y * y) - 0.1778f * y + 0.3021f;

            m_FocalPoint += -Right * delta.X * xFactor * m_Distance;
            m_FocalPoint += Up * delta.Y  * yFactor * m_Distance;
        }

        private void MouseRotate(Vector2 delta)
        {
            Logger.Info($"{delta}");
            float yawSign = Up.Y < 0 ? -1.0f : 1.0f;
            m_Pitch += delta.Y * 0.8f;
            m_Yaw += yawSign * delta.X * 0.8f;
        }

        void MouseZoom(float delta)
        {
            float distance = m_Distance * 0.2f;
            distance = Math.Max(distance, 0.0f);
            float speed = distance * distance;
            speed = Math.Min(speed, 100.0f); // max speed = 100

            m_Distance -= delta * speed;
            if (m_Distance < 1.0f)
            {
                m_FocalPoint += Forward;
                m_Distance = 1.0f;
            }
        }
    }
}
