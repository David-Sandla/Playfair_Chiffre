namespace ChiffreDLL;

public interface IChriffre
{
    string Encrypt(string msg);
    string Decrypt(string msg);
}