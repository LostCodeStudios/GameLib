#if WINDOWS
using GameLibrary.Helpers.Debug.DebugCommands;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameLibrary.Helpers.Debug
{
    public class DebugConsole 
    {
        public DebugConsole(World world, params DebugCommand[] commands)
        {
            _World = world;
            _Running = false;
            Commands = new DebugCommandManager(_World, commands);
            FreeConsole();
        }

        #region Functioning Loop

        public void BuildDefaultCommands()
        {
            //Help
            Commands.Add(new DebugCommand(
                "help", "Prints a detailed list of commands and their descriptions.",
                (world, args) =>
                {
                    if (args != null && args.Length > 0)
                    { //Usage
                        Console.WriteLine(
                            Commands.FirstOrDefault((c) => c.Command.Equals((string)args[0], StringComparison.OrdinalIgnoreCase))
                            .Info());
                    }
                    else //List of all commands
                    {
                        Console.WriteLine("For more information on a specific command, type \"help <command>\"");
                        Commands.Print();
                    }
                }));

            //Log

            //"ex" - Executes C#
            Commands.Add(new ExecCommand(this));

            //"cd" - Changes the scope of the variable printer.
            Commands.Add(new ChangeScopeCommand(this));

            //"dir" - Prints all variables in current scope.
            Commands.Add(new DisplayScopeCommand(this));
        }

        /// <summary>
        /// Starts the debug console
        /// </summary>
        /// <returns>If the console started successfully, true</returns>
        public bool Start()
        {
            if (_Running != true)
                if (this.CreateConsole())
                {
                    //Commands
                    Scope = _World;
                    BuildDefaultCommands();

                    //Thread stuff
                    _Running = true;
                    _ConsoleThread = new
                        Thread(new ThreadStart(this.Run));
                    _ConsoleThread.IsBackground = true;
                    _ConsoleThread.Start();
                    return true;
                }

            return false;
        }

        private void Run()
        {
            Header();
            while (_Running)
            {
                Console.Title = "=====[" + _World.Name.ToUpper() + "]DEV CONSOLE=====";
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("World" + ScopeString);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("> ");
                Commands.Execute(Console.ReadLine());
            }
        }

        /// <summary>
        /// Stops the console from running
        /// </summary>
        /// <returns>If the console closed successfully, true</returns>
        public bool Stop()
        {
            if (_Running)
            {
                Commands.Clear();
                _ConsoleThread.Abort();
                if (FreeConsole())
                {
                    _Running = false;
                    return true;
                }
            }
            return false;
        }

        #endregion Functioning Loop

        #region Properties

        /// <summary>
        /// The scope string
        /// </summary>
        public string ScopeString
        {
            set
            {
                this.Scope = _World;
                if (value.Split('.').Length > 0)
                    for (int i = 0; i < value.Split('.').Length; ++i)
                        if (!string.IsNullOrEmpty(value.Split('.')[i]))
                            if (this.Scope.GetType().GetField(value.Split('.')[i]).GetValue(this.Scope) != null)
                                this.Scope = this.Scope.GetType().GetField(value.Split('.')[i]).GetValue(this.Scope);

                _ScopeString = value;
            }
            get
            {
                return _ScopeString;
            }
        }

        private string _ScopeString = "";

        /// <summary>
        /// If the console is running
        /// </summary>
        public bool Running
        {
            get
            {
                return _Running;
            }
        }

        /// <summary>
        /// Manages all the commands
        /// </summary>
        public DebugCommandManager Commands;

        #endregion Properties

        #region Fields

        private World _World;
        public object Scope;

        private bool _Running;
        private Thread _ConsoleThread;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Region runs through the console's Header
        /// </summary>
        public void Header()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("=====[" + _World.Name.ToUpper() + "]DEV CONSOLE=====");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Type \"help\" for a list of commands;\n To exit any command, press Ctrl+C.");
        }

        #region Console Extension

        #endregion Console Extension

        #region Win32

        internal OutputWriter OutWriter;
        internal OutputWriter ErrWriter;

        /// <summary>
        /// Allocates a console for this Win32 application
        /// </summary>
        /// <returns>If the console was allocated.</returns>
        public bool CreateConsole()
        {
            if (Win32.ConsoleLibrary.AllocConsole())
            {
                var _STDOUT = Win32.ConsoleLibrary.GetConsoleStandardOutput();
                var _STDIN = Win32.ConsoleLibrary.GetConsoleStandardInput();

                Win32.ConsoleLibrary.SetStdHandle(Win32.ConsoleLibrary.StdHandle.Output, _STDOUT);
                Win32.ConsoleLibrary.SetStdHandle(Win32.ConsoleLibrary.StdHandle.Input, _STDIN);

                //Console.SetOut(new StreamWriter(
                //    new Out, Console.OutputEncoding));

                Console.OpenStandardOutput();
                Console.OpenStandardInput();

                var tempout = Console.Out;
                OutWriter = new OutputWriter(tempout);
                Console.SetOut(OutWriter);


                ErrWriter = new OutputWriter(OutWriter, () => { Console.ForegroundColor = ConsoleColor.Red; Console.Write("ERR<< "); }, () => { });
                Console.SetError(ErrWriter);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Frees the app from dah console
        /// </summary>
        /// <returns></returns>
        private bool FreeConsole()
        {
            if (_Running)
            {
                Win32.DeleteFile("CONOUT$");
                Win32.DeleteFile("CONIN$");
                Win32.CloseHandle(Win32.ConsoleLibrary.GetStdHandle((int)Win32.ConsoleLibrary.StdHandle.Output));
                Win32.CloseHandle(Win32.ConsoleLibrary.GetStdHandle((int)Win32.ConsoleLibrary.StdHandle.Input));
                return Win32.ConsoleLibrary.FreeConsole();
            }
            return false;
        }

        #endregion Win32

        #endregion Methods
    }

    internal class OutputWriter : TextWriter
    {
        public OutputWriter(TextWriter stdOut, Action prefix, Action suffix)
        {
            STDOUTWriter = stdOut;
            this.prefix = prefix;
            this.suffix = suffix;
        }

        public OutputWriter(TextWriter stdOut)
            : this(stdOut, new Action(() => { }), new Action(() => { }))
        {
        }

        public override void Write(string value)
        {
            prefix();
            STDOUTWriter.Write(value);
            suffix();
        }

        public override void WriteLine(string s)
        {
            prefix();
            STDOUTWriter.WriteLine(s);
            suffix();
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        private Action prefix;
        private Action suffix;
        private TextWriter STDOUTWriter;
    }
}
#endif