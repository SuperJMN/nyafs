﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NyaFs.Filesystem.SquashFs.Types.Nodes
{
    class BasicDirectory : SqInode
    {
        public BasicDirectory(uint Mode, uint User, uint Group, uint DirBlockStart, uint DirBlockOffset, uint HardLinkCount, uint DirEntriesFullSize, uint ParentINodeNumber) : base(0x20)
        {
            InodeType = SqInodeType.BasicDirectory;
            Permissions = Mode;
            GidIndex = Group;
            UidIndex = User;

            this.DirBlockStart = DirBlockStart;

            FileSize = DirEntriesFullSize;
            BlockOffset = DirBlockOffset;
            this.HardLinkCount = HardLinkCount;
            this.ParentINodeNumber = ParentINodeNumber;
        }

        public BasicDirectory(byte[] Data) : base(Data, 0x20)
        {

        }

        /// <summary>
        /// INode size
        /// </summary>
        internal override long INodeSize => 0x20;

        /// <summary>
        /// The location of the of the block in the Directory Table where the directory entry information starts
        /// u32 dir_block_start (0x10)
        /// </summary>
        public uint DirBlockStart
        {
            get { return ReadUInt32(0x10); }
            set { WriteUInt32(0x10, value); }
        }

        /// <summary>
        /// The number of hard links to this directory. 
        /// Note that for historical reasons, the hard link count of a directory includes the number of entries in the directory and is initialized to 2 for an empty directory. 
        /// I.e. a directory with N entries has at least N + 2 link count.
        /// u32 hard_link_count (0x14)
        /// </summary>
        public uint HardLinkCount
        {
            get { return ReadUInt32(0x14); }
            set { WriteUInt32(0x14, value); }
        }

        /// <summary>
        /// Total (uncompressed) size in bytes of the entries in the Directory Table, including headers plus 3. 
        /// The extra 3 bytes are for a virtual "." and ".." item in each directory which is not written, but can be considered to be part of the logical size of the directory.
        /// u16 file_size (0x18)
        /// </summary>
        public uint FileSize
        {
            get { return Convert.ToUInt32(ReadUInt16(0x18) + 3); }
            set { WriteUInt16(0x18, value - 3); }
        }

        /// <summary>
        /// The (uncompressed) offset within the block in the Directory Table where the directory entry information starts
        /// u16 block_offset (0x1A)
        /// </summary>
        public uint BlockOffset
        {
            get { return ReadUInt16(0x1A); }
            set { WriteUInt16(0x1A, value); }
        }

        /// <summary>
        /// The inode_number of the parent of this directory. If this is the root directory, this will be 1
        /// u32 parent_inode_number (0x1C)
        /// </summary>
        public uint ParentINodeNumber
        {
            get { return ReadUInt32(0x1C); }
            set { WriteUInt32(0x1C, value); }
        }

        public SqMetadataRef NodeReference => new SqMetadataRef(DirBlockStart, BlockOffset);
    }
}
