﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NyaFs.Filesystem.Universal.Items
{
    public class SymLink : FilesystemItem
    {
        public string Target;

        public SymLink(string Filename, uint User, uint Group, uint Mode, string Target) : base(Types.FilesystemItemType.SymLink, Filename, User, Group, Mode)
        {
            this.Target = (Target == null) ? "" : Target;
        }

        public override string ToString()
        {
            return $"LINK {Filename} {User}:{Group} {Mode:x03} => {Target}";
        }
        public override long Size => Target?.Length ?? 0;
    }
}
