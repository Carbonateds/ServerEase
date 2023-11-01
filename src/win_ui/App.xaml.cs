using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace ServerEase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ExportInternalDLLFile("libServerEase.dll");

            Library.Load(); //Load the core library into memory, so the functions can be called.

            Trace.Write("Current Library Version: " + Marshal.PtrToStringAnsi(Library.GetLibraryVersion()));
        }

        /// <summary>
        /// Export internal DLL files into local temporary folder.
        /// </summary>
        /// <param name="dllName">File name of the DLL file.</param>
        /// <exception cref="FileLoadException"></exception>
        private void ExportInternalDLLFile(string dllName)
        {
            string dllPath = Path.Combine(Path.GetTempPath(), dllName);
            if(!File.Exists(dllPath))
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ServerEase." + dllName))
                {
                    if (stream != null)
                    {
                        using (FileStream fileStream = File.Open(dllPath, FileMode.Create))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }
            if(!File.Exists(dllPath))
                throw new FileLoadException();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Library.Free();
        }
    }
}
