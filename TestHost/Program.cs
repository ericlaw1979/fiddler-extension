using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;
using RunscopeFiddlerExtension;

namespace TestHost
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var share = new ShareRequest(new TestHostAdapter());
            share.OnLoad();
            share.ConfigureRunscope();

        }

        public class TestPrefs : IFiddlerPreferences
        {
            private Dictionary<string, object> _prefs = new Dictionary<string, object>(); 
            public void SetBoolPref(string sPrefName, bool bValue)
            {
                _prefs[sPrefName] = bValue;
            }

            public void SetInt32Pref(string sPrefName, int iValue)
            {
                _prefs[sPrefName] = iValue;
            }

            public void SetStringPref(string sPrefName, string sValue)
            {
                _prefs[sPrefName] = sValue;
            }

            public bool GetBoolPref(string sPrefName, bool bDefault)
            {
                object value;
                return (bool) (_prefs.TryGetValue(sPrefName,out value) ? value : bDefault);
            }

            public string GetStringPref(string sPrefName, string sDefault)
            {
                object value;
                return (string)(_prefs.TryGetValue(sPrefName, out value) ? value : sDefault);

            }

            public int GetInt32Pref(string sPrefName, int iDefault)
            {
                object value;
                return (int)(_prefs.TryGetValue(sPrefName, out value) ? value : iDefault);

            }

            public void RemovePref(string sPrefName)
            {
                _prefs.Remove(sPrefName);
            }

            public PreferenceBag.PrefWatcher AddWatcher(string sPrefixFilter, EventHandler<PrefChangeEventArgs> pcehHandler)
            {
                return new PreferenceBag.PrefWatcher();
            }

            public void RemoveWatcher(PreferenceBag.PrefWatcher wliToRemove)
            {
            }

            public string this[string sName]
            {
                get { return (string)_prefs[sName]; }
                set { _prefs[sName] = value; }
            }
        }
        public class TestHostAdapter : IFiddlerHostAdapter
        {
            public void InstallContextMenu(MenuItem menuItem)
            {
              
            }

            public void InstallConfigMenu(MenuItem menuItem)
            {
                
            }

            public void ShowStatus(string text)
            {
               
            }

            public Session SelectedSession { get; private set; }
            public IFiddlerPreferences Preferences { get { return new TestPrefs();} }
        }
    }
}
