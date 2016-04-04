using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemCompare.GUI
{
    public partial class FrmGetUserInput : Form
    {

    public string SnapshotName { get; set; }

        public FrmGetUserInput()
        {
            InitializeComponent();
        }

        private void FrmGetUserInput_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                MessageBox.Show(@"A Snapshot name is required."); return;
            }
            else
            {
                SnapshotName = tbInput.Text;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
