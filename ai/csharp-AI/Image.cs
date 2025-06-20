using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAI;

internal class Image
{
    internal async Task Run()
    {
        string endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");
        string key = Environment.GetEnvironmentVariable("VISION_KEY");

        // Create an Image Analysis client.
        ImageAnalysisClient client = new ImageAnalysisClient(new Uri(endpoint), new AzureKeyCredential(key));


        // Generate an image caption for an image file
        // Use a file stream to pass the image data to the analyze call
        using FileStream stream = new FileStream("image-analysis-sample.jpg", FileMode.Open);

        // Get a caption for the image.
        ImageAnalysisResult result = client.Analyze(
            BinaryData.FromStream(stream),
            VisualFeatures.Caption,
            new ImageAnalysisOptions { GenderNeutralCaption = true });

        // Print caption results to the console
        Console.WriteLine($"Image analysis results:");
        Console.WriteLine($" Caption:");
        Console.WriteLine($"   '{result.Caption.Text}', Confidence {result.Caption.Confidence:F4}");



        // Generate an image caption for an image URL
        // Get a caption for the image.
        ImageAnalysisResult result = client.Analyze(
            new Uri("https://aka.ms/azsdk/image-analysis/sample.jpg"),
            VisualFeatures.Caption,
            new ImageAnalysisOptions { GenderNeutralCaption = true });

        // Print caption results to the console
        Console.WriteLine($"Image analysis results:");
        Console.WriteLine($" Caption:");
        Console.WriteLine($"   '{result.Caption.Text}', Confidence {result.Caption.Confidence:F4}");




        // Extract text from an image file
        // Load image to analyze into a stream
        using FileStream stream = new FileStream("image-analysis-sample.jpg", FileMode.Open);

        // Extract text (OCR) from an image stream.
        ImageAnalysisResult result = client.Analyze(
            BinaryData.FromStream(stream),
            VisualFeatures.Read);

        // Print text (OCR) analysis results to the console
        Console.WriteLine("Image analysis results:");
        Console.WriteLine(" Read:");

        foreach (DetectedTextBlock block in result.Read.Blocks)
            foreach (DetectedTextLine line in block.Lines)
            {
                Console.WriteLine($"   Line: '{line.Text}', Bounding Polygon: [{string.Join(" ", line.BoundingPolygon)}]");
                foreach (DetectedTextWord word in line.Words)
                {
                    Console.WriteLine($"     Word: '{word.Text}', Confidence {word.Confidence.ToString("#.####")}, Bounding Polygon: [{string.Join(" ", word.BoundingPolygon)}]");
                }
            }


        // Extract text from an image URL
        // Extract text (OCR) from an image stream.
        ImageAnalysisResult result = client.Analyze(
            new Uri("https://aka.ms/azsdk/image-analysis/sample.jpg"),
            VisualFeatures.Read);

        // Print text (OCR) analysis results to the console
        Console.WriteLine("Image analysis results:");
        Console.WriteLine(" Read:");

        foreach (DetectedTextBlock block in result.Read.Blocks)
            foreach (DetectedTextLine line in block.Lines)
            {
                Console.WriteLine($"   Line: '{line.Text}', Bounding Polygon: [{string.Join(" ", line.BoundingPolygon)}]");
                foreach (DetectedTextWord word in line.Words)
                {
                    Console.WriteLine($"     Word: '{word.Text}', Confidence {word.Confidence.ToString("#.####")}, Bounding Polygon: [{string.Join(" ", word.BoundingPolygon)}]");
                }
            }




        // Troubleshooting
        var imageUrl = new Uri("https://aka.ms.invalid/azai/vision/image-analysis-sample.jpg");

        try
        {
            var result = client.Analyze(imageUrl, VisualFeatures.Caption);
        }
        catch (RequestFailedException e)
        {
            if (e.Status == 400)
            {
                Console.WriteLine("Error analyzing image.");
                Console.WriteLine($"HTTP status code {e.Status}: {e.Message}");
            }
            else
            {
                throw;
            }
        }
    }
}