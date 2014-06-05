using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fiddler;

namespace RunscopeFiddlerExtension
{
    public class RunscopeSettings : INotifyPropertyChanged
    {
        private readonly IFiddlerPreferences _preferences;
        private string _bucket;
        private string _apiKey;
        private bool _useProxy;

        public string Bucket
        {
            get { return _bucket; }
            set
            {
                _bucket = value;
                _preferences.SetStringPref("runscope.bucketkey", value);
                OnPropertyChanged();
            }
        }

        public string ApiKey
        {
            get { return _apiKey; }
            set
            {
                _apiKey = value;
                _preferences.SetStringPref("runscope.apikey", value);
                OnPropertyChanged();
            }
        }

        public bool UseProxy
        {
            get { return _useProxy; }
            set
            {
                _useProxy = value;
                _preferences.SetStringPref("runscope.useproxy", Convert.ToString(value));
                OnPropertyChanged();
            }
        }

        public RunscopeSettings(IFiddlerPreferences preferences)
        {
            _preferences = preferences;
            UseProxy = _preferences.GetBoolPref("runscope.useproxy", false);
            ApiKey = _preferences.GetStringPref("runscope.apikey", "");
            Bucket = _preferences.GetStringPref("runscope.bucketkey", "" );

            _preferences.AddWatcher("runscope.", OnPrefChange);

        }

        public bool ConfigComplete()
        {
            return (!String.IsNullOrEmpty(_apiKey) && 
                    !String.IsNullOrEmpty(_bucket));
        }


        private void OnPrefChange(object sender, PrefChangeEventArgs oPref)
        {
            if (oPref.PrefName == "runscope.apikey")
            {
                ApiKey = oPref.ValueString;
            }
            if (oPref.PrefName == "runscope.bucketkey")
            {
                Bucket = oPref.ValueString;
            }
            if (oPref.PrefName == "runscope.useproxy")
            {
                UseProxy = oPref.ValueBool;             
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}