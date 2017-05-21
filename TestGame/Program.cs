using System;

namespace TestGame
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TestGame())
                game.Run();
        }
    }
#endif
}
