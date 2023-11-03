using Microsoft.Data.SqlClient;
using System;
using PROGPOEP2_Library_ST10083450;
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
using PROGPOEP2_Library_ST10083450.Models;
using BCrypt.Net;
using System.Security.Cryptography;

namespace PROGPOEP2_ST10083450
{
    /// <summary>
    /// Interaction logic for Registration_and_Login.xaml
    /// </summary>
    public partial class Registration_and_Login : Window
    {
        St10083450ProgPoeContext db = new St10083450ProgPoeContext();

        
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
                string regPasswordHash = GetSHA256Hash(regPassword);

               
               
                db.Users.Add(new User {UserName = regUsername, PasswordHash = regPasswordHash });
                db.SaveChanges();



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
            try
            { string logUsername = tbxloginUsername.Text;
              string logPassword = pbloginPassword.Password.ToString();
                string logPasswordHash = GetSHA256Hash(logPassword);

                var getUser = db.Users.FirstOrDefault(m => m.UserName == logUsername);

                if (getUser != null)
                {
                   string storedHashedPassword = getUser.PasswordHash;

                    
                    if (storedHashedPassword == logPasswordHash)
                    {
                         ListUtil.usersLoggedIn.Add(logUsername);
                         ListUtil.usersLoggedIn.Add(logPassword);
                         MainWindow mainWindow = new MainWindow();
                         this.Visibility = Visibility.Hidden;
                         mainWindow.ShowDialog();
                         this.Visibility = Visibility.Visible;
                    }else
                    {
                        MessageBox.Show("Incorrect Password");
                    }
                }
                else
                {
                    MessageBox.Show("Username does not match");
                }
                

               
               

               
            }catch (Exception ex)
            {
                MessageBox.Show("Please enter valid credentials");
            }
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

        static string GetSHA256Hash(string input)// This can be setup in your custom Class library
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
