using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Text;

namespace Project_UNIX.Common.Crypto
{
    public class RSA
    {
        public static string privateKey { get; private set; } = "";
        public static string publicKey { get; private set; } = "";
        public static void InitializeEncryption()
        {
            try
            {
                // ตำแหน่ง directory ปัจจุบันของแอปพลิเคชัน
                string currentDirectory = Directory.GetCurrentDirectory();

                // ตำแหน่งของแฟ้มข้อมูล private key และ public key ใน directory keys
                string privateKeyFilePath = Path.Combine(currentDirectory, "keys", "private-key.pem");
                string publicKeyFilePath = Path.Combine(currentDirectory, "keys", "public-key.pem");

                // Read the RSA private key from the PEM file
                privateKey = ReadRsaKeyFromPem(privateKeyFilePath, isPrivate: true);
                publicKey = ReadRsaKeyFromPem(publicKeyFilePath, isPrivate: false);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static string ReadRsaKeyFromPem(string filePath, bool isPrivate)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    PemReader pemReader = new PemReader(reader);

                    object keyObject = pemReader.ReadObject();

                    if (isPrivate)
                    {
                        RsaPrivateCrtKeyParameters privateKeyParams = (RsaPrivateCrtKeyParameters)keyObject;
                        RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(privateKeyParams);

                        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                        {
                            rsa.ImportParameters(rsaParams);
                            return rsa.ToXmlString(true);
                        }
                    }
                    else
                    {
                        RsaKeyParameters publicKeyParams = (RsaKeyParameters)keyObject;
                        RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParams);

                        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                        {
                            rsa.ImportParameters(rsaParams);
                            return rsa.ToXmlString(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading key from PEM file: {ex.Message}");
                return null;
            }
        }

        public static byte[] EncryptWithPublicKey(string data, string publicKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);

                    // Convert the data to bytes
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    // Encrypt the data with the public key
                    byte[] encryptedData = rsa.Encrypt(dataBytes, true);

                    return encryptedData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static string DecryptWithPrivateKey(byte[] encryptedData, string privateKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);

                    // Decrypt the data with the private key
                    byte[] decryptedData = rsa.Decrypt(encryptedData, true);

                    // Convert the decrypted data to a string
                    string decryptedString = Encoding.UTF8.GetString(decryptedData);

                    return decryptedString;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
