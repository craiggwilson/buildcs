BuildCs
===========================

A build system for .NET based on [scriptcs](https://github.com/scriptcs/scriptcs) using C# as the scripting language. 

Getting Started
---------------

1. scriptcs -install ScriptCs.BuildCs
1. define a build script. BuildCs is built using BuildCs and can server as an example [build script](build.csx).

TODO
-----
* More Tests. I built this as a prototype just to see if I could get it working and it works pretty darn well. However, I skipped the whole testing thing for the most part, so... need to do that!
* Build a standalone runner. It'd be great to be able to run like [FAKE](http://fsharp.github.io/FAKE/) without needing anything installed on the system.
* Documentation. There is currently 0 documentation. Don't really want to spend time on it if no one finds this project useful, but it's probably pretty hard to find it useful without documentation.  Ho Hum....
* More stuff.
** Support for artifact based dependencies. It'd be awesome to have the build step (for instance) define all the inputs and outputs and not run the raget if the outputs are newer than the inputs. 
** OpenCover, NCover, etc...

