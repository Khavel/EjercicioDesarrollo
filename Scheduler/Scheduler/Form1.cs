using System;
using System.Windows.Forms;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        private Schedule sched;
        public Form1()
        {
            InitializeComponent();
            cmbType.DataSource = Enum.GetValues(typeof(SchedulerType));
            cmbOccurs.DataSource = Enum.GetValues(typeof(FrequencyType));
        }

        private void btNextDate_Click(object sender, EventArgs e)
        {
            if(dtCurrent.Value == null)
            {
                MessageBox.Show("Please input a valid date");
                return;
            }
            SchedulerConfig configuration = new SchedulerConfig()
            {
                Type = (SchedulerType)cmbType.SelectedItem,
                Frequency = (FrequencyType)cmbType.SelectedItem,
                StartDate = dtStart.Value,
                EndDate = dtEnd.Value,
                InitialDateTime = dtDateTime.Value,
                Interval = Convert.ToInt32(txFrequency.Text)
            };
            if(sched == null)
            {
                sched = new Schedule(configuration);
            }
            txNextExec.Text = sched.GetNextExecutionTime().ToString("dd/MM/yyyy HH:mm");
            txDescription.Text = sched.GetDescription();
        }

    }
}
