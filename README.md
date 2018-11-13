# SvnModifiedFilesTattler
A Tattler Process That Returns a Non-Zero Exit Code and Output to StandardOut When Files Are Modified.

## Background
One of the bright ideas to come out of the DevOps movement is to automate code quality/code review processes. Facebook announced their work on tooling called [SapFix](https://code.fb.com/developer-tools/finding-and-fixing-software-bugs-automatically-with-sapfix-and-sapienz/). Once you get past the marketing jargon you realize all they have done is write a code analyzer that is able to identify poor coding practices, that can produce a fix, but for whatever reason still wants human interaction before pushing that fix in.

This can be easily implemented by a batch file, the unique part of their system is throwing it behind automation (they do not mention what tooling they use to drive it, in our case we use CruiseControl.NET) and then shoving a patch file to the developer. We have a similar tool which we call `Tattler`.

Chances are you have something similar too, but most likely like us, did not write the tool with creating a patch file in mind.

Instead of spending time to rewrite existing tooling to support reporting and patch creation it is better to write a wrapper that can run after those existing processes.

## When To Use This Tool
This tool works best with existing tooling that will perform modifications to a Subversion Working Copy but is not `Tattler` aware.

Many of our existing tools operated on the assumption that they were run interactively by a Developer on a clean working copy, they could then use the tooling afforded to them by their Version Control System (in our case Subversion) to show them the changes made to their source files. These tools were written to be [idempotent](https://en.wikipedia.org/wiki/Idempotence), such that the tooling can run hundreds of millions of time and the output is identical. Therefore in theory the only changes that are being shown are "legitimate" changes.

## Operation
At a high level the tool operates by:

* Calling out to an external svn.exe CLI Process with the arguments "status {WORKINGCOPY} --xml"
* This fragment is then parsed and filtered to just modified files, the names and count of which are returned.

## Usage
```
Usage: SvnModifiedFilesTattler.exe directory

Given Folder to an SVN Working Copy; Return a listing of modified files.
Invalid Command/Arguments. Valid commands are:

[directory] - [READS] MUST BE AN SVN WORKING COPY. Performs an
              svn status and then prints the files that were
              marked as modified.
              Returns the number of modified files.
```

## Hacking
The first and most obvious place to hack is to support repointing the Subversion Binary to a different location (IE not on the path). All of the Subversion operations should be encapsulated in the `SvnUtilities` class. There is already a static property `svnPath` which can be overwritten to support what you desire.

The next place to hack up this tool is to change the types of statuses modified. Take a look at `TattleModifiedFiles.Execute(string)` to see how this is implemented.

Finally the last extensibility point would be to change this tooling to produce a patch file that could then be passed on for further processing/evaluation by a developer. It very well may be that a sister tool will be written to accomplish this task.

## Contributing
Pull requests and bug reports are welcomed so long as they are MIT Licensed.

## License
This tool is MIT Licensed.