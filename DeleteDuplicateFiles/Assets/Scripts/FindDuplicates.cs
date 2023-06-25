using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public class FindDuplicates : MonoBehaviour
{
    string folderPath = @"C:\Users\olesc\Desktop\Neuer Ordner (12)\jdk-19"; // Verzeichnis, in dem die Bilder gespeichert sind
    Dictionary<string, string> imageHashes = new Dictionary<string, string>(); // Sammlung von Hashes der Bilder
    List<string> duplicateImages = new List<string>(); // Liste der doppelten Bilder

    private void Start()
    {
        test();


        //FindDuplicateImages();
        //DeleteImages();
    }

    public void FindDuplicateImages()
    {
        // Schleife durch alle Bilder im Verzeichnis
        foreach (string imagePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Where(
            s => s.EndsWith(".png") || s.EndsWith(".jpg")
            ))
        {
            // Hash des Bildes berechnen
            string imageHash = GetImageHash(imagePath);
            Debug.Log(imageHash);

            // Wenn der Hash bereits in der Sammlung vorhanden ist, handelt es sich um ein Duplikat
            if (imageHashes.ContainsValue(imageHash))
            {
                // Duplikat zu Liste hinzufügen
                duplicateImages.Add(imagePath);

            }
            else
            {
                // Hash des Bildes zur Sammlung hinzufügen
                imageHashes.Add(imagePath, imageHash);
            }
        }
    }

    //Delete Duplicates
    public void DeleteImages()
    {
        foreach (string duplicateImagePath in duplicateImages)
        {
            Debug.Log(duplicateImagePath);
            File.Delete(duplicateImagePath);
        }
    }

    //SHA256 Hash function
    private string GetImageHash(string imagePath)
    {
        using (var sha256 = SHA256.Create())
        {
            using (var stream = File.OpenRead(imagePath))
            {
                Console.WriteLine(BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "").ToLower());
                return BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "").ToLower();
            }
        }
    }


    private void test()
    {
        foreach (string imagePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
        {
            byte[] hash = CalculateSHA256(imagePath);
            string hashString = BitConverter.ToString(hash).Replace("-", string.Empty);
            if (imageHashes.ContainsValue(hashString))
            {
                Debug.Log("duplicate");
                Debug.Log(imagePath);
                // Duplikat zu Liste hinzufügen
                duplicateImages.Add(imagePath);

            }
            else
            {
                // Hash des Bildes zur Sammlung hinzufügen
                imageHashes.Add(imagePath, hashString);
            }
            //Debug.Log("SHA256 hash: " + hashString);
        }


        
    }


    private byte[] CalculateSHA256(string filePath)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                return sha256.ComputeHash(fileStream);
            }
        }
    }









}
