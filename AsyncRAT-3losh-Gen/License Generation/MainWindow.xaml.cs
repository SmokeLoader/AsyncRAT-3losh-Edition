using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace License_Generation
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            // I kindly ask you to not remove the following line:
            Process.Start("https://t.me/strikelab");
            LicenseKeyTextBox.Text = BinGen(UserIdTextBox.Text);
        }

        public static string BinGen(string id)
        {
            string[] iddParts = id.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);

            string combinedString = iddParts[1] + iddParts[1] + "3losh";

            byte[] md5Hash = ComputeMD5Hash(combinedString);

            byte[] key = new byte[32];
            Array.Copy(md5Hash, 0, key, 0, 5);
            Array.Copy(md5Hash, 0, key, 10, 5);

            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.Key = key;
                rijndael.Mode = CipherMode.ECB;
                rijndael.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = rijndael.CreateEncryptor())
                {
                    byte[] iddBytes = SBxx(id);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(iddBytes, 0, iddBytes.Length);

                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }
        public static byte[] ComputeMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }

        public static byte[] SBxx(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }
    }
}
