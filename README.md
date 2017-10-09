# reflectionCLI

This is a dotNET Core based Command Line Interface that takes advantage of reflection in order to call commands, even from assemblies (either precompiled or built in memory by the Roslyn compiler) loaded at runtime.

## Installing\Running

 - Make sure you have .NET Core Installed (this targets 1.1)
 - clone or download the repo, run `dotnet restore` from your terminal to ensure packages are installed
 - run `dotnet build`
 - run `dotnet run`

## Usage

Upon starting the application you will be greeted by `"Enter command (help to display help):"` my first suggestion would be to enter help and read the help. Beyond that there are not many standard commands, [some basic string and argument parsing commands](Commands\birdcommands.cs) to show that it works (and that I used for development/debugging purposes) and that is about it. "But Morgan why aren't there any commands?" you may ask, well because the idea of this is to be able to run external commands that have been loaded into it either through preexisting *.dll* files or through text files that contain source code with the correct format.

### Loading in Assemblies

This is the real goal of this project, dynamically changing the commands that can be accessed by the user or application. To load in a prebuilt assembly simply make sure that it implements the **ICommand** Interface that is present in [ICommand.cs](Commands\Base\ICommand.cs). For examples on how to write a set of external commands that are in the correct format see the pre-made set of libraries in my other repository [reflectionCLILibs](https://github.com/morganwm/reflectionCLILibs).

For help with compiling a standalone *.dll* look [here](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/command-line-building-with-csc-exe).

Next simply follow the commands you got when running the [*help*](Commands\Standard\help.cs) command to use the [*LoadAssembly*](Commands\Assembly\LoadAssembly.cs) Function to load this into your project.

`LoadAssembly -path [pathtoyourassembly]`

You can then check and make sure that all of your commands came in correctly by running the [*list*](Commands\Standard\list.cs) command to examine all the valid functions and assemblies currently loaded.

Using this command reads in the file as a stream so feel free to delete/move/modify your file without worring about anything breaking.

Removing an assembly is easy as well, simply run [*RemoveAssembly*](Commands\Assembly\RemoveAssembly.cs) followed by the name of the assembly you wish to remove. There is one caveat to this, because .NET Core does not support AppDomains you cannot reload in an assembly with the same name as a previously loaded assembly without restarting the app. If this fucntionality is required this next path is for you.

### Loading in Assemblies from Uncompiled Code

.NET Core Supports using the Roslyn Compiler as a Service to compile code at runtime, that combined with the before mentioned loading of prebuild assemblies allows this program to load in new commands straight from text files.

To access this functionality simply run the [*LoadUncompiledCode*](Commands\Assembly\LoadUncompiledCode.cs) command followed by a path to your text file. This command will read in and attempt to compile your code, and if successful generate an assembly named: **{name of your file}{guid}**. The GUID is important as this gets around the issue above with name collisions, meaning that you can load in the same set of code multiple times.
 
You can call functions with the same name from different assemblies by specifying the assembly name using "@" before the assembly name.

`@assemblyname commandname -variablename variablevalue`

 The other very powerful aspect of this is that because the file is read in completely at the beginning and the compilation takes place entirely in memory there are no files generated and no locks placed on existing files. Meaning that you do not need to stop or restart the application in order to continue modifying your code.