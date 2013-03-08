﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DELTA_Patcher.Data_Model;
using System.Diagnostics;

namespace DELTA_Patcher
{
    public class BSDIFFDeltaEncodingAlgortihm : IDeltaEncodingAlgorithm
    {
        public void ComputeDelta(ApplicationPackage reference, ApplicationPackage target, string pathOutputPatch)
        {
            // Invokes bsdiff.exe to compute patch
            var psi = new ProcessStartInfo
            {
                /* TODO:
                 * Bsdiff path is hardcoded. Use some function to return it instead
                 */
                FileName = "bsdiff.exe",
                // Parameters format: old_file new_file patch_file
                Arguments = String.Format("\"{0}\" \"{1}\" \"{2}\"", reference.PackagePath, target.PackagePath, pathOutputPatch),
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };

            // Start process and make sure that it is done before the execution proceeds
            var process = Process.Start(psi);
            process.WaitForExit();
        }
    }
}
