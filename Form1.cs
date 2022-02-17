using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using lw1_IS_2;

namespace lw1_IS
{
    public partial class Form1 : Form
    {
        int[] S1 = new int[] {11, 14, 15, 12, 5, 8, 7, 9, 11, 13, 14, 15, 6, 7, 9, 8,
                            7, 6, 8, 13, 11, 9, 7, 15, 7, 12, 15, 9, 11, 7, 13, 12,
                            11, 13, 6, 7, 14, 9, 13, 15, 14, 8, 13, 6, 5, 12, 7, 5,
                            11, 12, 14, 15, 14, 15, 9, 8, 9, 14, 5, 6, 8, 6, 5, 12,
                            9, 15, 5, 11, 6, 8, 13, 12, 5, 12, 13, 14, 11, 8, 5, 6 };

        int[] S2 = new int[] { 8, 9, 9, 11, 13, 15, 15, 5, 7, 7, 8, 11, 14, 14, 12, 6,
                                9, 13, 15, 7, 12, 8, 9, 11, 7, 7, 12, 7, 6, 15, 13, 11,
                                9, 7, 15, 11, 8, 6, 6, 14, 12, 13, 5, 14, 13, 13, 7, 5,
                                15, 5, 8, 11, 14, 14, 6, 14, 6, 9, 12, 9, 12, 5, 15, 8,
                                8, 5, 12, 9, 12, 5, 14, 6, 8, 13, 6, 5, 15, 13, 11, 11 };

        int[] R1 = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                                 7, 4, 13, 1, 10, 6, 15, 3, 12, 0, 9, 5, 2, 14, 11, 8,
                                 3, 10, 14, 4, 9, 15, 8, 1, 2, 7, 0, 6, 13, 11, 5, 12,
                                 1, 9, 11, 10, 0, 8, 12, 4, 13, 3, 7, 15, 14, 5, 6, 2,
                                 4, 0, 5, 9, 7, 12, 2, 10, 14, 1, 3, 8, 11, 6, 15, 13 };

        int[] R2 = new int[] { 5, 14, 7, 0, 9, 2, 11, 4, 13, 6, 15, 8, 1, 10, 3, 12,
                                 6, 11, 3, 7, 0, 13, 5, 10, 14, 15, 8, 12, 4, 9, 1, 2,
                                 15, 5, 1, 3, 7, 14, 6, 9, 11, 8, 12, 2, 10, 0, 4, 13,
                                 8, 6, 4, 1, 3, 11, 15, 0, 5, 12, 2, 13, 9, 7, 10, 14,
                                 12, 15, 10, 4, 1, 5, 8, 7, 6, 2, 13, 14, 0, 3, 9, 11  };

        uint f(int j, uint x, uint y, uint z)
        {
            if (0 <= j && j <= 15)
                return x ^ y ^ z;
            else if (16 <= j && j <= 31)
                return x & y | ~x & z;
            else if (32 <= j && j <= 47)
                return (x | ~y) ^ z;
            else if (48 <= j && j <= 63)
                return x & z | y & ~z;
            else return 0;

        }

        uint K1(int j)
        {
            if (0 <= j && j <= 15)
                return 0x00000000;
            if (16 <= j && j <= 31)
                return 0x5a827999;
            if (32 <= j && j <= 47)
                return 0x6ed9eba1;
            if (48 <= j && j <= 63)
                return 0x8f1bbcdc;
            return 0;
        }

        uint K2(int j)
        {
            if (0 <= j && j <= 15)
                return 0x50a28be6;
            if (16 <= j && j <= 31)
                return 0x5c4dd124;
            if (32 <= j && j <= 47)
                return 0x6d703ef3;
            if (48 <= j && j <= 63)
                return 0x00000000;
            return 0;
        }

        uint[,] NumOfChanBit1 = new uint[64, 8];

        uint[,] NumOfChanBit2 = new uint[64, 8];

        void Decode(ref uint[] output, byte[] input, uint len)
        {
            for (uint i = 0, j = 0; j < len; i++, j += 4)
                output[i] = ((uint)input[j]) | (((uint)input[j + 1]) << 8) | (((uint)input[j + 2]) << 16) | (((uint)input[j + 3]) << 24);
        }

        string hexdigest(uint[] h)
        {
            uint[] output = new uint[16];
            for (uint i = 0, j = 0; j < 16; i++, j += 4)
            {
                output[j] = (h[i] & 0xff);
                output[j + 1] = ((h[i] >> 8) & 0xff);
                output[j + 2] = ((h[i] >> 16) & 0xff);
                output[j + 3] = ((h[i] >> 24) & 0xff);
            }
            string result = "";
            for (int i = 0; i < 16; i++)
            {
                result += Convert.ToString(output[i], 16);
                if ((i + 1) % 4 == 0) result += " ";
            }
            return result;
        }

        uint Rotate(uint x, int n)
        {
            return (x << n) | (x >> 32 - n);
        }

        int GetDiffBit(uint input)
        {
            string tmp = Convert.ToString(input, 2);
            int result = 0;
            foreach (char c in tmp)
                if (c == '1') result += 1;
            return result;
        }

        void GetDiffBits(ref int[] output)
        {
            for (int i = 0; i < 64; i++)
            {
                output[i] = 0;
                for (int j = 0; j < 8; j++)
                    output[i] += GetDiffBit(NumOfChanBit1[i, j] ^ NumOfChanBit2[i, j]);
            }
        }

        void plot()
        {
            int[] diff = new int[64];
            GetDiffBits(ref diff);
            ChartForm cf = new ChartForm(diff);
            cf.Show();
        }

