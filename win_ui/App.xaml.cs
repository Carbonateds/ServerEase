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
            bool FirstLaunch = false;

            ExportInternalDLLFile("libServerEase.dll", "Panuon.WPF.dll", "Panuon.WPF.UI.dll");

            AppDomain.CurrentDomain.AssemblyResolve += (sender1, e1) =>
            {
                string assemblyName = new AssemblyName(e1.Name).Name;
                if (!assemblyName.Contains(".resource"))
                {
                    return Assembly.LoadFrom(Path.Combine(Path.GetTempPath(), assemblyName + ".dll"));
                }
                else return null;
            };

            Library.Load(); //Load the core library into memory, so the functions can be called.

            Trace.WriteLine("Current Library Version: " + Marshal.PtrToStringAnsi(Library.GetLibraryVersion()));

            try
            {
                FirstLaunch = Library.Initialize(); //Intialize configuration by invoking the core library function.

                Current.Resources.Clear();
                Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Panuon.WPF.UI;component/Control.xaml", UriKind.Absolute) });
                Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Language\\" + Marshal.PtrToStringAnsi(Library.GetLanguage()) + ".xaml", UriKind.Relative) });
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(1);
            }

            //If this is the first time the program has been launched and the current language is machine-translated, then pop up a warning box. 
            if (FirstLaunch && (bool)FindResource("MachineTranslate"))
                MessageBox.Show(FindResource("MachineTranslateWarning").ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);


        }

        /// <summary>
        /// Export internal DLL files into local temporary folder.
        /// </summary>
        /// <param name="dllName">File name of the DLL file.</param>
        /// <exception cref="FileLoadException"></exception>
        private void ExportInternalDLLFile(params string[] dllNames)
        {
            foreach (string dllName in dllNames)
            {
                string dllPath = Path.Combine(Path.GetTempPath(), dllName);
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
                if (!File.Exists(dllPath))
                    throw new FileLoadException();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Library.Free();
        }
    }
}
