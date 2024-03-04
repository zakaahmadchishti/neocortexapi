﻿//Global Variable
using KNNImplementation;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using static NeoCortexApiSample.MultiSequenceLearning;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NeoCortexApiSample
{
    class Program
    {

        // Main method to start the program
        static void Main(string[] args)
        {
            // Loading training data
            double[][] sdrData = ReadSDRDataFromFile("/Users/zakaahmedchishti/Documents/GitHub/se-cloud-2023-2024/MyWork_Exerices/neocortexapi/My Project:KNN /New/New/sdr_data.txt");
            foreach (var dataset in sdrData)
            {
                string.Join(", ", dataset);
            }

            // Getting test datasets
            double[][] testDatasets = GetTestDatasets("/Users/zakaahmedchishti/Documents/GitHub/se-cloud-2023-2024/MyWork_Exerices/neocortexapi/My Project:KNN /New/New/test_data.txt");
            foreach (var dataset in testDatasets)
            {
                string.Join(", ", dataset);
            }

            // Number of classes in the dataset
            int numofclass = 3;

            // Looping through different values of K
            for (int k = 1; k <= 3; k++)
            {
                Console.WriteLine($"Value of K is equal to {k}");
                KNNClassifier kNN = new KNNClassifier();

                // Looping through each test dataset
                foreach (var testData in testDatasets)
                {
                    // Classifying the test data using KNN algorithm
                    int sequence = kNN.Classifier(testData, sdrData, numofclass, k);

                    // Displaying the predicted class for the test data
                    Console.WriteLine($"Predicted class for test data: {(sequence == 0 ? "Even" : (sequence == 1 ? "Odd" : "Neither Odd nor Even"))}");
                }

                Console.WriteLine();
            }
        }

        // Method to get test datasets
        static double[][] GetTestDatasets(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            double[][] datasets = new double[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                datasets[i] = new double[values.Length];

                for (int j = 0; j < values.Length; j++)
                {
                    if (!double.TryParse(values[j], out datasets[i][j]))
                    {
                        // Handle parsing error
                        throw new FormatException($"Failed to parse value at line {i + 1}, position {j + 1}");
                    }
                }
            }

            return datasets;
        }


        // Method to get SDR datasets
        static double[][] ReadSDRDataFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            double[][] sdrData = new double[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                sdrData[i] = new double[values.Length];

                for (int j = 0; j < values.Length; j++)
                {
                    if (!double.TryParse(values[j], out sdrData[i][j]))
                    {
                        // Handle parsing error
                        throw new FormatException($"Failed to parse value at line {i + 1}, position {j + 1}");
                    }
                }
            }

            return sdrData;
        }


        /// Runs a multi-sequence learning experiment using simple sequences.
        private static void RunMultiSimpleSequenceLearningExperiment()
        {
            // Initialize a dictionary to store sequences, where each sequence is represented by a list of doubles.
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            // Define the first sequence (S1) with prime numbers: 2, 3, 7.
            sequences.Add("S1", new List<double>(new double[] { 2, 3, 7 }));

            // Define the second sequence (S2) with non-prime numbers: 10, 15, 21.
            sequences.Add("S2", new List<double>(new double[] { 10, 15, 21 }));

            // Initialize the multi-sequence learning experiment.
            MultiSequenceLearning experiment = new MultiSequenceLearning();

            // Run the experiment to build the prediction engine.
            var predictor = experiment.Run(sequences);
        }


        /// <summary>
        /// Runs a multi-sequence learning experiment using various types of sequences.
        /// </summary>
        private static void RunMultiSequenceLearningExperiment()
        {
            // Initialize a dictionary to store sequences, where each sequence is represented by a list of doubles.
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            // Define the first sequence (S1) with even numbers: 2, 4, 6, 8, 10, 12, 14.
            sequences.Add("S1", new List<double>(new double[] { 2, 4, 6, 8, 10, 12, 14 }));

            // Define the second sequence (S2) with even numbers: 2, 6, 12, 14.
            sequences.Add("S2", new List<double>(new double[] { 2, 6, 12, 14 }));

            // Define the third sequence (S3) with odd numbers starting from 3: 3, 5, 7, 9, 11, 13, 15.
            sequences.Add("S3", new List<double>(new double[] { 3, 5, 7, 9, 11, 13, 15 }));

            // Define the fourth sequence (S4) with odd numbers: 3, 9, 13, 15.
            sequences.Add("S4", new List<double>(new double[] { 3, 9, 13, 15 }));

            // Define the fifth sequence (S5) with numbers that are neither odd nor even: 4.5, 11.4, 12.8, 15.5, 16.6, 17.7.
            sequences.Add("S5", new List<double>(new double[] { 4.5, 11.4, 12.8, 15.5, 16.6, 17.7 }));

            // Define the sixth sequence (S6) with numbers that are neither odd nor even: 4.5, 11.4, 12.8, 16.6.
            sequences.Add("S6", new List<double>(new double[] { 4.5, 11.4, 12.8, 16.6 }));

            // Initialize the multi-sequence learning experiment.
            MultiSequenceLearning experiment = new MultiSequenceLearning();

            // Run the experiment to build the prediction engine.
            var predictor = experiment.Run(sequences);
        }


        private static void PredictNextElement(Predictor predictor, double[] list)
        {
            // Output a separator for better readability in debug output.
            Debug.WriteLine("------------------------------");

            // Iterate through each element in the provided list.
            foreach (var item in list)
            {
                // Predict the next element based on the current item in the sequence.
                var res = predictor.Predict(item);

                // Check if predictions are available.
                if (res.Count > 0)
                {
                    // Output each prediction along with its similarity score.
                    foreach (var pred in res)
                    {
                        Debug.WriteLine($"{pred.PredictedInput} - {pred.Similarity}");
                    }

                    // Extract the predicted sequence and the next predicted element.
                    var tokens = res.First().PredictedInput.Split('_');
                    var tokens2 = res.First().PredictedInput.Split('-');

                    // Output the predicted sequence and the next predicted element.
                    Debug.WriteLine($"Predicted Sequence: {tokens[0]}, predicted next element {tokens2.Last()}");
                }
                else
                {
                    // Output a message if no predictions are available.
                    Debug.WriteLine("Nothing predicted :(");
                }
            }

            // Output a separator for better readability in debug output.
            Debug.WriteLine("------------------------------");
        }
    }
}