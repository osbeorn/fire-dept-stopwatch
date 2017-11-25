using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using utils;

namespace video.hosting
{
    internal static class Program
    {
        private static string controllerUrl;

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.WriteError("Unhandled exception was caught: " + e.ExceptionObject);
            Process.GetCurrentProcess().Kill();
        }

        [STAThread]
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomain_UnhandledException);
            try
            {
                controllerUrl = CommandLineArgs.Parse(args).GetParamAsString("controller-url");
            }
            catch (Exception exception)
            {
                log.WriteError(exception);
                return;
            }
            try
            {
                log.WriteInfo("connecting to controller...");
                IHostController controller = RemotingServices.Connect(typeof(IHostController), controllerUrl) as IHostController;
                if (controller != null)
                {
                    log.WriteInfo("sending hello to controller...");
                    Action<IHostController> action = controller.Hello();
                    log.WriteInfo("executing action returned by controller...");
                    action(controller);
                    log.WriteInfo("sending bye to controller...");
                    controller.Bye();
                }
                else
                {
                    log.WriteError("failed to connect to controller...");
                }
            }
            catch (Exception exception2)
            {
                log.WriteError(exception2);
            }
            log.WriteInfo("host process terminated....");
        }

        [DllImport("kernel32.dll")]
        private static extern UnhandledExceptionHandler SetUnhandledExceptionFilter(UnhandledExceptionHandler lpTopLevelExceptionFilter);

        private delegate uint UnhandledExceptionHandler(IntPtr ExceptionPointers);
    }
}

