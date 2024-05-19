using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Text;

namespace DatabaseManagers
{
	public static class Encryption
	{
		private const string Key = "14gtpsdpssdgomss12f";

		public static string EncryptString(string plainText)
		{
			byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
			byte[] keyBytes = Encoding.UTF8.GetBytes(Key);

			byte[] cipherBytes = new byte[plainBytes.Length];

			for (int i = 0; i < plainBytes.Length; i++)
			{
				cipherBytes[i] = (byte)(plainBytes[i] ^ keyBytes[i % keyBytes.Length]);
			}

			return Convert.ToBase64String(cipherBytes);
		}

		public static string DecryptString(string cipherText)
		{
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			byte[] keyBytes = Encoding.UTF8.GetBytes(Key);

			byte[] plainBytes = new byte[cipherBytes.Length];

			for (int i = 0; i < cipherBytes.Length; i++)
			{
				plainBytes[i] = (byte)(cipherBytes[i] ^ keyBytes[i % keyBytes.Length]);
			}

			return Encoding.UTF8.GetString(plainBytes);
		}
	}
}
