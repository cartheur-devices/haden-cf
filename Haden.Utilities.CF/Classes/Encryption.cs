using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Haden.Utilities.CF {
	public class Encryption {
		static byte[] salt = new byte[] {
			0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
		};

		static SymmetricAlgorithm algorithm = new RijndaelManaged();

		public static byte[] Encrypt(byte[] clear, string password) {
			//SymmetricAlgorithm algorithm = new RijndaelManaged();
			Rfc2898DeriveBytes db = new Rfc2898DeriveBytes(password, salt);
			byte[] key = db.GetBytes(32);
			byte[] iv = db.GetBytes(16);
			ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
			cs.Write(clear, 0, clear.Length);
			cs.FlushFinalBlock();
			return ms.ToArray();
		}

		public static byte[] Decrypt(byte[] encrypted, string password) {
			//SymmetricAlgorithm algorithm = new RijndaelManaged();
			Rfc2898DeriveBytes db = new Rfc2898DeriveBytes(password, salt);
			byte[] key = db.GetBytes(32);
			byte[] iv = db.GetBytes(16);
			ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
			
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
			cs.Write(encrypted, 0, encrypted.Length);
			cs.FlushFinalBlock();
			return ms.ToArray();

		}
	}
}
