using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.WSA.WebCam;

public class MPhotoCapture : MonoBehaviour
{
    string filePath;

    PhotoCapture m_PhotoCapture;

    void Start()
    {
        string filepath = System.IO.Path.Combine(Application.persistentDataPath, "TestPhoto.jpg");
        StartCapturePhoto(filepath);
    }

    public void StartCapturePhoto(string filePath)
    {
        Debug.Log("Start capture procedure's photo");

        this.filePath = filePath;

        PhotoCapture.CreateAsync(true, OnPhotoCaptureCreated);
    }

    void OnPhotoCaptureCreated(PhotoCapture photoCapture)
    {
        if (photoCapture != null)
        {
            m_PhotoCapture = photoCapture;

            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 1.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            m_PhotoCapture.StartPhotoModeAsync(cameraParameters, OnPhotoModeStarted);
        }
        else
        {
            Debug.LogError("Failed to create PhotoCapture Instance!");
        }
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            m_PhotoCapture.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }

    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        //if (result.success)
        //{
        //    Debug.Log("Saved Photo to disk!");
        //}
        //else
        //{
        //    Debug.Log("Failed to save Photo to disk");
        //}

        m_PhotoCapture.StopPhotoModeAsync(OnStoppedPhotoCaptureMode);
    }

    void OnStoppedPhotoCaptureMode(PhotoCapture.PhotoCaptureResult result)
    {
        m_PhotoCapture.Dispose();
        m_PhotoCapture = null;
        //Destroy(this.gameObject);
    }
}
