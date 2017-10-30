# Architecture of MME

MME's architecure is kept simple. It really consists of 2 assemblies. One assembly MMEVS2010.dll is the Visual Studio AddIn, which bridges the MME AddIns with Visual Studio. If no MME AddIns are present it is invisible to the user. Nothing will indicate that it is there. Consequently is might as well be installed for all users. And that is exactly what the installer does.

The other assembly is the MMEContracts assembly which contains the Interface that you have to implement to create a Managed Menu Extension. When you install MME the MMEContracts is installed in the GAC, and is also added to the Visual Studio Add References dialog, for ease of use.

Here is an overview of the architecture:;

![](Architecture_MMEArchitectureSmall.jpg)

MMEVS2010 loads the MME's via MEF. An MME is all objects in the appropiate directories implementing IMenuManager (see [deployment](deploying)).