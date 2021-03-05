using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace DesAlgorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }




        public static string Encrypt(string message, string key, byte [] IV )
        {
            if (key.Length == 8)
            {
            
                // Encode message and password
                byte[] messageBytes = ASCIIEncoding.ASCII.GetBytes(message);
                byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(key);

                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, IV);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and encrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(messageBytes, 0, messageBytes.Length);
                cryptoStream.FlushFinalBlock();

                // Read the encrypted message from the memory stream
                byte[] encryptedMessageBytes = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(encryptedMessageBytes, 0, encryptedMessageBytes.Length);

                // Encode the encrypted message as base64 string
                string encryptedMessage = Convert.ToBase64String(encryptedMessageBytes);

                return encryptedMessage;
            }
            else throw new Exception("Raktas turi buti 8-bitu");
        }


        public static string Decrypt(string encryptedMessage, string key, byte[] IV)
        {
            // Convert encrypted message and password to bytes
            byte[] encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
            byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(key);

            // Set encryption settings -- Use password for both key and init. vector
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, IV);
            CryptoStreamMode mode = CryptoStreamMode.Write;

            // Set up streams and decrypt
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
            cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Read decrypted message from memory stream
            byte[] decryptedMessageBytes = new byte[memStream.Length];
            memStream.Position = 0;
            memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

            // Encode deencrypted binary data to base64 string
            string message = ASCIIEncoding.ASCII.GetString(decryptedMessageBytes);

            return message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                Encoding encoding = Encoding.GetEncoding("437");
                byte[] IV = encoding.GetBytes(textBox2.Text);

                string text = textBox1.Text;
                string key = Raktas.Text;

               
                string encryptedString = Encrypt(text, key, IV);
                textBox3.Text = encryptedString;
               
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Encoding encoding = Encoding.GetEncoding("437");
                byte[] IV = encoding.GetBytes(textBox2.Text);

                string text = textBox4.Text;
                string key = Raktas.Text;

                string decryptedString = Decrypt(text, key, IV);
                textBox6.Text = decryptedString;

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            var random = new Random();
            byte[] IV = new byte[8];
            random.NextBytes(IV);
            Encoding encoding = Encoding.GetEncoding("437");
            textBox2.Text = encoding.GetString(IV);
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "File selection";
            fdlg.InitialDirectory = @"C:\";
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                string[] lines = System.IO.File.ReadAllLines(fdlg.FileName);
                textBox4.Text = lines[0];
                textBox2.Text = lines[1];
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            File.WriteAllText("C:\\Users\\TobY\\Desktop\\sidel\\SIDEL.txt", textBox3.Text + Environment.NewLine + textBox2.Text);
            MessageBox.Show("Saved to C:\\Users\\TobY\\Desktop\\sidel\\SIDEL.txt", "Saved Log File", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

   