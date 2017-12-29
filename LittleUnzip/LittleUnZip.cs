/////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// LittleUnZip C#. (GPL) Jose M. Piñeiro
/// Version: 1.0.0.1 (Dec 29, 2017)
/// 
/// LittleUnZip can:
/// - Decompress in little time
/// - Use very little code
/// - Use very memmory
/// - Very little learning for use
/// - Work without DLLs. You only need add LittleUnZip.cs to you proyect.
/// 
/// LittleUnZip can not:
/// - Decompress a large zip ( > 2.147.483.647 bytes)
/// - Decompress other metods than Storage and Deflate
/// - Compress one ZIP file. Use LittleZip program.
///
///////////////////////////////////////////////////////////////////////////////////////////////////////////// 
/// Implemented functions:
/// LittleUnZip(string zipFilename)
/// - Open an existing ZIP file for extract or test files. Return zip object.
/// 
/// LittleUnZip(Stream zipStream)
/// - Open an open zip stream for extract or test files. Return zip object.
/// 
/// Test()
/// - Test zip object. Return true if all test are OK.
/// 
/// Extract(string outputFolder, bool dirs, ProgressBar progressBar = null)
/// - Extract full contents of a zip file into the target folder. Optionally you can put a progress bar.
/// 
/// ExtractFile(ZipFileEntry zfe, string outPathFilename)
/// - Extract a file into physical file.
///  
/// ExtractFile(ZipFileEntry zfe, Stream outStream)
/// - Extract a file into stream.
/// 
/// ExtractFile(string filename, Stream outStream)
/// - Extract a file into stream.
/// 
/// TestFile(ZipFileEntry zfe)
/// - Test one file (zfe). Return true if all test are OK. 
/// 
/// Close()
/// - Close streams and free memmory. Automatic call in dispose
/// 
///
/// Public zipFileEntrys:
/// list of all files in zip, with all information.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace System.IO.Compression
{
    /// <summary>Unique class for decompression file. Represents a Zip file.</summary>
    public sealed class LittleUnZip : IDisposable
    {
        /// <summary>
        /// Compression method enumeration
        /// </summary>
        public enum Compression : ushort
        {
            /// <summary>Uncompressed storage</summary> 
            Store = 0,
            /// <summary>Deflate compression method</summary>
            Deflate = 8
        }

        /// <summary>
        /// Represents an entry in Zip file directory
        /// </summary>
        public struct ZipFileEntry
        {
            /// <summary>Compression method</summary>
            public Compression method;
            /// <summary>Full path and filename as stored in Zip</summary>
            public string filename;
            /// <summary>Original file size</summary>
            public uint fileSize;
            /// <summary>Compressed file size</summary>
            public uint compressedSize;
            /// <summary>Offset of header information inside Zip storage</summary>
            public uint headerOffset;
            /// <summary>Offset of file inside Zip storage</summary>
            public uint fileOffset;
            /// <summary>Size of header information</summary>
            public int headerSize;
            /// <summary>32-bit checksum of entire file</summary>
            public uint crc32;
            /// <summary>Last modification time of file</summary>
            public DateTime modifyTime;
            /// <summary>User comment for file</summary>
            public string comment;
            /// <summary>True if UTF8 encoding for filename and comments, false if default (CP 437)</summary>
            public bool encodeUTF8;

            /// <summary>Overriden method</summary>
            /// <returns>Filename in Zip</returns>
            public override string ToString()
            {
                return this.filename;
            }
        }

        #region Public fields
        // List of files to store
        public List<ZipFileEntry> zipFileEntrys = new List<ZipFileEntry>();
        #endregion

        #region Private fields
        // Stream object of storage file
        private Stream zipFileStream;
        // Stream object of storage file
        private string zipFileName;
        // Existing files in zip
        private ushort existingFiles = 0;
        #endregion

        #region | Constructors |
        /// <summary>
        /// Create zip object
        /// </summary>
        /// <param name="filename">Full path of Zip file to open</param>
        public LittleUnZip(string filename)
        {
            //Generate CRC32 table
            if (crcTable[0] == 0)
                PrecalcCRC();

            this.zipFileName = filename;
            using (Stream zipStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                if (!ReadZipInfo(zipStream))
                    throw new System.IO.InvalidDataException();
            }
        }

        /// <summary>
        /// Create zip object
        /// </summary>
        /// <param name="stream">Already opened stream with zip contents</param>
        public LittleUnZip(Stream stream)
        {
            //Generate CRC32 table
            if (crcTable[0] == 0)
                PrecalcCRC();

            if (!stream.CanSeek)
                throw new InvalidOperationException("Stream cannot seek");

            this.zipFileStream = stream;
            if (!ReadZipInfo(stream))
                throw new System.IO.InvalidDataException();
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Closes the Zip file stream
        /// </summary>
        public void Dispose()
        {
            if (this.zipFileStream != null)
            {
                zipFileStream.Close();
                this.zipFileStream.Dispose();
                this.zipFileStream = null;
            }
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Public methods
        /// <summary>Text zip integrity</summary>
        /// <returns>True if zip file is ok</returns>
        public bool Test()
        {
            try
            {
                // Test all files
                for (int f = 0; f < this.zipFileEntrys.Count; f++)
                    if (!TestFile(this.zipFileEntrys[f]))
                        return false;

                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nIn ZipExtract.Test"); }
        }

        /// <summary>
        /// Copy the contents of all stored files into a folder
        /// </summary>
        /// <param name="outputFolder">Destination folder</param>
        /// <param name="dirs">if TRUE, recreate struct directories</param>
        /// <param name="progressBar">Progress bar of extract process</param>
        /// <remarks>Unique decompression methods are Store and Deflate</remarks>
        public void Extract(string outputFolder, bool dirs, ProgressBar progressBar = null)
        {
            try
            {
                // Extract all files in target directory
                string path;

                for (int f = 0; f < this.zipFileEntrys.Count; f++)
                {
                    if (dirs)
                    {
                        path = outputFolder + Path.DirectorySeparatorChar + this.zipFileEntrys[f].filename;
                        path = path.Replace('/', Path.DirectorySeparatorChar);
                    }
                    else
                        path = Path.Combine(outputFolder, Path.GetFileName(this.zipFileEntrys[f].filename));

                    if (!ExtractFile(this.zipFileEntrys[f], path))
                        throw new Exception("Can´t extract " + this.zipFileEntrys[f]);

                    //Update progress bar
                    if (progressBar != null)
                    {
                        progressBar.Value = (int)(progressBar.Maximum * (this.zipFileEntrys.IndexOf(this.zipFileEntrys[f]) + 1) / this.zipFileEntrys.Count);
                        Application.DoEvents();
                    }
                }
            }
            catch (InvalidDataException) { throw new InvalidDataException("Bad zip file."); }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nIn ZipExtract.ExtractAll"); }
        }

        /// <summary>
        /// Copy the contents of a stored file into a physical file
        /// </summary>
        /// <param name="zfe">Entry information of file to extract</param>
        /// <param name="outPathFilename">Name of file to store uncompressed data</param>
        /// <returns>True if success, false if not.</returns>
        /// <remarks>Unique decompression methods are Store and Deflate</remarks>
        public bool ExtractFile(ZipFileEntry zfe, string outPathFilename)
        {
            try
            {
                bool result;

                // Make sure the parent directory exist
                string path = Path.GetDirectoryName(outPathFilename);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                // Check it is directory. If so, do nothing
                if (Directory.Exists(outPathFilename))
                    return true;

                //Delete file to create
                if (File.Exists(outPathFilename))
                    try
                    {
                        File.Delete(outPathFilename);
                    }
                    catch
                    {
                        throw new InvalidOperationException("File '" + outPathFilename + "' cannot be written");
                    }

                using (Stream output = new FileStream(outPathFilename, FileMode.Create, FileAccess.Write))
                {
                    result = ExtractFile(zfe, output);
                    output.Close();
                }

                //Change file datetimes
                while (IsFileLocked(outPathFilename))
                    Application.DoEvents();
                File.SetCreationTime(outPathFilename, zfe.modifyTime);
                File.SetLastWriteTime(outPathFilename, zfe.modifyTime);

                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nIn ZipExtract.ExtractFile"); }
        }

        /// <summary>
        /// Copy the contents of a stored file into an opened stream
        /// </summary>
        /// <param name="zfe">Entry information of file to extract</param>
        /// <param name="outStream">Stream to store the uncompressed data</param>
        /// <returns>True if success, false if not.</returns>
        /// <remarks>Unique decompression methods are Store and Deflate</remarks>
        public bool ExtractFile(ZipFileEntry zfe, Stream outStream)
        {
            Stream zipStream;

            try
            {
                if (this.zipFileStream != null)
                    zipStream = this.zipFileStream;
                else
                    zipStream = new FileStream(this.zipFileName, FileMode.Open, FileAccess.Read);

                if (!outStream.CanWrite)
                    throw new InvalidOperationException("Stream cannot be written");

                // check signature
                byte[] signature = new byte[4];
                zipStream.Seek(zfe.headerOffset, SeekOrigin.Begin);
                zipStream.Read(signature, 0, 4);
                if (BitConverter.ToUInt32(signature, 0) != 0x04034b50)
                    return false;

                // Select input stream for inflating or just reading
                Stream deflatedStream;
                if (zfe.method == Compression.Store)
                    deflatedStream = zipStream;
                else if (zfe.method == Compression.Deflate)
                    deflatedStream = new DeflateStream(zipStream, CompressionMode.Decompress, true);
                else
                    return false;

                //Inicialize CRC
                uint crc32 = 0xffffffff;

                // Buffered copy
                byte[] buffer = new byte[16384];
                zipStream.Seek(zfe.fileOffset, SeekOrigin.Begin);
                uint bytesPending = zfe.fileSize;
                while (bytesPending > 0)
                {
                    int bytesRead = deflatedStream.Read(buffer, 0, (int)Math.Min(bytesPending, buffer.Length));
                    crc32 = Crc32(crc32, ref buffer, bytesRead);
                    bytesPending -= (uint)bytesRead;
                    outStream.Write(buffer, 0, bytesRead);
                }
                outStream.Flush();

                //Close streams
                if (zfe.method == Compression.Deflate)
                    deflatedStream.Dispose();
                if (this.zipFileStream == null)
                    zipStream.Dispose();

                //Verify data integrity
                crc32 ^= 0xffffffff;
                if (zfe.crc32 != crc32)
                    return false;
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nIn ZipExtract.ExtractFile"); }
        }

        /// <summary>
        /// Copy the contents of a stored file into an opened stream
        /// </summary>
        /// <param name="filename">Filename to extract</param>
        /// <param name="outStream">Stream to store the uncompressed data</param>
        /// <returns>True if success, false if not.</returns>
        /// <remarks>Unique decompression methods are Store and Deflate</remarks>
        public bool ExtractFile(string filename, Stream outStream)
        {
            try
            {
                List<ZipFileEntry> entry = zipFileEntrys.FindAll(name => name.filename == filename);
                ZipFileEntry _zfe = entry[0];

                return ExtractFile(_zfe, outStream);
            }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nIn ZipExtract.ExtractFile"); }
        }

        /// <summary>
        /// Test the contents of a stored file
        /// </summary>
        /// <param name="_zfe">Entry information of file to extract</param>
        /// <returns>True if success, false if not.</returns>
        /// <remarks>Unique decompression methods are Store and Deflate</remarks>
        public bool TestFile(ZipFileEntry _zfe)
        {
            Stream zipStream;

            try
            {
                if (this.zipFileStream != null)
                    zipStream = this.zipFileStream;
                else
                    zipStream = new FileStream(this.zipFileName, FileMode.Open, FileAccess.Read);

                // check signature
                byte[] signature = new byte[4];
                zipStream.Seek(_zfe.headerOffset, SeekOrigin.Begin);
                zipStream.Read(signature, 0, 4);
                if (BitConverter.ToUInt32(signature, 0) != 0x04034b50)
                    return false;

                // Select input stream for inflating or just reading
                Stream deflatedStream;
                if (_zfe.method == Compression.Store)
                    deflatedStream = zipStream;
                else if (_zfe.method == Compression.Deflate)
                    deflatedStream = new DeflateStream(zipStream, CompressionMode.Decompress, true);
                else
                    return false;

                // Check CRC
                uint crc32 = 0 ^ 0xffffffff;
                byte[] buffer = new byte[16384 * 8];
                zipStream.Seek(_zfe.fileOffset, SeekOrigin.Begin);
                uint bytesPending = _zfe.fileSize;
                while (bytesPending > 0)
                {
                    int bytesRead = deflatedStream.Read(buffer, 0, (int)Math.Min(bytesPending, buffer.Length));
                    crc32 = Crc32(crc32, ref buffer, bytesRead);
                    bytesPending -= (uint)bytesRead;
                }

                //Close streams
                if (_zfe.method == Compression.Deflate)
                    deflatedStream.Dispose();
                if (this.zipFileStream == null)
                    zipStream.Dispose();

                //Verify data integrity
                crc32 ^= 0xffffffff;
                if (_zfe.crc32 != crc32)
                    return false;

                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nIn ZipExtract.TestFile"); }
        }
        #endregion

        #region Private methods
        /* DOS Date and time:
            MS-DOS date. The date is a packed value with the following format. Bits Description 
                0-4 Day of the month (1–31) 
                5-8 Month (1 = January, 2 = February, and so on) 
                9-15 Year offset from 1980 (add 1980 to get actual year) 
            MS-DOS time. The time is a packed value with the following format. Bits Description 
                0-4 Second divided by 2 
                5-10 Minute (0–59) 
                11-15 Hour (0–23 on a 24-hour clock) 
        */
        private DateTime DosTimeToDateTime(uint _dt)
        {
            return new DateTime(
                (int)(_dt >> 25) + 1980,
                (int)(_dt >> 21) & 15,
                (int)(_dt >> 16) & 31,
                (int)(_dt >> 11) & 31,
                (int)(_dt >> 5) & 63,
                (int)(_dt & 31) * 2);
        }

        // Reads the end-of-central-directory record and entrys
        private bool ReadZipInfo(Stream zipStream)
        {
            byte[] centralDirImage = null;                  // Central dir image
            try
            {
                if (zipStream.Length < 22)
                    return false;

                zipStream.Seek(-17, SeekOrigin.End);

                do
                {
                    zipStream.Seek(-5, SeekOrigin.Current);

                    BinaryReader br = new BinaryReader(zipStream);
                    UInt32 sig = br.ReadUInt32();
                    if (sig == 0x06054b50)
                    {
                        zipStream.Seek(6, SeekOrigin.Current);

                        UInt16 entries = br.ReadUInt16();
                        Int32 centralSize = br.ReadInt32();
                        UInt32 centralDirOffset = br.ReadUInt32();
                        UInt16 commentSize = br.ReadUInt16();

                        // check if comment field is the very last data in file
                        if (zipStream.Position + commentSize != zipStream.Length)
                            return false;

                        // Copy entire central directory to a memory buffer
                        this.existingFiles = entries;
                        centralDirImage = new byte[centralSize];
                        zipStream.Seek(centralDirOffset, SeekOrigin.Begin);
                        zipStream.Read(centralDirImage, 0, centralSize);


                        //Read ZipEntrys
                        /* Central directory's File header:
                           central file header signature   4 bytes  (0x02014b50)
                           version made by                 2 bytes
                           version needed to extract       2 bytes
                           general purpose bit flag        2 bytes
                           compression method              2 bytes
                           last mod file time              2 bytes
                           last mod file date              2 bytes
                           crc-32                          4 bytes
                           compressed size                 4 bytes
                           uncompressed size               4 bytes
                           filename length                 2 bytes
                           extra field length              2 bytes
                           file comment length             2 bytes
                           disk number start               2 bytes
                           internal file attributes        2 bytes
                           external file attributes        4 bytes
                           relative offset of local header 4 bytes
                           filename (variable size)
                           extra field (variable size)
                           file comment (variable size)
                        */
                        for (int pointer = 0; pointer < centralDirImage.Length; )
                        {
                            uint signature = BitConverter.ToUInt32(centralDirImage, pointer);
                            if (signature != 0x02014b50)
                                break;

                            ZipFileEntry zfe = new ZipFileEntry();

                            bool encodeUTF8 = (BitConverter.ToUInt16(centralDirImage, pointer + 8) & 0x0800) != 0; //True if UTF8 encoding for filename and comments, false if default (CP 437)
                            Encoding encoder = encodeUTF8 ? Encoding.UTF8 : Encoding.GetEncoding(437);
                            zfe.method = (Compression)BitConverter.ToUInt16(centralDirImage, pointer + 10);
                            zfe.modifyTime = DosTimeToDateTime(BitConverter.ToUInt32(centralDirImage, pointer + 12));
                            zfe.crc32 = BitConverter.ToUInt32(centralDirImage, pointer + 16);
                            zfe.compressedSize = BitConverter.ToUInt32(centralDirImage, pointer + 20);
                            zfe.fileSize = BitConverter.ToUInt32(centralDirImage, pointer + 24);
                            ushort filenameSize = BitConverter.ToUInt16(centralDirImage, pointer + 28);
                            ushort extraSize = BitConverter.ToUInt16(centralDirImage, pointer + 30);
                            ushort fileCommentSize = BitConverter.ToUInt16(centralDirImage, pointer + 32);
                            zfe.headerOffset = BitConverter.ToUInt32(centralDirImage, pointer + 42);
                            zfe.headerSize = 46 + filenameSize + extraSize + fileCommentSize;
                            zfe.filename = encoder.GetString(centralDirImage, pointer + 46, filenameSize);
                            zfe.fileOffset = (uint)(30 + filenameSize + extraSize + zfe.headerOffset);

                            if (fileCommentSize > 0)
                                zfe.comment = encoder.GetString(centralDirImage, pointer + 46 + filenameSize + extraSize, fileCommentSize);

                            this.zipFileEntrys.Add(zfe);
                            pointer += zfe.headerSize;
                        }
                        br.Close();
                        return true;
                    }
                } while (zipStream.Position > 0);

                return false;
            }
            catch (Exception ex) { throw new Exception(ex.Message + "\r\nEn ZipExtract.ReadZipInfo"); }
        }

        /// <summary>Check if the file is locked</summary>
        /// <param name="pathFileName">File to check</param>
        /// <returns>True if the file is locked</returns>
        private bool IsFileLocked(string pathFileName)
        {
            try
            {
                //File exist?
                if (!File.Exists(pathFileName))
                    return false;

                //if can open, not is blocked
                using (File.Open(pathFileName, FileMode.Open, FileAccess.Write, FileShare.None)) { }
                return false;
            }
            catch
            {
                return true;
            }
        }
        #endregion

        #region CRC32 methods
        private static readonly uint[] crcTable = new uint[16 * 256];

        /// <summary>
        /// Generate CRC32 table
        /// </summary>
        private void PrecalcCRC()
        {
            const uint Poly = 0xedb88320u;

            var table = crcTable;
            for (uint i = 0; i < 256; i++)
            {
                uint res = i;
                for (int t = 0; t < 16; t++)
                {
                    for (int k = 0; k < 8; k++) res = (res & 1) == 1 ? Poly ^ (res >> 1) : (res >> 1);
                    table[(t * 256) + i] = res;
                }
            }
        }

        /// <summary>
        /// Make CRC 32
        /// </summary>
        /// <param name="crc32">Actual CRC32 value</param>
        /// <param name="data">Data to process</param>
        /// <param name="length">Length of data</param>
        /// <returns>New CRC32 value</returns>
        private uint Crc32(uint crc32, ref byte[] data, int length)
        {
            int offset = 0;
            uint[] table = crcTable;

            //Process block of 16 bytes
            while (length >= 16)
            {
                var a = table[(3 * 256) + data[offset + 12]]
                    ^ table[(2 * 256) + data[offset + 13]]
                    ^ table[(1 * 256) + data[offset + 14]]
                    ^ table[(0 * 256) + data[offset + 15]];

                var b = table[(7 * 256) + data[offset + 8]]
                    ^ table[(6 * 256) + data[offset + 9]]
                    ^ table[(5 * 256) + data[offset + 10]]
                    ^ table[(4 * 256) + data[offset + 11]];

                var c = table[(11 * 256) + data[offset + 4]]
                    ^ table[(10 * 256) + data[offset + 5]]
                    ^ table[(9 * 256) + data[offset + 6]]
                    ^ table[(8 * 256) + data[offset + 7]];
                var d = table[(15 * 256) + ((byte)crc32 ^ data[offset])]
                    ^ table[(14 * 256) + ((byte)(crc32 >> 8) ^ data[offset + 1])]
                    ^ table[(13 * 256) + ((byte)(crc32 >> 16) ^ data[offset + 2])]
                    ^ table[(12 * 256) + ((crc32 >> 24) ^ data[offset + 3])];

                crc32 = d ^ c ^ b ^ a;
                offset += 16;
                length -= 16;
            }

            //Process remain bytes
            while (--length >= 0)
                crc32 = table[(crc32 ^ data[offset++]) & 0xff] ^ crc32 >> 8;

            //return result
            return crc32;
        }
        #endregion
    }
}

