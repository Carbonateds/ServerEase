using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerEase
{
    internal class Library
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpLibFileName);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hLibModule);

        public delegate IntPtr DGetLibraryVersion();

        public static DGetLibraryVersion GetLibraryVersion;

        private static IntPtr LibraryModule;

        /// <summary>
        /// Instruct the program to load the core library into memory using Win32 APIs.
        /// </summary>
        public static void Load()
        {
            LibraryModule = LoadLibrary(Path.Combine(Path.GetTempPath(), "libServerEase.dll"));
            GetLibraryVersion = Marshal.GetDelegateForFunctionPointer<DGetLibraryVersion>(GetProcAddress(LibraryModule, "GetLibraryVersion"));
        }

        /// <summary>
        /// Instrust the program to unload the core library from memory.
        /// </summary>
        public static void Free()
        {
            FreeLibrary(LibraryModule);
        }
    }
}
