using System.Diagnostics;
using System;

namespace Orbital_Cannons
{
    static class ModLog
    {
        //Formatting class, for easier readability in the log (copied from Dexter Feldoh's cookiecutter)
        [Conditional("DEBUG")]
        public static void Debug(string x)
        {
            Verse.Log.Message(x);
        }

        public static void Log(string msg)
        {
            Verse.Log.Message($"<color=#1c6beb>[OrbitalCannons]</color> {msg ?? "<null>"}");
        }

        public static void Warn(string msg)
        {
            Verse.Log.Warning($"<color=#1c6beb>[OrbitalCannons]</color> {msg ?? "<null>"}");
        }

        public static void Error(string msg, Exception e = null)
        {
            Verse.Log.Error($"<color=#1c6beb>[OrbitalCannons]</color> {msg ?? "<null>"}");
            if (e != null)
            {
                Verse.Log.Error(e.ToString());
            }
        }

    }
}