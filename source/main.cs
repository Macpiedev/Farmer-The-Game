using System;

namespace ProjektPO
{
    public static class main
    {
        /// <summary>
        /// Main function.
        /// </summary>
        static void Main()
        {
            using (var game = new GameManager())
                game.Run();
        }
    }
}
