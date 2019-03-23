# LittleUnZip. A simple class for ZIP uncompress, test and get zip information

The class is in safe managed code in one class. No need external dll. The class work in 32, 64 bit or ANY.

The code is full comented and include simple example for using the class.
## Uncompress file functions:
Open an existing ZIP file for extract, test or get info.
```C#
LittleUnZip zip = new LittleUnZip("c:\\directory\\file.zip");
```

Extract full contents of a ZIP file into the output folder. Optionally you can put a progress bar.
```C#
using (LittleUnZip zip = new LittleUnZip("file.zip"))
    zip.Extract("c:\\directory", true);
```

Extract "test.txt" of a ZIP file into "C:\\extract\\test.txt".
```C#
using (LittleUnZip zip = new LittleUnZip("file.zip"))
    zip.ExtractFile("test.txt", "C:\\extract\\test.txt");
```

Extract the second file of a ZIP file into the physical file.
```C#
using (LittleUnZip zip = new LittleUnZip("file.zip"))
    zip.ExtractFile(zip.zipFileEntrys[1], "C:\\test\\" + zip.zipFileEntrys[1].filename);
```

Close zip file. This function is automatic call when dispose LittleUnZip
```C#
zip.Close()
```

## Aditional stream functions:
Open an stream for extract or test files.
```C#
using (Stream zipStream = new FileStream("file.zip", FileMode.Open, FileAccess.Read))
    LittleUnZip zip = new LittleUnZip(zipStream);
```

Extract the second file of a ZIP file into one stream.
```C#
string outPathFilename = "C:\\test\\" + zip.zipFileEntrys[1].filename;

using (Stream output = new FileStream(outPathFilename, FileMode.Create, FileAccess.Write))
using (LittleUnZip zip = new LittleUnZip("file.zip"))
    zip.ExtractFile(zip.zipFileEntrys[1], output);
```

Extract the "test.txt" file of a ZIP file into one stream.
```C#
string outPathFilename = "C:\\test\\test.txt";

using (Stream output = new FileStream(outPathFilename, FileMode.Create, FileAccess.Write))
using (LittleUnZip zip = new LittleUnZip("file.zip"))
    zip.ExtractFile("test.txt", output);
```

## Use
LittleUnZip can:
- Decompress in little time
- Use little code
- Use little memmory
- Little learning for use
- Work without DLLs. You only need add LittleUnZip.cs to you proyect.

LittleUnZip can not:
- Decompress a large zip ( > 2.147.483.647 bytes)
- Decompress other metods than Storage and Deflate
- Create ZIP file. Use LittleZip class.
- Descompress Split Zip files.
