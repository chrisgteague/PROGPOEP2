using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PROGPOEP2_ST10083450
{
    /// <summary>
    /// Interaction logic for Registration_and_Login.xaml
    /// </summary>
    public partial class Registration_and_Login : Window
    {
        public Registration_and_Login()
        {
            InitializeComponent();
            Closing += RegAndLogClosing;
        }
        public SqlConnection sqlConnection;
        private void btnLoginPage_Click(object sender, RoutedEventArgs e)
        {
            gridRegisterOrLogin.Visibility = Visibility.Hidden;
            gridRegisterPage.Visibility = Visibility.Hidden;   
            gridLoginPage.Visibility = Visibility.Visible;
        }

        private void btnRegisterUser_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                string regUsername = tbxRegUsername.Text;
                string regPassword = pbRegPassword.Password.ToString();
                string regPasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(regPassword, 10);

                using (SqlConnection connection = new SqlConnection(ConnectionString.connStr))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Users (userName, passwordHash) VALUES (@UserName ,@PasswordHash)";
                        command.Parameters.AddWithValue("@UserName", regUsername);
                        command.Parameters.AddWithValue("@PasswordHash", regPasswordHash);
                        command.ExecuteNonQuery();
                    }
                  
                }
            gridRegisterPage.Visibility = Visibility.Hidden;
            gridLoginPage.Visibility = Visibility.Visible;


            }
            catch (Exception ex)
            {
               MessageBox.Show("Errrrrrrrrror");
            }
        }

        private void btnLoginAccount_Click(object sender, RoutedEventArgs e)
        {   
            MainWindow mainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden;
            mainWindow.ShowDialog();
            this.Visibility = Visibility.Visible;

            
        }

        private void RegAndLogClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         
            Application.Current.Shutdown();
        }

        private void btnRegisterPage_Click(object sender, RoutedEventArgs e)
        {
            gridRegisterOrLogin.Visibility = Visibility.Hidden;
            gridRegisterPage.Visibility= Visibility.Visible;
            
        }
    }
}
