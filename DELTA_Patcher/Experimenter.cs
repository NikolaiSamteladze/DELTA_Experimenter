﻿using System.Collections.Generic;
using DELTA_Patcher.Data_Model;
using DELTA_Patcher.Utils;
using System.IO;
using System;

namespace DELTA_Patcher
{
    public static class Experimenter
    {
        #region Constants

        private const string NAME_STAT_FILE = "stats.csv";

        #endregion

        // This method will be called from the console
        /* LOGIC
         * 1. Load experiment's options
         * 2. Get patching algorithm
         * 3. Run algorithm on the specified applications
         */
        public static void StartExperiment(string pathExperimentOptions)
        {
            var experimentOptions = LoadExperimentOptions(pathExperimentOptions);

            if (experimentOptions == null) return;

            var patchingAlgorithm = PatchingAlgorithmFactory.ConstructAlgorithm(experimentOptions.PatchingAlgorithm);

            foreach (var appInExperiment in experimentOptions.AppsInExperiment)
            {
                Console.Write(String.Format("Processing {0} ... ", appInExperiment.PackageName));

                var listAppPackages = StorageManager.GetLatestAppPackages(appInExperiment.PackageName,
                                                                            appInExperiment.NumOfLatestVersionsToUse);

                for (int i = 0; i < listAppPackages.Count - 1; ++i)
                {
                    var reference = listAppPackages[i];
                    var target = listAppPackages[i + 1];
                    var pathCurPatch = Path.Combine(experimentOptions.OutputDir,
                                                    NamingHelper.GetPatchName(reference, target));
                    patchingAlgorithm.ComputePatch(reference, target, pathCurPatch);
                }
                Console.WriteLine("--- DONE");
            }
        }

        /* ExperimentOptions file structure:
         * 
         * Patching algorithm
         * Output directory
         * Statistics directory
         * List of applications as { package name; number of latest versions to use }
         */
        private static ExperimentOptions LoadExperimentOptions(string pathExperimentOptions)
        {
            // Read experiments options as raw data and conver it later
            List<List<string>> rawExperimentOptions = FileManager.ReadFromCSV(pathExperimentOptions);
            
            // Parse and get experiments options
            ExperimentOptions experimentOptions = new ExperimentOptions();
            if (!experimentOptions.ConstructObject(rawExperimentOptions)) return null;

            return experimentOptions;
        }

        
    }
}
