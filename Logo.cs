using System;
using System.Threading;

namespace CyberSecurityChatBot
{
    public class Logo
    {
        public static void Display()
        {
            Console.Clear();

            int width = Console.WindowWidth; // Get console width for centering

            // =================== Helper Function ===================
            void WriteCentered(string text, ConsoleColor color = ConsoleColor.White)
            {
                int padding = (width - text.Length) / 2;
                padding = padding < 0 ? 0 : padding; // Prevent negative padding
                Console.ForegroundColor = color;
                Console.WriteLine(new string(' ', padding) + text);
                Console.ResetColor();
            }

            // Top Border
            WriteCentered(new string('=', width), ConsoleColor.DarkRed);

            // KANYA
            WriteCentered(@"██╗  ██╗ █████╗ ███╗   ██╗██╗   ██╗ █████╗ ", ConsoleColor.DarkRed);
            WriteCentered(@"██║ ██╔╝██╔══██╗████╗  ██║╚██╗ ██╔╝██╔══██╗", ConsoleColor.Red);
            WriteCentered(@"█████╔╝ ███████║██╔██╗ ██║ ╚████╔╝ ███████║", ConsoleColor.DarkRed);
            WriteCentered(@"██╔═██╗ ██╔══██║██║╚██╗██║  ╚██╔╝  ██╔══██║", ConsoleColor.Red);
            WriteCentered(@"██║  ██╗██║  ██║██║ ╚████║   ██║   ██║  ██║", ConsoleColor.DarkRed);
            WriteCentered(@"╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝", ConsoleColor.Red);

            Console.WriteLine();

            // SHIELD
            WriteCentered(@" ███████╗██╗  ██╗██╗███████╗██╗     ██████╗ ", ConsoleColor.White);
            WriteCentered(@" ██╔════╝██║  ██║██║██╔════╝██║     ██╔══██╗", ConsoleColor.DarkRed);
            WriteCentered(@" ███████╗███████║██║█████╗  ██║     ██║  ██║", ConsoleColor.Red);
            WriteCentered(@" ╚════██║██╔══██║██║██╔══╝  ██║     ██║  ██║", ConsoleColor.DarkRed);
            WriteCentered(@" ███████║██║  ██║██║███████╗███████╗██████╔╝", ConsoleColor.Red);
            WriteCentered(@" ╚══════╝╚═╝  ╚═╝╚═╝╚══════╝╚══════╝╚═════╝ ", ConsoleColor.DarkRed);

            Console.WriteLine();

            // Subtitle
            WriteCentered("🛡️ CYBERSECURITY AWARENESS CHATBOT 🛡️", ConsoleColor.Yellow);
            Console.WriteLine();

            // Catchy Slogan
            WriteCentered("\"Defending Your Digital World, One Click at a Time!\"", ConsoleColor.Cyan);
            Console.WriteLine();

            // Created by
            WriteCentered("Created By KANYA KAPO DEV", ConsoleColor.Green);
            Console.WriteLine();

            // Bottom Border
            WriteCentered(new string('=', width), ConsoleColor.DarkRed);
            Console.WriteLine();

            // Loading Animation
            LoadingText("Loading KANYA SHIELD", width);
            LoadingText("Starting secure systems", width);
            LoadingText("Launching Cybersecurity Assistant", width);

            WriteCentered("System Ready!", ConsoleColor.Green);
            Thread.Sleep(1000);

            // Logo stays on screen until user exits
        }

        // ================= LOADING DOTS =================
        private static void LoadingText(string message, int width)
        {
            int padding = (width - message.Length - 3) / 2; // 3 dots
            padding = padding < 0 ? 0 : padding;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(new string(' ', padding) + message);
            Console.ResetColor();

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }

            Console.WriteLine();
        }
    }
}