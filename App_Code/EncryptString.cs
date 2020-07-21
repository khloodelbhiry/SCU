using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Encrypt String
/// </summary>
public class EncryptString
{
    private static string _key = "AQWSEDRF@SCU";

    public EncryptString()
    {
    }


    public static string Key
    {
        set
        {
            _key = value;
        }
    }

    /// <summary>
    /// Encrypt the given string using the default key.
    /// </summary>
    /// <param name="strToEncrypt">The string to be encrypted.</param>
    /// <returns>The encrypted string.</returns>
    public static string Encrypt(string strToEncrypt)
    {
        try
        {
            return Encrypt(strToEncrypt, _key);
        }
        catch (Exception ex)
        {
            return "Wrong Input. " + ex.Message;
        }

    }

    /// <summary>
    /// Decrypt the given string using the default key.
    /// </summary>
    /// <param name="strEncrypted">The string to be decrypted.</param>
    /// <returns>The decrypted string.</returns>
    public static string Decrypt(string strEncrypted)
    {
        try
        {
            return Decrypt(strEncrypted, _key);
        }
        catch (Exception ex)
        {
            //return "Wrong Input. " + ex.Message; //Hamed //09-06-2013
            throw ex;
        }
    }

    /// <summary>
    /// Encrypt the given string using the specified key.
    /// </summary>
    /// <param name="strToEncrypt">The string to be encrypted.</param>
    /// <param name="strKey">The encryption key.</param>
    /// <returns>The encrypted string.</returns>
    public static string Encrypt(string strToEncrypt, string strKey)
    {
        try
        {
            TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();

            byte[] byteHash, byteBuff;
            string strTempKey = strKey;

            byteHash = objHashMD5.ComputeHash(Encoding.UTF8.GetBytes(strTempKey));
            objHashMD5 = null;
            objDESCrypto.Key = byteHash;
            objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

            byteBuff = Encoding.UTF8.GetBytes(strToEncrypt);
            return Convert.ToBase64String(objDESCrypto.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length)).Replace("+", "-").Replace('/', '_').Trim();
        }
        catch (Exception ex)
        {
            return "Wrong Input. " + ex.Message;
        }
    }


    /// <summary>
    /// Decrypt the given string using the specified key.
    /// </summary>
    /// <param name="strEncrypted">The string to be decrypted.</param>
    /// <param name="strKey">The decryption key.</param>
    /// <returns>The decrypted string.</returns>
    public static string Decrypt(string strEncrypted, string strKey)
    {
        try
        {
            TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();

            byte[] byteHash, byteBuff;
            string strTempKey = strKey;

            byteHash = objHashMD5.ComputeHash(Encoding.UTF8.GetBytes(strTempKey));
            objHashMD5 = null;
            objDESCrypto.Key = byteHash;
            objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

            byteBuff = Convert.FromBase64String(strEncrypted.Replace("-", "+").Replace('_', '/'));
            string strDecrypted = Encoding.UTF8.GetString(objDESCrypto.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            objDESCrypto = null;

            return strDecrypted;
        }
        catch (Exception ex)
        {
            return "Wrong Input. " + ex.Message;
        }
    }
}
