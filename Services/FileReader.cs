using Deferat.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Deferat.Services
{
    public class FileReader : IFileReader
    {
        public (string MetaData, string Text) ReadFile(string path)
        {
            string meta = string.Empty;
            string text = string.Empty;

            using(var reader = File.OpenText(path))
            {
                if(reader.ReadLine() != "---")
                    throw new FormatException("File was not in expected format");
                
                string line;

                while((line = reader.ReadLine()) != "---")
                {
                    meta += line + "\r\n";
                }

                text = reader.ReadToEnd();
            }

            return (meta, text);
        }
    } 
}