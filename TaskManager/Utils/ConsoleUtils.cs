using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Utils
{
    public static class ConsoleUtils
    {
        private const int MF_BYCOMMAND = 0x00000000;
        private const int SC_CLOSE = 0xF060;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_SIZE = 0xF000;

        private static readonly ConsoleColor BackgroundColor = Console.BackgroundColor;
        private static readonly ConsoleColor ForegroundColor = Console.ForegroundColor;


        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [SupportedOSPlatform("windows")]
        private static void SetWindowSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height + 1);
        }

        [SupportedOSPlatform("windows")]
        public static void CreateWindow(int width, int height)
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                // DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
            SetWindowSize(width, height);
            Console.CursorVisible = false;
        }

        public static void SetColors(ConsoleColor background, ConsoleColor foreground)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }

        public static void SetColors(ConsoleColor background)
        {
            SetColors(background, ForegroundColor);
        }

        public static void SetColors()
        {
            SetColors(BackgroundColor, ForegroundColor);
        }
    }
}