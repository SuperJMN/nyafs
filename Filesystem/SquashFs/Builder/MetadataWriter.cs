﻿using Extension.Array;
using System;
using System.Collections.Generic;
using System.Text;

namespace NyaFs.Filesystem.SquashFs.Builder
{
    class MetadataWriter
    {
        List<byte> Dst;
        uint BlockSize;
        ulong Offset;
        bool AddHeader;

        FragmentBlock TempMetablock;
        Compression.BaseCompressor Compressor;

        public MetadataWriter(List<byte> Dst, ulong Offset, uint BlockSize, Compression.BaseCompressor Compressor, bool AddHeader = true)
        {
            this.AddHeader = AddHeader;
            this.Dst = Dst;
            this.Offset = Offset;
            this.BlockSize = BlockSize;
            this.Compressor = Compressor;

            TempMetablock = new FragmentBlock(0, BlockSize);
        }

        private byte[] CompressBlock(byte[] Data)
        {
            var Compressed = Compressor?.Compress(Data) ?? Data;

            if (AddHeader)
            {
                if (Compressed.Length >= Data.Length)
                {
                    var Res = new byte[Data.Length + 2];
                    Res.WriteUInt16(0, Convert.ToUInt32(Data.Length) | 0x8000);
                    Res.WriteArray(2, Data, Data.Length);

                    return Res;
                }
                else
                {
                    var Res = new byte[Compressed.Length + 2];
                    Res.WriteUInt16(0, Convert.ToUInt32(Compressed.Length));
                    Res.WriteArray(2, Compressed, Compressed.Length);

                    return Res;
                }
            }
            else
            {
                return Compressed;
            }
        }

        public MetadataRef Write(byte[] Data)
        {
            long Offset = 0;
            var Ref = TempMetablock.Write(Data, ref Offset);

            while (Offset < Data.Length)
            {
                if (TempMetablock.IsFilled)
                {
                    var Compressed = CompressBlock(TempMetablock.Data);
                    System.Diagnostics.Debug.WriteLine($"Metadata: {Dst.Count:x06} l {Compressed.Length:x04}");
                    Dst.AddRange(Compressed);
                    var Dec = Compressor.Decompress(Compressed);

                    TempMetablock = new FragmentBlock(Dst.Count, BlockSize);
                }

                TempMetablock.Write(Data, ref Offset);
            }

            Ref.MetadataOffset += this.Offset;
            return Ref;
        }

        public void Flush()
        {
            if (TempMetablock.DataSize > 0)
            {
                var Compressed = CompressBlock(TempMetablock.Data);
                System.Diagnostics.Debug.WriteLine($"Metadata: {Dst.Count:x06} l {Compressed.Length:x04}");
                Dst.AddRange(Compressed);

                if (Compressor != null)
                {
                    if (AddHeader)
                    {
                        if((Compressed[1] & 0x80) == 0)
                            Compressor.Decompress(Compressed.ReadArray(2, Compressed.Length));
                    }
                    else
                        Compressor.Decompress(Compressed);
                }
            }
        }
    }
}