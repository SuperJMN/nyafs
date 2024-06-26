﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NyaFs.Processor.Scripting.Variables
{
    public class VariableChecker
    {
        public static bool IsCorrectName(string Name)
        {
            return (Name != null) && (Name.Length > 2) && (Name.Count(C => (C == '$')) == 1) && (Name[0] == '$');
        }

        public static string[] ExtractVariables(string Text)
        {
            var Res = new List<string>();
            var Parts = Text.Split(new char[] { ' ', '\t', ',', '.', ':', '(', ')', '[', ']', '{', '}', '-', '\\', '/', '>', '<', '?', '!', '+', '*', '*', '%', '#', '@', '~' });

            foreach(var P in Parts)
            {
                if (IsCorrectName(P))
                    Res.Add(P);
            }

            return Res.ToArray();
        }
    }
}
