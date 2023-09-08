# BrainFExec
 BrainFExec is a [BrainFuck](https://esolangs.org/wiki/BrainFuck) compiler to Windows executable in C#.
## Usage
```commandline
BrainFExec <input> [output] [-o]
```
Compiles BrainFuck code into a Windows executable:
* input: Input BrainFuck file name
* output: Output BrainFuck file name
* -o: Optimize
  
If `output` is not passed, BrainFExec automatically decides the file name: For example, if the input BrainFuck filename is `a.bf`, it sets the output file to `a.exe`.
## How it works
It first converts BrainFuck to C#, then uses dynamic compiling to compile the resulting C# program to an executable.
## Requires
.NET < 5
## Changelog
* 1.0.0
  * First version
* 1.0.1
  * Added support for optimizing
  * Bugfix of help prompt
