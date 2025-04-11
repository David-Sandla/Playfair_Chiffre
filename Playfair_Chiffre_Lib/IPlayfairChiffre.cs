namespace Playfair_Chiffre_Lib;

public interface IPlayfairChiffre{
    // Das geheime Wort, aus dem die 5x5-Matrix erstellt wird
    string Key { get; set; }
    // Verschlüsselt den als String übergebenen Klartext
    string Encrypt(string plaintext);
    // Entschlüsselt den als String übergebenen Geheimtext
    string Decrypt(string ciphertext);
    // Optionale Funktionalität: Verschlüsselt den Inhalt einer Textdatei und speichert das Ergebnis in einer Ausgabedatei
    void EncryptFile(string inputFilePath, string outputFilePath);
    // Optionale Funktionalität: Entschlüsselt eine Textdatei und speichert den Klartext in einer Ausgabedatei
    void DecryptFile(string inputFilePath, string outputFilePath);
}