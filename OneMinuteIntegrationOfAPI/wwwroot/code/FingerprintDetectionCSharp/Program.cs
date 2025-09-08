// Face Detection API - One Minute Integration Example
// This file demonstrates how to call the MXFace.ai Face Detection API in a single C# file.
// Just copy-paste this code into your project, update your subscription key and image path, and run!

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
// For JSON serialization/deserialization
using Newtonsoft.Json;

namespace FingerprintDetectionAPI
{
    // Model for the API request
    public class APIRequest
    {
        // Base64-encoded image string
        public string encoded_image { get; set; }
    }

    // Model for the face rectangle returned by the API
    public class FaceRectangle
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    // Model for each detected face
    public class FaceDetect
    {
        public FaceRectangle FaceRectangle { get; set; }
        public float quality { get; set; }
    }

    // Model for the API response
    public class FaceDetectResponse
    {
        public List<FaceDetect> Faces { get; set; }
    }

    // Main class to call the Face Detection API
    public class MXFaceAPI
    {
        private readonly string _apiUrl;
        private readonly string _subscriptionKey;

        // Constructor to set API URL and subscription key
        public MXFaceAPI(string apiUrl, string subscriptionKey)
        {
            _apiUrl = apiUrl;
            _subscriptionKey = subscriptionKey;
        }

        // Detect faces in an image using the Face Detection API
        public async Task Detect(string imagePath)
        {
            // Read the image file and encode it as Base64
            string base64Image = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            // Prepare the API request object
            APIRequest request = new APIRequest { encoded_image = base64Image };

            // Serialize the request to JSON
            string jsonRequest = JsonConvert.SerializeObject(request);

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                httpClient.DefaultRequestHeaders.Add("subscriptionkey", _subscriptionKey);

                // Prepare the HTTP content
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Call the Face Detection API endpoint
                HttpResponseMessage response = await httpClient.PostAsync(
                    "/api/v3/face/detect",
                    httpContent
                );

                // Read the API response
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize the response to FaceDetectResponse
                    FaceDetectResponse detectFaces =
                        JsonConvert.DeserializeObject<FaceDetectResponse>(apiResponse);

                    // Print results for each detected face
                    if (detectFaces?.Faces != null && detectFaces.Faces.Count > 0)
                    {
                        Console.WriteLine($"Detected {detectFaces.Faces.Count} face(s):");
                        foreach (var face in detectFaces.Faces)
                        {
                            Console.WriteLine(
                                $"Face Quality: {face.quality}, Rectangle: x={face.FaceRectangle.x}, y={face.FaceRectangle.y}, width={face.FaceRectangle.width}, height={face.FaceRectangle.height}"
                            );
                        }
                    }
                    else
                    {
                        Console.WriteLine("No faces detected in the image.");
                    }
                }
                else
                {
                    // Print error details
                    Console.WriteLine($"Error {response.StatusCode}: {apiResponse}");
                }
            }
        }
    }

    // Entry point for the application
    class Program
    {
        static async Task Main(string[] args)
        {
            // 1. Set your API subscription key here
            string subscriptionKey = "PdVLlEYd7Ig51n3oTJ-00RIdBGO6I897"; // <-- Replace with your MXFace.ai subscription key

            // 2. Set the path to your image file here
            string imagePath = @"rohit_sharma.jpg"; // <-- Replace with your image file path

            // 3. Create the API client
            MXFaceAPI mxfaceAPI = new MXFaceAPI("https://faceapi.mxface.ai", subscriptionKey);

            Console.WriteLine("Calling Face Detect API...");
            await mxfaceAPI.Detect(imagePath);

            Console.WriteLine("Done. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}

// ----------------------
// Integration Instructions:
// 1. Install Newtonsoft.Json via NuGet:
//    dotnet add package Newtonsoft.Json
// 2. Copy this file into your project.
// 3. Replace 'YOUR_SUBSCRIPTION_KEY' with your actual MXFace.ai subscription key.
// 4. Replace 'Leonardo.jpg' with the path to your image file.
// 5. Run the program. It will print detected face rectangles and quality scores.
// ----------------------