        public string Encoder(string input = "")
        {
            uint length = (uint)input.Length;
            uint rest = length % 64, size;

            // Step 1
            if (rest < 56)
                size = 56 - rest + length + 8;
            else
                size = 64 - rest + 56 + length + 8;

            byte[] message = new byte[size];
            for (int i = 0; i < length; i++)
                message[i] = (byte)input[i];

            //adding 1
            message[length] = 0x80;

            //add zero, while lenght != 448(mod 512) 
            for (uint i = length + 1; i < size; i++)
                message[i] = 0;

            //current block
            byte[] chunk = new byte[64];

            //add length of message
            ulong length64 = (ulong)length * 8;
            uint length64_l = (uint)length64;
            uint length64_r = (uint)(length64 << 32);

            uint[] X = new uint[16];

            //step 3 - Buffer initialization
            uint T;
            uint[] h = new uint[] { 0x67452301, 0xEFCDAB89, 0x98BADCFE, 0x10325476 };
            string result = "";

            // step 4 - Main Loop
            for (int i = 0; i <= (size - 64); i += 64)
            {
                for (int j = 0; j < 64; j++)
                    chunk[j] = message[i + j];

                Decode(ref X, chunk, 64);

                if (i == (size - 64)) { X[14] = length64_l; X[15] = length64_r; };

                uint A1 = h[0], B1 = h[1], C1 = h[2], D1 = h[3];
                uint A2 = h[0], B2 = h[1], C2 = h[2], D2 = h[3];
                uint A1_, B1_, C1_, D1_;
                uint A2_, B2_, C2_, D2_;
                for (int j = 0; j < 64; j++)
                {
                    for (int u = 0; u < 8; u++)
                        NumOfChanBit1[j, u] = NumOfChanBit2[j, u];

                    NumOfChanBit2[j, 0] = A1; NumOfChanBit2[j, 1] = B1; NumOfChanBit2[j, 2] = C1; NumOfChanBit2[j, 3] = D1;
                    NumOfChanBit2[j, 4] = A2; NumOfChanBit2[j, 5] = B2; NumOfChanBit2[j, 6] = C2; NumOfChanBit2[j, 7] = D2;
                    T = Rotate((A1 + f(j, B1, C1, D1) + X[R1[j]] + K1(j)), S1[j]);
                    A1 = D1; D1 = C1; C1 = B1; B1 = T;
                    T = Rotate((A2 + f(63 - j, B2, C2, D2) + X[R2[j]] + K2(j)), S2[j]);
                    A2 = D2; D2 = C2; C2 = B2; B2 = T;

                }
                T = h[1] + C1 + D2;
                h[1] = h[2] + D1 + A2;
                h[2] = h[3] + A1 + B2;
                h[3] = h[0] + B1 + C2;
                h[0] = T;
            }
            result += hexdigest(h);
            return result;
        }

        void init()
        {
            loadfile();
            comboBox1.Items.AddRange(new string[] { "", "abc", "12345678901234567890123456789012345678901234567890123456789012345678901234567890", "aaa100" });
        }

        public Form1()
        {
            InitializeComponent();
            init();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        void loadfile()
        {
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                String A = sr.ReadToEnd();
                //input.Text = A.Remove(A.Length - 2, 2);
                input.Text = A;
            }
            output.Text = Encoder(input.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:/Users/dimon/OneDrive/NETI/Vlll semester/Information Security/lw 1/lw1_IS";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        String A = reader.ReadToEnd();
                        //input.Text = A.Remove(A.Length - 2, 2);
                        input.Text = A;
                    }
                }
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            output.Text = Encoder(input.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            input.Text = comboBox1.SelectedItem.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            output.Text = Encoder(input.Text);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("output.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(output.Text);
            }
            using (StreamWriter sw = new StreamWriter("input.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(input.Text);
            }
            this.Close();
        }

        private void bitNumber_TextChanged(object sender, EventArgs e)
        {
            if (bitNumber.Text.Length > 0)
            {
                if (bitNumber.Text[0] == '-')
                    MessageBox.Show("Please enter a valid bit number", "Incorret data", MessageBoxButtons.OK);
                else if (Convert.ToInt32(bitNumber.Text) > input.Text.Length * 8)
                    MessageBox.Show("Please enter a valid bit number", "Incorret data", MessageBoxButtons.OK);

                int bit = Convert.ToInt32(bitNumber.Text);
                int n = (bit - 1) / 8;

                char a = input.Text[n];
                int i = (1 << (8 - (bit - n * 8)));
                a ^= (char)i;
                string tmp = "";
                for (int l = 0; l < input.Text.Length; l++)
                    if (l == n)
                        tmp += a;
                    else tmp += input.Text[l];

                inputAval.Text = tmp;
                outputAval.Text = Encoder(tmp);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (bitNumber.Text.Length > 0)
            {
                if (bitNumber.Text[0] == '-')
                    MessageBox.Show("Please enter a valid bit number", "Incorret data", MessageBoxButtons.OK);
                else if (Convert.ToInt32(bitNumber.Text) > input.Text.Length * 8)
                    MessageBox.Show("Please enter a valid bit number", "Incorret data", MessageBoxButtons.OK);

                int bit = Convert.ToInt32(bitNumber.Text);
                int n = (bit - 1) / 8;

                char a = input.Text[n];
                int i = (1 << (8 - (bit - n * 8)));
                a ^= (char)i;
                string tmp = "";
                for (int l = 0; l < input.Text.Length; l++)
                    if (l == n)
                        tmp += a;
                    else tmp += input.Text[l];

                inputAval.Text = tmp;
                outputAval.Text = Encoder(tmp);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Encoder(input.Text);
            Encoder(inputAval.Text);
            plot();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
