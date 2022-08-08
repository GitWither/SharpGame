using SharpGame;

namespace SharpEditor
{
    internal class SharpEditor : SharpGameWindow
    {
        public SharpEditor(int width, int height, string title) : base(width, height, title)
        {
            PushLayer(new SharpEditorLayer());
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            SharpEditor editor = new SharpEditor(1920, 1080, "SharpEditor");
            editor.Run();
        }
    }
}