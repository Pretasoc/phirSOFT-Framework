using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace phirSOFT.Common.Controls.Tasks
{
    public partial class TaskOverview : Form
    {
        public TaskOverview()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var prog = toolStripTaskHost1.RegisterProgress();
            var t = new Task(() =>
            {
                var rnd = new Random();
                var max = rnd.Next(5, 15);
                for (int i = 0; i < max; i++)
                {
                    prog.Report(new ProgressInfo() {PercentComplete = (float) i/max, Title = "Doing something.."});
                    Thread.Sleep(max*200);
                }
                prog.Report(new ProgressInfo() {PercentComplete = 1, Title = "Done something.."});
            });
            toolStripTaskHost1.StartTask(t, prog);
        }
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new TaskOverview());
        }

        private void TaskOverview_Load(object sender, EventArgs e)
        {

        }
    }
}
