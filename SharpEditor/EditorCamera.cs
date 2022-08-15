using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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
        public Vector3 Position { get; private set; }
        public Vector3 FocalPoint { get; private set; }

        public float Distance { get; private set; }


        public Vector3 Rotation { get; private set; }
        private Vector2 m_ViewportSize = new Vector2(1280, 720);

        public Quaternion Orientation => Quaternion.Conjugate(Quaternion.FromEulerAngles(Rotation.X, Rotation.Y, 0));
        public Vector3 Up => Orientation * Vector3.UnitY;
        public Vector3 Right => Orientation * Vector3.UnitX;
        public Vector3 Forward => Orientation * -Vector3.UnitZ;

        public EditorCamera(float fov, float aspectRatio, float near, float far, float width, float height) : base(fov, aspectRatio, near, far)
        {
            Distance = 10;

            UpdateProjectionMatrix();
        }

        private void UpdateViewMatrix()
        {
            Position = FocalPoint - Forward * Distance;

            View = Matrix4.CreateFromQuaternion(Orientation) * Matrix4.CreateTranslation(Position);
            View = Matrix4.Invert(View);
        }

        private void UpdateProjectionMatrix()
        {
            AspectRatio = m_ViewportSize.X / m_ViewportSize.Y;
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio, Near, Far);
        }

        public void UpdateViewportSize(float width, float height)
        {
            m_ViewportSize.X = width;
            m_ViewportSize.Y = height;
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
            float x = Math.Min(m_ViewportSize.X / 1000.0f, 2.4f); // max = 2.4f
            float xFactor = 0.0366f * (x * x) - 0.1778f * x + 0.3021f;

            float y = Math.Min(m_ViewportSize.Y / 1000.0f, 2.4f); // max = 2.4f
            float yFactor = 0.0366f * (y * y) - 0.1778f * y + 0.3021f;

            FocalPoint += -Right * delta.X * xFactor * Distance;
            FocalPoint += Up * delta.Y  * yFactor * Distance;
        }

        private void MouseRotate(Vector2 delta)
        {
            float yawSign = Up.Y < 0 ? -1.0f : 1.0f;

            Rotation += new Vector3(delta.Y * 0.8f, delta.X * 0.8f * yawSign, 0);
        }

        void MouseZoom(float delta)
        {
            float distance = Distance * 0.2f;
            distance = Math.Max(distance, 0.0f);
            float speed = distance * distance;
            speed = Math.Min(speed, 100.0f); // max speed = 100

            Distance -= delta * speed;
            if (Distance < 1.0f)
            {
                FocalPoint += Forward;
                Distance = 1.0f;
            }
        }
    }
}
