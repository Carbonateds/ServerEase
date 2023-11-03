using System;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

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

            Trace.WriteLine("Current Library Version: " + Marshal.PtrToStringAnsi(Library.GetLibraryVersion()));

            Library.Initialize(); //Intialize configuration by invoking the core library function.

            Current.Resources.Clear();
            Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Language\\" + Marshal.PtrToStringAnsi(Library.GetLanguage()) + ".xaml", UriKind.Relative) });
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
