using System;
using System.Windows.Forms;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbType.DataSource = Enum.GetValues(typeof(SchedulerType));
            cmbOccurs.DataSource = Enum.GetValues(typeof(FrequencyType));
            
            txFrequency.Text = "1";
        }

        private void btNextDate_Click(object sender, EventArgs e)
        {
            if(dtCurrent.Value == null)
            {
                MessageBox.Show("Please input a valid date");
                return;
            }
            if (string.IsNullOrWhiteSpace(txFrequency.Text) || int.TryParse(txFrequency.Text,out _) == false)
            {
                MessageBox.Show("Please input a valid frequency");
                return;
            }
            try
            {
                SchedulerConfiguration configuration = new SchedulerConfiguration(
                    (SchedulerType)cmbType.SelectedItem,
                    (FrequencyType)cmbOccurs.SelectedItem,
                    dtDateTime.Value,
                    Convert.ToInt32(txFrequency.Text),
                    dtStart.Value,
                    dtEnd.Value);
                configuration.validateConfiguration();
                Schedule sched = new Schedule(configuration);
                DateTime? nextExecution = sched.GetNextExecutionTime(dtCurrent.Value);
                if(nextExecution.HasValue == true)
                {
                    txNextExec.Text = nextExecution.Value.ToString("dd/MM/yyyy HH:mm");
                }
                txDescription.Text = sched.GetDescription(dtCurrent.Value);
            }
            catch(ConfigurationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (ScheduleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dtEnd_ValueChanged(object sender, EventArgs e)
        {
            dtEnd.CustomFormat = "dd/MM/yyyy";
        }

        private void dtDateTime_ValueChanged(object sender, EventArgs e)
        {
            dtDateTime.CustomFormat = "dd/MM/yyyy HH:mm";
        }

    }
}
