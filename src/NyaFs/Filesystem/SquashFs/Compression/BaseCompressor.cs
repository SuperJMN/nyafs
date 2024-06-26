﻿using System;
using System.Collections.Generic;
using System.Text;
using NyaIO.Data;

namespace NyaFs.Filesystem.SquashFs.Compression
{
    internal abstract class BaseCompressor : ArrayWrapper
    {
        protected readonly bool HasMetadata;

        internal BaseCompressor(long Size) : base(Size)
        {
            HasMetadata = false;
        }

        internal BaseCompressor() : base(null, 0, 0)
        {
            HasMetadata = false;
        }

        internal BaseCompressor(byte[] Raw, long Offset, long Size) : base(Raw, Offset, Size)
        {
            HasMetadata = true;
        }

        internal abstract byte[] Compress(byte[] Data);

        internal abstract byte[] Decompress(byte[] Data);
    }
}
