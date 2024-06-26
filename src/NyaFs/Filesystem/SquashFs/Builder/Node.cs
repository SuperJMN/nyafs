﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NyaFs.Filesystem.SquashFs.Builder
{
    class Node
    {
        public MetadataRef Ref = new MetadataRef(0, 0);

        public Types.SqInodeType Type;
        public string Path;
        public uint User;
        public uint Group;
        public uint Mode;

        public uint Index = 0;

        /// <summary>
        /// User index in id table
        /// </summary>
        public uint UId;

        /// <summary>
        /// Group iondex in id table
        /// </summary>
        public uint GId;

        public Node(Types.SqInodeType Type, string Path, uint User, uint Group, uint Mode)
        {
            this.Type = Type;
            this.Path = Path;
            this.User = User;
            this.Group = Group;
            this.Mode = Mode;
        }

        public virtual Types.SqInode GetINode() => null;
    }
}
