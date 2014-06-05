using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Fiddler;

namespace RunscopeFiddlerExtension
{
    public interface IFiddlerHostAdapter
    {
        void InstallContextMenu(MenuItem menuItem);
        void InstallConfigMenu(MenuItem menuItem);
        void ShowStatus(string text);
        Session SelectedSession { get; }

        IFiddlerPreferences Preferences { get;  }
    }
}