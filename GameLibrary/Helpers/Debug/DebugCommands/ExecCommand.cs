#if WINDOWS

namespace GameLibrary.Helpers.Debug.DebugCommands
{
    public class ExecCommand : DebugCommand
    {
        public ExecCommand(DebugConsole console)
            : base("ex", "Executes C# cod e in the current scope.",
            null, 1, "ex <C#-Expression>\n C#-Expression: var S = scope;")
        {
            this._Console = console;
            this._Execute = Run;
        }

        public void Run(World world, params object[] args)
        {
            if (args != null && args.Length > 0)
            { //arg based
                string code = "";
                foreach (string arg in args)
                    code += arg + " ";
                ScriptRuntime.RunScope(code, _Console.Scope);
            }
        }

        private DebugConsole _Console;
    }
}
#endif