using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ankama.Cube.Utility
{
	public static class StringCipher
	{
		private const int Keysize = 256;

		private const int DerivationIterations = 1000;

		public static string Encrypt(string plainText, string passPhrase)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			byte[] array = Generate256BitsOfRandomEntropy();
			byte[] array2 = Generate256BitsOfRandomEntropy();
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			byte[] bytes2 = new Rfc2898DeriveBytes(passPhrase, array, 1000).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			try
			{
				rijndaelManaged.set_BlockSize(256);
				rijndaelManaged.set_Mode(1);
				rijndaelManaged.set_Padding(2);
				ICryptoTransform val = rijndaelManaged.CreateEncryptor(bytes2, array2);
				try
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						CryptoStream val2 = new CryptoStream((Stream)memoryStream, val, 1);
						try
						{
							((Stream)val2).Write(bytes, 0, bytes.Length);
							val2.FlushFinalBlock();
							byte[] inArray = array.Concat(array2).ToArray().Concat(memoryStream.ToArray())
								.ToArray();
							memoryStream.Close();
							((Stream)val2).Close();
							return Convert.ToBase64String(inArray);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)rijndaelManaged)?.Dispose();
			}
		}

		public static string Decrypt(string cipherText, string passPhrase)
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			byte[] array = Convert.FromBase64String(cipherText);
			byte[] salt = array.Take(32).ToArray();
			byte[] array2 = array.Skip(32).Take(32).ToArray();
			byte[] array3 = array.Skip(64).Take(array.Length - 64).ToArray();
			byte[] bytes = new Rfc2898DeriveBytes(passPhrase, salt, 1000).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			try
			{
				rijndaelManaged.set_BlockSize(256);
				rijndaelManaged.set_Mode(1);
				rijndaelManaged.set_Padding(2);
				ICryptoTransform val = rijndaelManaged.CreateDecryptor(bytes, array2);
				try
				{
					using (MemoryStream memoryStream = new MemoryStream(array3))
					{
						CryptoStream val2 = new CryptoStream((Stream)memoryStream, val, 0);
						try
						{
							byte[] array4 = new byte[array3.Length];
							int count = ((Stream)val2).Read(array4, 0, array4.Length);
							memoryStream.Close();
							((Stream)val2).Close();
							return Encoding.UTF8.GetString(array4, 0, count);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)rijndaelManaged)?.Dispose();
			}
		}

		private static byte[] Generate256BitsOfRandomEntropy()
		{
			byte[] array = new byte[32];
			new RNGCryptoServiceProvider().GetBytes(array);
			return array;
		}
	}
}
