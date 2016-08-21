using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES_doer
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) {

                string input = "";
                do
                {
                    Console.Out.WriteLine("[E]ncryption, [D]ecription, or [C]hain decryption?");
                    input = Console.In.ReadLine();
                } while (!(input.ToUpper().Contains("E") ||  input.ToUpper().Contains("D") || input.ToUpper().Contains("C")));

                if (input.ToUpper()[0] == 'E')
                {
                    encrypt();
                }
                else if (input.ToUpper()[0] == 'D')
                {
                    decrypt();
                }
                else
                {
                    //chaindecrypt
                    cdecrypt();
                }

            }
        }

        static void encrypt()
        {
            //define sboxes
            String[,] sbox1 = new String[2, 8] { { "101", "010", "001", "110", "011", "100", "111", "000" },
                                                 { "001", "100", "110", "010", "000", "111", "101", "011" } };

            String[,] sbox2 = new String[2, 8] { { "100", "000", "110", "101", "111", "001", "011", "010" },
                                                 { "101", "011", "000", "111", "110", "010", "001", "100" } };

            //for input
            String kin = "", loro = "";
            int round = 0;
            int rounds = 0;

            //validate
            do
            {
                Console.Out.WriteLine("Please enter your key:");
                kin = Console.In.ReadLine();
            } while (!isValid(kin) && kin.Length == 9);


            do
            {
                Console.Out.WriteLine("Please input the round number:");
                round = int.Parse(Console.In.ReadLine());
            } while (!isValid(kin) && kin.Length == 9);

            do
            {
                Console.Out.WriteLine("Please input the number of rounds to complete:");
                rounds = int.Parse(Console.In.ReadLine());
            } while (!isValid(kin) && kin.Length == 9);

            do
            {
                Console.Out.WriteLine("Please enter L" + (round) + "R" + (round) + ":");
                loro = Console.In.ReadLine();
            } while (!isValid(loro) && loro.Length == 12);

            for (int i = 0; i < rounds; i++, round++)
            {
                //break in half
                String l = loro.Substring(0, 6);
                String r = loro.Substring(6);

                //expand
                String er = new string(new char[] { r[0], r[1], r[3], r[2], r[3], r[2], r[4], r[5] });

                //get the key
                String rk = kin.Substring(round) + kin.Substring(0, round);
                rk = rk.Substring(0, 8);

                Console.Out.WriteLine("K" + (i + 1) + ": " + rk);

                //xor
                String pres = Convert.ToString(Convert.ToInt32(er, 2) ^ Convert.ToInt32(rk, 2), 2);
                pres = pres.PadLeft(8, '0');

                String sl = pres.Substring(0, 4);
                String sr = pres.Substring(4);

                //sboxes
                sl = sbox1[int.Parse(sl[0].ToString()), Convert.ToInt32(sl.Substring(1), 2)];
                sr = sbox2[int.Parse(sr[0].ToString()), Convert.ToInt32(sr.Substring(1), 2)];

                //combine and XOR with L
                String rn = Convert.ToString(Convert.ToInt32(sl + sr, 2) ^ Convert.ToInt32(l, 2), 2);
                rn = rn.PadLeft(6, '0');

                //final result
                loro = r + rn;
                Console.Out.WriteLine("L" + (i+1) + "R" + (i+1) + ": " + loro);

            }

            Console.Out.Write("Press any key to be returned.");
            Console.In.ReadLine();
        }


        //interesting note... even after building this I have no idea why it works.
        //the encryption and decryption process seem so similar it's insane...
        static void decrypt()
        {
            //define sboxes
            String[,] sbox1 = new String[2, 8] { { "101", "010", "001", "110", "011", "100", "111", "000" },
                                                 { "001", "100", "110", "010", "000", "111", "101", "011" } };

            String[,] sbox2 = new String[2, 8] { { "100", "000", "110", "101", "111", "001", "011", "010" },
                                                 { "101", "011", "000", "111", "110", "010", "001", "100" } };

            //for input
            String kin = "", loro = "";
            int round = 0;

            //validate
            do
            {
                Console.Out.WriteLine("Please enter your key:");
                kin = Console.In.ReadLine();
            } while (!isValid(kin) && kin.Length == 9);

            do
            {
                Console.Out.WriteLine("Please input the round number:");
                round = int.Parse(Console.In.ReadLine());
            } while (!isValid(kin) && kin.Length == 9);

            do
            {
                Console.Out.WriteLine("Please enter L" + (round) + "R" + (round) + ":");
                loro = Console.In.ReadLine();
            } while (!isValid(loro) && loro.Length == 12);

            for (int i = round; i > 0; i--, round--)
            {
                //break in half
                String r = loro.Substring(0, 6);
                String l = loro.Substring(6);

                //expand
                String er = new string(new char[] { r[0], r[1], r[3], r[2], r[3], r[2], r[4], r[5] });

                //get the key
                String rk = kin.Substring(round-1) + kin.Substring(0, round-1);
                rk = rk.Substring(0, 8);

                Console.Out.WriteLine("K" + (i - 1) + ": " + rk);

                //xor
                String pres = Convert.ToString(Convert.ToInt32(er, 2) ^ Convert.ToInt32(rk, 2), 2);
                pres = pres.PadLeft(8, '0');

                String sl = pres.Substring(0, 4);
                String sr = pres.Substring(4);

                //sboxes
                sl = sbox1[int.Parse(sl[0].ToString()), Convert.ToInt32(sl.Substring(1), 2)];
                sr = sbox2[int.Parse(sr[0].ToString()), Convert.ToInt32(sr.Substring(1), 2)];

                //combine and XOR with L
                String rn = Convert.ToString(Convert.ToInt32(sl + sr, 2) ^ Convert.ToInt32(l, 2), 2);
                rn = rn.PadLeft(6, '0');

                //final result
                loro = rn + r;
                Console.Out.WriteLine("L" + (i - 1) + "R" + (i - 1) + ": " + loro);

            }

            Console.Out.Write("Press any key to be returned.");
            Console.In.ReadLine();
        }

        static void cdecrypt()
        {
            //define sboxes
            String[,] sbox1 = new String[2, 8] { { "101", "010", "001", "110", "011", "100", "111", "000" },
                                                 { "001", "100", "110", "010", "000", "111", "101", "011" } };

            String[,] sbox2 = new String[2, 8] { { "100", "000", "110", "101", "111", "001", "011", "010" },
                                                 { "101", "011", "000", "111", "110", "010", "001", "100" } };

            //for input
            String kin = "", fin = "", iv="", output = "";
            int tr = 0;

            //validate
            do
            {
                Console.Out.WriteLine("Please enter your key:");
                kin = Console.In.ReadLine();
            } while (!isValid(kin) && kin.Length == 9);

            do
            {
                Console.Out.WriteLine("Please input the number of rounds:");
                tr = int.Parse(Console.In.ReadLine());
            } while (!isValid(kin) && kin.Length == 9);

            do
            {
                Console.Out.WriteLine("Please input the iv:");
                iv = Console.In.ReadLine();
            } while (!isValid(iv) && iv.Length == 12);

            do
            {
                Console.Out.WriteLine("Please enter a 12n block of text to decrypt: ");
                fin = Console.In.ReadLine();
            } while (!isValid(fin) && fin.Length % 12 == 0);

            //now break it into chunks...
            for (int j = 0; j < fin.Length; j += 12)
            {
                string loro = fin.Substring(j, 12);
                string niv = loro;

                int round = tr;


                for (int i = round; i > 0; i--, round--)
                {
                    //break in half
                    String r = loro.Substring(0, 6);
                    String l = loro.Substring(6);

                    //expand
                    String er = new string(new char[] { r[0], r[1], r[3], r[2], r[3], r[2], r[4], r[5] });

                    //get the key
                    String rk = kin.Substring(round - 1) + kin.Substring(0, round - 1);
                    rk = rk.Substring(0, 8);

                    //Console.Out.WriteLine("K" + (i - 1) + ": " + rk);

                    //xor
                    String pres = Convert.ToString(Convert.ToInt32(er, 2) ^ Convert.ToInt32(rk, 2), 2);
                    pres = pres.PadLeft(8, '0');

                    String sl = pres.Substring(0, 4);
                    String sr = pres.Substring(4);

                    //sboxes
                    sl = sbox1[int.Parse(sl[0].ToString()), Convert.ToInt32(sl.Substring(1), 2)];
                    sr = sbox2[int.Parse(sr[0].ToString()), Convert.ToInt32(sr.Substring(1), 2)];

                    //combine and XOR with L
                    String rn = Convert.ToString(Convert.ToInt32(sl + sr, 2) ^ Convert.ToInt32(l, 2), 2);
                    rn = rn.PadLeft(6, '0');

                    //almost final result
                    loro = rn + r;

                }

                //XOR with IV/previous block
                loro = Convert.ToString(Convert.ToInt32(loro, 2) ^ Convert.ToInt32(iv, 2), 2);
                loro = loro.PadLeft(12, '0');

                //Set IV for next pass
                iv = niv;

                //output
                Console.Out.WriteLine(j / 12 + ":\t" + loro);
            }


            Console.Out.Write("Press any key to be returned.");
            Console.In.ReadLine();
        }

        static bool isValid(string check)
        {

            string accepted = "01";

            foreach (char c in check)
            {
                if (!accepted.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }


        static Byte[] GetBytesFromBinaryString(String binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < binary.Length; i += 8)
            {
                String t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }

    }
}
