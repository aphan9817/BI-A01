/*
* FILE : Dataset.cs
* PROJECT : SENG3120 - Assignment #1
* PROGRAMMER : Anthony Phan
* FIRST VERSION : January 20, 2025
* DESCRIPTION :
* Class for Datasets
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENG3120_A01
{
    public class Dataset
    {
        public string Name { get; }
        public string[] Categories { get; }
        public int[] Frequencies { get; }

        public Dataset(string name, string[] categories, int[] frequencies)
        {
            Name = name;
            Categories = categories;
            Frequencies = frequencies;
        }
    }
}
