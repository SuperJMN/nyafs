﻿using System;
using System.Collections.Generic;
using System.Text;
using NyaIO.Data;

namespace NyaFs.ImageFormat.Elements.Fs.Writer
{
    public class Writer
    {
        public virtual void WriteFs(LinuxFilesystem Fs)
        {

        }

        public virtual bool HasRawStreamData => RawStream != null;

        public virtual byte[] RawStream => null;

        protected static uint ConvertToUnixTimestamp(DateTime timestamp)
        {
            return Convert.ToUInt32(((DateTimeOffset)timestamp).ToUnixTimeSeconds());
        }

        public virtual bool CheckFilesystem(LinuxFilesystem Fs) => (Fs != null);

        internal static bool IsFilesystemSupported(Types.FsType Type)
        {
            switch (Type)
            {
                case Types.FsType.Cpio: 
                case Types.FsType.Ext2:
                case Types.FsType.SquashFs:
                case Types.FsType.CramFs:
                case Types.FsType.RomFs:
                    return true;
                default:
                    return false;
            }
        }

        internal static uint DetectFixDiskSize(LinuxFilesystem Fs, uint BlockSize) =>Convert.ToUInt32(Fs.GetContentSize() * 1.5).GetAligned(BlockSize);

        internal static Writer GetRawFilesystemWriter(LinuxFilesystem Fs)
        {
            switch(Fs.FilesystemType)
            {
                case Types.FsType.Cpio: return new CpioFsWriter();
                case Types.FsType.Ext2: return new Ext2FsWriter(DetectFixDiskSize(Fs, 0x800000));
                case Types.FsType.SquashFs: return new SquashFsWriter(Fs.SquashFsCompression);
                case Types.FsType.CramFs: return new CramFsWriter();
                case Types.FsType.RomFs: return new RomFsWriter();

                default: throw new InvalidOperationException($"Unsupported filesystem: {Fs.FilesystemType}");
            }
        }
    }
}
