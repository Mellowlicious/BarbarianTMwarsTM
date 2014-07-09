using System;

namespace BarbarianTMwarsTM
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BW game = new BW())
            {
                game.Run();
            }
        }
    }
#endif
}

