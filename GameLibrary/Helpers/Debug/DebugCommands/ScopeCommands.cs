using System;
using System.Linq;
using System.Reflection;

namespace GameLibrary.Helpers.Debug.DebugCommands
{
    /// <summary>
    /// Changes the scope of a console
    /// </summary>
    public class ChangeScopeCommand : DebugCommand
    {
        public ChangeScopeCommand(DebugConsole console)
            : base("cd", "Changes the current scope.",
            null, 1, "cd <Scope(^ move up, * move in)>")
        {
            this._Execute = Run;
            this.console = console;
        }

        public void Run(World world, params object[] args)
        {
            if (args[0].Equals("^")) //move up a variable level
                console.ScopeString = console.ScopeString.TrimEnd(
                        (("." + console.ScopeString.Split('.').Last()).ToCharArray())
                    );
            else
                console.ScopeString += "." + args[0];
        }

        #region Fields

        private DebugConsole console;

        #endregion Fields
    }

    /// <summary>
    /// Displays the current scope.
    /// </summary>
    public class DisplayScopeCommand : DebugCommand
    {
        /// <summary>
        /// Creates a display scope command.
        /// </summary>
        public DisplayScopeCommand(DebugConsole console)
            : base("dir", "Displays all variables within the current scope.",
            null, 0,
            "dir <display-mode> (public/private)\n -Display-Modes: all,fields,properties,methods")
        {
            this.console = console;
            this._Execute = new ExecutionDelegate(this.Run);
        }

        public void Run(World world, params object[] args)
        {
            string displayMode = "vars";
            string protection = "private";

            #region Handle Arguments

            if (args != null)
            {
                //Display mode
                if (args.Length > 0)
                    displayMode = (string)args[0];

                //Protection
                if (args.Length > 1)
                    protection = (string)args[1];
            }

            #endregion Handle Arguments

            #region Process

            BindingFlags flags = BindingFlags.Instance;
            MemberInfo[] members;
            ConsoleColor memberColor = ConsoleColor.White;

            //Protection
            switch (protection)
            {
                case "private":
                    flags = flags | BindingFlags.NonPublic | BindingFlags.Public;
                    break;

                case "public":
                    flags = flags | BindingFlags.Public;
                    break;
            }

            //Display mode Display-Modes: all,fields,properties,methods
            members = console.Scope.GetType().GetMembers(flags)
                .Where(mi => mi.DeclaringType == typeof(World) || mi.DeclaringType == console.Scope.GetType()).ToArray();
            switch (displayMode)
            {
                case "all":
                    members = members.Where(x => x.MemberType == MemberTypes.All).ToArray();
                    break;

                case "fields":
                    members = members.Where(x => x.MemberType == MemberTypes.Field).ToArray();
                    memberColor = ConsoleColor.DarkCyan;
                    break;

                case "methods":
                    members = members.Where(x => x.MemberType == MemberTypes.Method).ToArray();
                    memberColor = ConsoleColor.DarkMagenta;
                    break;

                case "properties":
                    members = members.Where(x => x.MemberType == MemberTypes.Property).ToArray();
                    memberColor = ConsoleColor.DarkYellow;
                    break;

                default:
                    memberColor = ConsoleColor.DarkCyan;
                    members = members.Where(x => x.MemberType == MemberTypes.Property || x.MemberType == MemberTypes.Field).ToArray();
                    break;
            }

            #endregion Process

            #region Display

            Console.WriteLine(console.Scope.GetType() + " S{");
            Console.WriteLine(" -" + displayMode + "- ");

            //print
            foreach (var member in members)
            {
                //Protection
                if (console.Scope.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance).Contains(member))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(" [pu]");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" [pr]");
                }

                //Data/name
                Console.ForegroundColor = memberColor;
                if (member.MemberType == MemberTypes.Property)
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(member.Name);
                Console.ForegroundColor = ConsoleColor.Gray;
                if (member.MemberType == MemberTypes.Field)
                    Console.Write(": " + (member as FieldInfo).GetValue(console.Scope));
                else if (member.MemberType == MemberTypes.Property)
                    if ((member as PropertyInfo).CanRead)
                        Console.Write(": " + (member as PropertyInfo).GetValue(console.Scope, null));

                Console.WriteLine(" ");
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("};");

            #endregion Display
        }

        #region Fields

        private DebugConsole console;

        #endregion Fields
    }
}