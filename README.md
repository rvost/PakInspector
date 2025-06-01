# PakInspector

PakInspector is simple CLI tool for exploring and extracting Arma Reforger's PAC1 files.

---

This is a proof of concept implementation aimed at investigating and addressing the issues with existing tools ([\[1\]](https://github.com/FlipperPlz/PakExplorer/issues/5), [\[2\]](https://github.com/Rendszerguru/PakEntpacker/issues/1)). If you encounter similar problems, please share the results obtained using the command `PakInspector inspect <file> --save`.

## Features

- View the file structure inside Arma Reforger's `.pak` files;
- Extract all the content from the `.pak` file, or just specific files;
  - If the file is compressed using an unknown algorithm, you can extract the raw file section for analysis
- View chunks of arbitrary Interchange File Format files;
  - Extracting chunks isn't currently supported, but this feature may be added later.

## Requirements

**This program requires [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) to run**.

On Windows 11 you can easily install it with `winget`:
```
winget install Microsoft.DotNet.DesktopRuntime.8
```

## Installation

PakInspector does not require installation. Simply download `.exe` from the latest release and start using it! For convenience, you may want to [add](https://stackoverflow.com/questions/9546324/adding-a-directory-to-the-path-environment-variable-in-windows) your PakInspector location to the `PATH`.

Sometimes you may get a warning from Windows Defender or other anti-virus software. You can safely ignore it. This is because PakInspector is not a properly signed application.

## Usage examples

- Extract all files from `data.pak` to `output` folder:
  ```
    .\PakInspector extract data.pak output
  ```
- Extract `foo\bar.xob` and `foo\bazz.xob`  from `data.pak` to `output` folder:
  ```
    .\PakInspector extract data.pak output -f "foo\bar.xob" -f "foo\bazz.xob"
  ```
  Please note that you must specify `-f` (or `--file`) key before each filename. The filename must match one of those returned by the 'inspect' command.
- Show file tree for `data.pak`
  ```
  .\PakInspector inspect data.pak --tree
  ```
- Inspect `data.pak` and save results without printing to console
  ```
  .\PakInspector inspect data.pak -qs
  ```

You can also use the `--help` key to view all the available commands and options.

## Issues

If you find a bug or have a feature request, please use [Issues](https://github.com/rvost/PakInspector/issues) to report it. Samples of the `.pak` files that the tool fails to extract are needed. If you have encountered this problem, please post your *inspection results* in the Issues or send me a message on Discord.

## Contribution

All contributions are welcome! If you have any plans for improving the parser, please use the `formats/pak.ksy` file as a starting point and avoid modifying the *generated* parser code.

## Acknowledgments

This tool relies heavily on [@FlipperPlz](https://github.com/FlipperPlz/)'s work in [PakExplorer](https://github.com/FlipperPlz/PakExplorer), the first open source tool for reading `PAC1` files.
