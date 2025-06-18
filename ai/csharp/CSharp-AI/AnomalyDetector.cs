using Azure;
using Azure.AI.AnomalyDetector;
using Azure.Core;
using Azure.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_AI;

internal class AnomalyDetector
{
    

    public void Run()
    {
        // Setup a listener to monitor logged events.
        using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();


        string endpoint = "<endpoint>";
        string apiKey = "<apiKey>";
        var credential = new AzureKeyCredential(apiKey);
        var client = new AnomalyDetectorClient(new Uri(endpoint), credential);


        // Batch detection
        Console.WriteLine("Detecting anomalies in the entire time series.");

        try
        {
            UnivariateEntireDetectionResult result = client.DetectUnivariateEntireSeries(request);

            bool hasAnomaly = false;
            for (int i = 0; i < request.Series.Count; ++i)
            {
                if (result.IsAnomaly[i])
                {
                    Console.WriteLine($"An anomaly was detected at index: {i}.");
                    hasAnomaly = true;
                }
            }
            if (!hasAnomaly)
            {
                Console.WriteLine("No anomalies detected in the series.");
            }
        }
        catch (RequestFailedException ex)
        {
            Console.WriteLine($"Entire detection failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Detection error. {ex.Message}");
            throw;
        }



    }

    // Streaming Detection
    public void StreamingDetection()
    {
        // Setup a listener to monitor logged events.
        using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();

        //detect
        Console.WriteLine("Detecting the change point in the series.");

        UnivariateChangePointDetectionResult result = client.DetectUnivariateChangePoint(request);

        if (result.IsChangePoint.Contains(true))
        {
            Console.WriteLine("A change point was detected at index:");
            for (int i = 0; i < request.Series.Count; ++i)
            {
                if (result.IsChangePoint[i])
                {
                    Console.Write(i);
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("No change point detected in the series.");
        }
    }
}