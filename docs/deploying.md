# Deploying MME Menus

To deploy your Custom Managed Menu Extensions to a developers Machine the following needs to be done:

1. Make sure she has installed a version of Visual Studio 2010 (not express)
2. Make sure she has installed MME.msi
3. Place the assemblies you have made with implementations of IMenuManager in one or more of 3 different locations.

It is on purpose I say assemblies in plural. You can make as many IMenuManager implementations as you want, and have them all in one assembly or spread them over a number of assemblies. If several implementations of IMenuManager use the same name for the MainMenu() the submenus are placed under the same submenu. But the ordering will be dependent on the order MEF loads the IMenuManager's in.

## Where to place your MME assemblies

You can place MME assemblies in the C:\ProgramData\MME folder. The name of this folder will vary between operating systems C:\ProgramData\MME is default on Windows 7. But it is the {"[CommonAppDataFolder](CommonAppDataFolder)\MME"} folder.

The MMEVS2010 AddIn will load these assemblies for all Visual Studio Solutions,

You can also place assemblies in the same folder as the .sln file of a specific solution. In this case the menus are only loaded for that specific solution.

In between these two extremes you can choose to place the assemblies in the parent folder of the .sln file, in this case the menus will be loaded for the solutions in all the subfolders of that folder. 

E.g. I typically place all Visual Studio Solutions in the C:\Projects folder. So if I have a solution called ClassLibrary1 the solution file will be C:\Projects\ClassLibrary1\ClassLibrary1.sln. If I place an MME assembly in C:\Projects it will be loaded for all solutions in C:\Projects including ClassLibrary1. If I place it in C:\Projects\ClassLibrary1 it will only be loaded for ClassLibrary1.

If you do not want to do all this manually you can of course choose to make a bat file, an MSI package or similar.

