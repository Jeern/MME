# Why ?

It is much too hard to implement new Menus for the Visual Studio Solution Explorer using the convoluted AddIn model of Visual Studio. In MME I have made this incredibly easy.

# How ?

You just implement 2 methods of the IMenuManager interface, compile and place the resulting assembly in a directory. Viola! You get the MenuItems of your choice. You can follow the links provided here to see details about [implementing](implementing) and [deploying](deploying) Managed Menu Extensions.

# What ?

The architecture of MME is explained in detail [here](Architecture).

