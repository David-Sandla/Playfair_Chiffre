namespace Playfair_Chiffre_Lib;

public interface IChriffre
{
    string Encrypt(string msg);
    string Decrypt(string msg);
}