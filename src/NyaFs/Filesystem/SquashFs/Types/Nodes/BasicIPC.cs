﻿using System;
using System.Collections.Generic;
using System.Text;
using NyaIO.Data;

namespace NyaFs.Filesystem.SquashFs.Types.Nodes
{
    class BasicIPC : SqInode
    {
        public BasicIPC(SqInodeType Type, uint Mode, uint User, uint Group, uint LinkCount) : base(0x14)
        {
            InodeType = Type;
            Permissions = Mode;
            GidIndex = Group;
            UidIndex = User;

            HardLinkCount = LinkCount;
        }

        public BasicIPC(byte[] Data) : base(Data, 0x14)
        {

        }

        /// <summary>
        /// INode size
        /// </summary>
        internal override long INodeSize => 0x14; // TODO: add blocks_sizes calculation...

        /// <summary>
        /// The number of hard links to this ipc item
        /// u32 hard_link_count (0x10)
        /// </summary>
        public uint HardLinkCount
        {
            get { return ReadUInt32(0x10); }
            set { WriteUInt32(0x10, value); }
        }
    }
}
