using System;
using System.IO;
using System.Runtime.InteropServices;

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
        public delegate void DInitialize();
        public delegate IntPtr DGetLanguage();
        public delegate bool DSetLanguage(IntPtr lang);

        public static DGetLibraryVersion GetLibraryVersion;
        public static DInitialize Initialize;
        public static DGetLanguage GetLanguage;
        public static DSetLanguage SetLanguage;

        private static IntPtr LibraryModule;

        /// <summary>
        /// Instruct the program to load the core library into memory using Win32 APIs.
        /// </summary>
        public static void Load()
        {
            LibraryModule = LoadLibrary(Path.Combine(Path.GetTempPath(), "libServerEase.dll"));
            GetLibraryVersion = Marshal.GetDelegateForFunctionPointer<DGetLibraryVersion>(GetProcAddress(LibraryModule, "GetLibraryVersion"));
            Initialize = Marshal.GetDelegateForFunctionPointer<DInitialize>(GetProcAddress(LibraryModule, "Initialize"));
            GetLanguage = Marshal.GetDelegateForFunctionPointer<DGetLanguage>(GetProcAddress(LibraryModule, "GetLanguage"));
            SetLanguage = Marshal.GetDelegateForFunctionPointer<DSetLanguage>(GetProcAddress(LibraryModule, "SetLanguage"));
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
