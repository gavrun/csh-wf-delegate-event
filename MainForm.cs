using System;
using System.Windows.Forms;

namespace csh_wf_delegate_event
{
    // Class for custom events
    public class CloseFormEventArgs : EventArgs
    {
        public string Message { get; set; }

        public CloseFormEventArgs(string message)
        {
            Message = message;
        }
    }

    // Form class acting as Publisher class
    public partial class MainForm : Form
    {
        public event EventHandler<CloseFormEventArgs>? CloseFormEvent; // Event for closing the form

        private Button closeButton; // Closing button

        public MainForm()
        {
            InitializeComponent();

            // Create button and adding it on the form
            closeButton = new Button
            {
                Text = "Close Form",
                Location = new System.Drawing.Point(100, 100),
                Size = new System.Drawing.Size(100, 50)
            };

            // Subscribe to Resize event for centering the button
            this.Resize += MainForm_Resize;

            // Subscribe to Click button event
            closeButton.Click += CloseButton_Click;

            Controls.Add(closeButton);
            CenterButton(); // Сentering the button when creating a form
        }

        // The method of initializing control and components
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Name = "MainForm";
            this.Text = "Main Form";
            this.StartPosition = FormStartPosition.CenterScreen; // Center of the screen
            this.ResumeLayout(false);
        }

        // Button centering method
        private void CenterButton()
        {
            closeButton.Location = new System.Drawing.Point(
                (this.ClientSize.Width - closeButton.Width) / 2,   // width
                (this.ClientSize.Height - closeButton.Height) / 2  // height
            );
        }

        // Resize event handler
        private void MainForm_Resize(object sender, EventArgs e)
        {
            CenterButton();
        }

        // The method handler for clicking on the button
        private void CloseButton_Click(object sender, EventArgs e)
        {
            // Generation of CloseFormEvent event
            OnCloseFormEvent(new CloseFormEventArgs("Form is closing..."));
        }

        // Method for calling an event
        protected virtual void OnCloseFormEvent(CloseFormEventArgs e)
        {
            CloseFormEvent?.Invoke(this, e); // Call the event if there are subscribers
        }
    }

    // Subscriber class 
    public class FormCloser
    {
        private MainForm form;

        public FormCloser(MainForm form)
        {
            this.form = form;
            // Subscribe to CloseFormEvent event
            form.CloseFormEvent += HandleCloseFormEvent;
        }

        // Method-handler of the event
        private void HandleCloseFormEvent(object sender, CloseFormEventArgs e)
        {
            MessageBox.Show(e.Message); // Show message before closing
            form.Close(); 
        }
    }

    // Entry point
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create form
            var mainForm = new MainForm();

            // Create a subscriber for processing closing event
            var closer = new FormCloser(mainForm);

            // Run application
            Application.Run(mainForm);
        }
    }
}
