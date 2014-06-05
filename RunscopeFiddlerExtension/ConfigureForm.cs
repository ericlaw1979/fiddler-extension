using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunscopeFiddlerExtension
{
    public partial class ConfigureForm : Form
    {
        private readonly ConfigureViewModel _model;

        public ConfigureForm(ConfigureViewModel model) : this()
        {
            _model = model;
            _model.PropertyChanged += _model_PropertyChanged;

            RefreshApiKey();
            RefreshUseProxy();
            LoadBuckets();
        }

        private void RefreshUseProxy()
        {
            chkUseProxy.Checked = _model.UseProxy;
        }

        void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Buckets")
            {
                Invoke((Action)LoadBuckets);
            }
            if (e.PropertyName == "ApiKey")
            {
                Invoke((Action)RefreshApiKey);
            }
            
        }

        public ConfigureForm()
        {
            InitializeComponent();
            cboBuckets.SelectedValueChanged += CboBucketsSelectedValueChanged;
            cboBuckets.Enabled = false;
        }

        void CboBucketsSelectedValueChanged(object sender, EventArgs e)
        {
            _model.SelectBucket((string) cboBuckets.SelectedItem);
        }

        private void RefreshApiKey()
        {
            txtApiKey.Text = _model.ApiKey;
        }

        private void LoadBuckets()
        {
            cboBuckets.Items.Clear();

            if (_model.Buckets != null && _model.Buckets.Count > 0)
            {
                foreach (var bucket in _model.Buckets)
                {
                    cboBuckets.Items.Add(bucket.Name);
                }
           
                cboBuckets.Enabled = true;
                var selectedBucket = _model.SelectedBucket;
                if (selectedBucket != null)
                {
                    cboBuckets.SelectedItem = selectedBucket.Name;
                }
            }
            else
            {
                cboBuckets.Enabled = false;
                
            }

        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void cmdGetKey_Click(object sender, EventArgs e)
        {
            _model.GetApiKey();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _model.UseProxy = chkUseProxy.Checked;
        }



    }
}
