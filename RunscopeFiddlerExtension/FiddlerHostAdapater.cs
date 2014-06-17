using System.Collections.Generic;
using System.Windows.Forms;
using Fiddler;

namespace RunscopeFiddlerExtension
{
    public class FiddlerHostAdapater : IFiddlerHostAdapter
    {
        public void InstallContextMenu(MenuItem menuItem)
        {
            FiddlerApplication.UI.lvSessions.ContextMenu.MenuItems.Add(0, menuItem);
        }

        public void InstallConfigMenu(MenuItem menuItem)
        {
            FiddlerApplication.UI.mnuTools.MenuItems.Add(menuItem);
        }

        public void ShowStatus(string text)
        {
            FiddlerApplication.UI.SetStatusText(text);
        }

        public Session SelectedSession
        {
            get { return FiddlerApplication.UI.GetFirstSelectedSession(); }
        }

        public IFiddlerPreferences Preferences
        {
            get { return FiddlerApplication.Prefs; }
        }
    }
}