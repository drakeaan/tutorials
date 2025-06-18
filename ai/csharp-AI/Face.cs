using Azure;
using Azure.AI.Vision.Face;
using Azure.Core.Diagnostics;
using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_AI
{
    internal class Face
    {
        public async Task Run()
        {
            // Setup a listener to monitor logged events.
            using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();

            // Create the client with AzureKeyCredential
            Uri endpoint = new Uri("<your endpoint>");
            DefaultAzureCredential credential = new DefaultAzureCredential();
            var client = new FaceClient(endpoint, credential);



            // Select a service API version
            Uri endpoint = new Uri("<your endpoint>");
            DefaultAzureCredential credential = new DefaultAzureCredential();
            AzureAIVisionFaceClientOptions options = new AzureAIVisionFaceClientOptions(AzureAIVisionFaceClientOptions.ServiceVersion.V1_2_Preview_1);
            FaceClient client = new FaceClient(endpoint, credential, options);


            // Face Detection
            using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);

            var detectResponse = client.Detect(
                BinaryData.FromStream(stream),
                FaceDetectionModel.Detection03,
                FaceRecognitionModel.Recognition04,
                returnFaceId: false,
                returnFaceAttributes: new[] { FaceAttributeType.Detection03.HeadPose, FaceAttributeType.Detection03.Mask, FaceAttributeType.Recognition04.QualityForRecognition },
                returnFaceLandmarks: true,
                returnRecognitionModel: true,
                faceIdTimeToLive: 120);

            var detectedFaces = detectResponse.Value;
            Console.WriteLine($"Detected {detectedFaces.Count} face(s) in the image.");
            foreach (var detectedFace in detectedFaces)
            {
                Console.WriteLine($"Face Rectangle: left={detectedFace.FaceRectangle.Left}, top={detectedFace.FaceRectangle.Top}, width={detectedFace.FaceRectangle.Width}, height={detectedFace.FaceRectangle.Height}");
                Console.WriteLine($"Head pose: pitch={detectedFace.FaceAttributes.HeadPose.Pitch}, roll={detectedFace.FaceAttributes.HeadPose.Roll}, yaw={detectedFace.FaceAttributes.HeadPose.Yaw}");
                Console.WriteLine($"Mask: NoseAndMouthCovered={detectedFace.FaceAttributes.Mask.NoseAndMouthCovered}, Type={detectedFace.FaceAttributes.Mask.Type}");
                Console.WriteLine($"Quality: {detectedFace.FaceAttributes.QualityForRecognition}");
                Console.WriteLine($"Recognition model: {detectedFace.RecognitionModel}");
                Console.WriteLine($"Landmarks: ");

                Console.WriteLine($"    PupilLeft: ({detectedFace.FaceLandmarks.PupilLeft.X}, {detectedFace.FaceLandmarks.PupilLeft.Y})");
                Console.WriteLine($"    PupilRight: ({detectedFace.FaceLandmarks.PupilRight.X}, {detectedFace.FaceLandmarks.PupilRight.Y})");
                Console.WriteLine($"    NoseTip: ({detectedFace.FaceLandmarks.NoseTip.X}, {detectedFace.FaceLandmarks.NoseTip.Y})");
                Console.WriteLine($"    MouthLeft: ({detectedFace.FaceLandmarks.MouthLeft.X}, {detectedFace.FaceLandmarks.MouthLeft.Y})");
                Console.WriteLine($"    MouthRight: ({detectedFace.FaceLandmarks.MouthRight.X}, {detectedFace.FaceLandmarks.MouthRight.Y})");
                Console.WriteLine($"    EyebrowLeftOuter: ({detectedFace.FaceLandmarks.EyebrowLeftOuter.X}, {detectedFace.FaceLandmarks.EyebrowLeftOuter.Y})");
                Console.WriteLine($"    EyebrowLeftInner: ({detectedFace.FaceLandmarks.EyebrowLeftInner.X}, {detectedFace.FaceLandmarks.EyebrowLeftInner.Y})");
                Console.WriteLine($"    EyeLeftOuter: ({detectedFace.FaceLandmarks.EyeLeftOuter.X}, {detectedFace.FaceLandmarks.EyeLeftOuter.Y})");
                Console.WriteLine($"    EyeLeftTop: ({detectedFace.FaceLandmarks.EyeLeftTop.X}, {detectedFace.FaceLandmarks.EyeLeftTop.Y})");
                Console.WriteLine($"    EyeLeftBottom: ({detectedFace.FaceLandmarks.EyeLeftBottom.X}, {detectedFace.FaceLandmarks.EyeLeftBottom.Y})");
                Console.WriteLine($"    EyeLeftInner: ({detectedFace.FaceLandmarks.EyeLeftInner.X}, {detectedFace.FaceLandmarks.EyeLeftInner.Y})");
                Console.WriteLine($"    EyebrowRightInner: ({detectedFace.FaceLandmarks.EyebrowRightInner.X}, {detectedFace.FaceLandmarks.EyebrowRightInner.Y})");
                Console.WriteLine($"    EyebrowRightOuter: ({detectedFace.FaceLandmarks.EyebrowRightOuter.X}, {detectedFace.FaceLandmarks.EyebrowRightOuter.Y})");
                Console.WriteLine($"    EyeRightInner: ({detectedFace.FaceLandmarks.EyeRightInner.X}, {detectedFace.FaceLandmarks.EyeRightInner.Y})");
                Console.WriteLine($"    EyeRightTop: ({detectedFace.FaceLandmarks.EyeRightTop.X}, {detectedFace.FaceLandmarks.EyeRightTop.Y})");
                Console.WriteLine($"    EyeRightBottom: ({detectedFace.FaceLandmarks.EyeRightBottom.X}, {detectedFace.FaceLandmarks.EyeRightBottom.Y})");
                Console.WriteLine($"    EyeRightOuter: ({detectedFace.FaceLandmarks.EyeRightOuter.X}, {detectedFace.FaceLandmarks.EyeRightOuter.Y})");
                Console.WriteLine($"    NoseRootLeft: ({detectedFace.FaceLandmarks.NoseRootLeft.X}, {detectedFace.FaceLandmarks.NoseRootLeft.Y})");
                Console.WriteLine($"    NoseRootRight: ({detectedFace.FaceLandmarks.NoseRootRight.X}, {detectedFace.FaceLandmarks.NoseRootRight.Y})");
                Console.WriteLine($"    NoseLeftAlarTop: ({detectedFace.FaceLandmarks.NoseLeftAlarTop.X}, {detectedFace.FaceLandmarks.NoseLeftAlarTop.Y})");
                Console.WriteLine($"    NoseRightAlarTop: ({detectedFace.FaceLandmarks.NoseRightAlarTop.X}, {detectedFace.FaceLandmarks.NoseRightAlarTop.Y})");
                Console.WriteLine($"    NoseLeftAlarOutTip: ({detectedFace.FaceLandmarks.NoseLeftAlarOutTip.X}, {detectedFace.FaceLandmarks.NoseLeftAlarOutTip.Y})");
                Console.WriteLine($"    NoseRightAlarOutTip: ({detectedFace.FaceLandmarks.NoseRightAlarOutTip.X}, {detectedFace.FaceLandmarks.NoseRightAlarOutTip.Y})");
                Console.WriteLine($"    UpperLipTop: ({detectedFace.FaceLandmarks.UpperLipTop.X}, {detectedFace.FaceLandmarks.UpperLipTop.Y})");
                Console.WriteLine($"    UpperLipBottom: ({detectedFace.FaceLandmarks.UpperLipBottom.X}, {detectedFace.FaceLandmarks.UpperLipBottom.Y})");
                Console.WriteLine($"    UnderLipTop: ({detectedFace.FaceLandmarks.UnderLipTop.X}, {detectedFace.FaceLandmarks.UnderLipTop.Y})");
                Console.WriteLine($"    UnderLipBottom: ({detectedFace.FaceLandmarks.UnderLipBottom.X}, {detectedFace.FaceLandmarks.UnderLipBottom.Y})");
            }





            // Liveness detection
            var createContent = new CreateLivenessSessionContent(LivenessOperationMode.Passive)
            {
                SendResultsToClient = true,
                DeviceCorrelationId = Guid.NewGuid().ToString(),
            };

            var createResponse = sessionClient.CreateLivenessSession(createContent);

            var sessionId = createResponse.Value.SessionId;
            Console.WriteLine($"Session created, SessionId: {sessionId}");
            Console.WriteLine($"AuthToken: {createResponse.Value.AuthToken}");


            //
            var getResultResponse = sessionClient.GetLivenessSessionResult(sessionId);
            var sessionResult = getResultResponse.Value;
            Console.WriteLine($"Id: {sessionResult.Id}");
            Console.WriteLine($"CreatedDateTime: {sessionResult.CreatedDateTime}");
            Console.WriteLine($"SessionExpired: {sessionResult.SessionExpired}");
            Console.WriteLine($"DeviceCorrelationId: {sessionResult.DeviceCorrelationId}");
            Console.WriteLine($"AuthTokenTimeToLiveInSeconds: {sessionResult.AuthTokenTimeToLiveInSeconds}");
            Console.WriteLine($"Status: {sessionResult.Status}");
            Console.WriteLine($"SessionStartDateTime: {sessionResult.SessionStartDateTime}");
            if (sessionResult.Result != null)
            {
                WriteLivenessSessionAuditEntry(sessionResult.Result);
            }





            // Troubleshooting
            var invalidUri = new Uri("http://invalid.uri");
            try
            {
                var detectResponse = client.Detect(
                    invalidUri,
                    FaceDetectionModel.Detection01,
                    FaceRecognitionModel.Recognition04,
                    returnFaceId: false);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }
    }
}
