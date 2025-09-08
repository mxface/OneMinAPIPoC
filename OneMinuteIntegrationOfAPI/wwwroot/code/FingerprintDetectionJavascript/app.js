// Face Detection API - One Minute Integration Example
// This file demonstrates how to call the MXFace.ai Face Detection API in a single JavaScript file (Node.js).
// Just copy-paste this code into your project, update your subscription key and image path, and run!

// Required modules
const fs = require("fs");
const path = require("path");
const https = require("https");

// You can use 'node-fetch' or 'axios' for easier HTTP requests. Here, we use 'node-fetch':
// Install with: npm install node-fetch@2
const fetch = require("node-fetch");

// Model for the API request
class APIRequest {
  constructor(encoded_image) {
    // Base64-encoded image string
    this.encoded_image = encoded_image;
  }
}

// Model for the face rectangle returned by the API
class FaceRectangle {
  constructor(x, y, width, height) {
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;
  }
}

// Model for each detected face
class FaceDetect {
  constructor(faceRectangle, quality) {
    this.faceRectangle = faceRectangle;
    this.quality = quality;
  }
}

// Model for the API response
class FaceDetectResponse {
  constructor(faces) {
    this.faces = faces;
  }

  static fromJson(data) {
    const faces = [];
    for (const face of data.faces || []) {
      const rect = face.faceRectangle || {};
      const faceRectangle = new FaceRectangle(
        rect.x || 0,
        rect.y || 0,
        rect.width || 0,
        rect.height || 0
      );
      const quality = face.quality || 0.0;
      faces.push(new FaceDetect(faceRectangle, quality));
    }
    return new FaceDetectResponse(faces);
  }
}

// Main class to call the Face Detection API
class MXFaceAPI {
  constructor(apiUrl, subscriptionKey) {
    this.apiUrl = apiUrl;
    this.subscriptionKey = subscriptionKey;
  }

  // Detect faces in an image using the Face Detection API
  async detect(imagePath) {
    // Read the image file and encode it as Base64
    const imageBuffer = fs.readFileSync(imagePath);
    console.log(`Read ${imageBuffer.length} bytes from image file.`);
    const base64Image = imageBuffer.toString("base64");

    // Prepare the API request object
    const request = new APIRequest(base64Image);
    const jsonRequest = JSON.stringify(request);

    // Prepare headers
    const headers = {
      "Content-Type": "application/json",
      Accept: "application/json",
      subscriptionkey: this.subscriptionKey,
    };

    // Call the Face Detection API endpoint
    try {
      const response = await fetch(`${this.apiUrl}/api/v3/face/detect`, {
        method: "POST",
        headers: headers,
        body: jsonRequest,
      });

      const responseText = await response.text();
      if (response.status === 200) {
        console.log("Raw API response:", responseText);
        const apiResponse = JSON.parse(responseText);
        const detectFaces = FaceDetectResponse.fromJson(apiResponse);

        // Print results for each detected face
        if (detectFaces.faces.length > 0) {
          console.log(`Detected ${detectFaces.faces.length} face(s):`);
          for (const face of detectFaces.faces) {
            const rect = face.faceRectangle;
            console.log(
              `Face Quality: ${face.quality}, Rectangle: x=${rect.x}, y=${rect.y}, width=${rect.width}, height=${rect.height}`
            );
          }
        } else {
          console.log("No faces detected in the image.");
        }
      } else {
        // Print error details
        console.log(`Error ${response.status}: ${responseText}`);
      }
    } catch (err) {
      console.error("Request failed:", err);
    }
  }
}

// Entry point for the application
(async () => {
  // 1. Set your API subscription key here
  const subscriptionKey = "PdVLlEYd7Ig51n3oTJ-00RIdBGO6I897"; // Replace with your MXFace.ai subscription key

  // 2. Set the path to your image file here
  const imagePath = "rohit_sharma.jpg"; // Replace with your image file path

  // 3. Create the API client
  const mxfaceAPI = new MXFaceAPI("https://faceapi.mxface.ai", subscriptionKey);

  console.log("Calling Face Detect API...");
  await mxfaceAPI.detect(imagePath);

  console.log("Done.");
})();

// ----------------------
// Integration Instructions:
// 1. Install dependencies:
//    npm install node-fetch@2
// 2. Copy this file into your project.
// 3. Replace 'YOUR_SUBSCRIPTION_KEY' with your actual MXFace.ai subscription key.
// 4. Replace 'rohit_sharma.jpg' with the path to your image file.
// 5. Run the program with: node main.js
//    It will print detected face rectangles and quality scores.
//
