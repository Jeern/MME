# Implementing a Managed Menu Extension

## Install

To Implement a Managed Menu Extension you first need to install the MME.msi from the downloads page. Optionally you can also install the MMEMenuManager.vsix to get the project template for the MenuManager project type.

Both can alternatively be installed from the Visual Studio Gallery via the Extension Manager in Visual Studio 2010.

## Implement IMenuManager

Secondly you need to implement the IMenuManager interface. If you have installed the MMEMenuManager.vsix you can use the Add New Project dialog to add a Managed Menu Extension project. Choose "Add New Project" and then "Visual C#" -> "Extensibility". Alternatively you can add a classlibrary, and use the Add References dialog to Add the "MMEContracts" assembly.

The MMEContracts assembly contains the IMenuManager interface.

Now you just need to implement the two methods of the interface. One called MainMenu() and one called GetMenus().

Here is a sample of an implementation that adds a Menu called "My Main Menu" and two SubMenus called "My Menu1" and "My Menu2":

{code:c#}
    public class MenuManager : IMenuManager
    {
        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            var menuItems = new List<IMenuItem>();
            var menuItem1 = new MenuItem("My Menu1");
            menuItems.Add(menuItem1);
            var menuItem2 = new MenuItem("My Menu2");
            menuItems.Add(menuItem2);
            return menuItems;
        }

        public string MainMenu()
        {
            return "My Main Menu";
        }
    }
{code:c#}

This works perfectly. However it is really boring when nothing happens when you click the Menus. To add functionality for this I provide a click event on the MenuItems. In this sample you can see how to add a click event to "My Menu1":

{code:c#}
    public class MenuManager : IMenuManager
    {
        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            var menuItems = new List<IMenuItem>();
            var menuItem1 = new MenuItem("My Menu1");
            menuItem1.Click += Menu1Click;
            menuItems.Add(menuItem1);
            ...
            return menuItems;
        }

        private void Menu1Click(object sender, EventArgs<IMenuContext> e)
        {
            //TODO: Add functionality for clicking the menu here. You can access the MenuContext
            //by using e.Data, and the MenuItem by casting sender to MenuItem like this:
            var menuItem = sender as MenuItem;
            var menuContext = e.Data;
        }

       ...
    }
{code:c#}

In the Menu1Click method you can implement whatever functionality you need when clicking the menu. One tip is that you can access more than 4000 built in Visual Studio commands by using e.Data.Details.VSStudio.ExecuteCommand(...).

I have added the names of these commands to the VSCommands.txt file in the Lib solution folder of the Sourcecode. Only a few of these will be relevant and some of them requires parameters. If you need more information about specific commands you will have to use your favourite search engine to find out more. Use at your own discretion.

In the sourcecode there is also some samples you can use as inspiration, just search for IMenuManager.

## Visibility of your menus

When you need menus that are only shown in certain situations you can use the ContextLevel parameter of the GetMenus method for simple scenarioes. Below you can see a menu that is only shown on projects. When you are clicking a solution or class file it will not be shown:

{code:c#}
    public class MenuManager : IMenuManager
    {
        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            var menuItems = new List<MenuItem>();
            if (menuForLevel == ContextLevels.Project)
            {
                var menuItem1 = new MenuItem("My Menu1");
                menuItem1.Click += Menu1Click;
                menuItems.Add(menuItem1);
                ...
            }
            return menuItems;
        }

       ...
    }
{code:c#}

For more advanced scenarioes I provide a lambda called IsVisible on the MenuItem. Below is one that is only shown on Designer.cs files:

{code:c#}
    public class MenuManager : IMenuManager
    {
        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            var menuItems = new List<MenuItem>();
            var menuItem1 = new MenuItem("My Menu1");
            menuItem1.Click += Menu1Click;
            menuItem1.IsVisible = context => context.ItemName.ToLower().EndsWith("designer.cs");
            menuItems.Add(menuItem1);
            ...
            return menuItems;
        }

       ...
    }
{code:c#}
