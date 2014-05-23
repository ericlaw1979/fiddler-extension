using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Runscope;

namespace RunscopeFiddlerExtension
{
    public partial class ConfigureForm : Form
    {
        private List<Bucket> _buckets;
        public string SelectedBucketKey { get; set; }
        public ConfigureForm()
        {
            InitializeComponent();
            cboBuckets.SelectedValueChanged += CboBucketsSelectedValueChanged;
            cboBuckets.Enabled = false;
        }

        void CboBucketsSelectedValueChanged(object sender, EventArgs e)
        {
            SelectedBucketKey =
                _buckets.Where(b => b.Name == (string) cboBuckets.SelectedItem).Select(b => b.Key).FirstOrDefault();
            
        }

        public List<Runscope.Bucket> Buckets
        {
            get { return _buckets; }
            set
            {
                _buckets = value;
                this.Invoke((Action)LoadBuckets);
                
                
            }
        }

        private void LoadBuckets()
        {
            cboBuckets.Items.Clear();
            foreach (var bucket in _buckets)
            {
                this.cboBuckets.Items.Add(bucket.Name);
            }
            cboBuckets.Enabled = true;
            var selectedBucket = _buckets.FirstOrDefault(b => b.Key == SelectedBucketKey);
            if (selectedBucket != null)
            {
                cboBuckets.SelectedItem = selectedBucket.Name;
            }

        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }



    }
}
