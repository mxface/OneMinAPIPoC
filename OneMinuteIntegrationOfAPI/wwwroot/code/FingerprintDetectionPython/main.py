# Face Detection API - One Minute Integration Example
# This file demonstrates how to call the MXFace.ai Face Detection API in a single Python file.
# Just copy-paste this code into your project, update your subscription key and image path, and run!

import base64
import json
import requests


# Model for the API request
class APIRequest:
    def __init__(self, encoded_image):
        # Base64-encoded image string
        self.encoded_image = encoded_image

    def to_json(self):
        return json.dumps({"encoded_image": self.encoded_image})


# Model for the face rectangle returned by the API
class FaceRectangle:
    def __init__(self, x, y, width, height):
        self.x = x
        self.y = y
        self.width = width
        self.height = height


# Model for each detected face
class FaceDetect:
    def __init__(self, face_rectangle, quality):
        self.face_rectangle = face_rectangle
        self.quality = quality


# Model for the API response
class FaceDetectResponse:
    def __init__(self, faces):
        self.faces = faces

    @staticmethod
    def from_json(data):
        faces = []
        for face in data.get("faces", []):
            rect = face.get("faceRectangle", {})
            face_rectangle = FaceRectangle(
                rect.get("x", 0),
                rect.get("y", 0),
                rect.get("width", 0),
                rect.get("height", 0),
            )
            quality = face.get("quality", 0.0)
            faces.append(FaceDetect(face_rectangle, quality))
        return FaceDetectResponse(faces)


# Main class to call the Face Detection API
class MXFaceAPI:
    def __init__(self, api_url, subscription_key):
        self.api_url = api_url
        self.subscription_key = subscription_key

    # Detect faces in an image using the Face Detection API
    def detect(self, image_path):
        # Read the image file and encode it as Base64
        with open(image_path, "rb") as image_file:
            image_bytes = image_file.read()
            print(f"Read {len(image_bytes)} bytes from image file.")
            base64_image = base64.b64encode(image_bytes).decode("utf-8")  # FIXED

        # Prepare the API request object
        request = APIRequest(base64_image)
        json_request = request.to_json()

        headers = {
            "Content-Type": "application/json",
            "Accept": "application/json",
            "subscriptionkey": self.subscription_key,
        }

        # Call the Face Detection API endpoint
        response = requests.post(
            f"{self.api_url}/api/v3/face/detect", data=json_request, headers=headers
        )

        # Read the API response
        if response.status_code == 200:
            print("Raw API response:", response.text)
            api_response = response.json()
            detect_faces = FaceDetectResponse.from_json(api_response)

            # Print results for each detected face
            if detect_faces.faces:
                print(f"Detected {len(detect_faces.faces)} face(s):")
                for face in detect_faces.faces:
                    rect = face.face_rectangle
                    print(
                        f"Face Quality: {face.quality}, Rectangle: x={rect.x}, y={rect.y}, width={rect.width}, height={rect.height}"
                    )
            else:
                print("No faces detected in the image.")
        else:
            # Print error details
            print(f"Error {response.status_code}: {response.text}")


# Entry point for the application
if __name__ == "__main__":
    # 1. Set your API subscription key here
    subscription_key = "PdVLlEYd7Ig51n3oTJ-00RIdBGO6I897"  # <-- Replace with your MXFace.ai subscription key

    # 2. Set the path to your image file here
    image_path = "rohit_sharma.jpg"  # <-- Replace with your image file path

    # 3. Create the API client
    mxface_api = MXFaceAPI("https://faceapi.mxface.ai", subscription_key)

    print("Calling Face Detect API...")
    mxface_api.detect(image_path)

    print("Done.")

# ----------------------
# Integration Instructions:
# 1. Install requests library:
#    pip install requests
# 2. Copy this file into your project.
# 3. Replace 'YOUR_SUBSCRIPTION_KEY' with your actual MXFace.ai subscription key.
# 4. Replace 'Leonardo.jpg' with the path to your image file.
# 5. Run the program. It will print detected face rectangles and quality
