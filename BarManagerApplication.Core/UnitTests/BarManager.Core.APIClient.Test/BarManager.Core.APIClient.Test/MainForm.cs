using BarApplication.APIClient.Identity.Core;
using BarApplication.APIClient.Identity.Models.Requests;
using System;
using System.Windows.Forms;

namespace BarManager.Core.APIClient.Test
{
    public partial class MainForm : Form
    {
        private readonly IIdentityClient identityClient;

        public MainForm(IIdentityClient identityClient)
        {
            this.identityClient = identityClient;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string password = txtPassword.Text;

            var request = new LoginRequest
            {
                UserName = userName,
                Password = password
            };

            var response = await identityClient.LoginAsync(request);
            if (response != null)
            {
                var user = await identityClient.GetUserAsync(response.AccessToken);
                MessageBox.Show($"{user.FirstName} {user.LastName}");
            }
        }
    }
}