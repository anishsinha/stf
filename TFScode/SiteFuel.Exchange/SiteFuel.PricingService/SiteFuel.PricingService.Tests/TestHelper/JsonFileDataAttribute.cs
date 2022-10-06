using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace SiteFuel.PricingService.Tests.TestHelper
{
    public class JsonFileDataAttribute : DataAttribute
    {
        private readonly string _filePath;
        private readonly string _propertyName;

        /// <summary>
        /// Load data from a JSON file as the data source for a theory
        /// </summary>
        /// <param name="filePath">The absolute or relative path to the JSON file to load</param>
        public JsonFileDataAttribute(string filePath)
            : this(filePath, null) { }

        /// <summary>
        /// Load data from a JSON file as the data source for a theory
        /// </summary>
        /// <param name="filePath">The absolute or relative path to the JSON file to load</param>
        /// <param name="propertyName">The name of the property on the JSON file that contains the data for the test</param>
        public JsonFileDataAttribute(string filePath, string propertyName)
        {
            _filePath = filePath;
            _propertyName = propertyName;
        }

        /// <inheritDoc />
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) { throw new ArgumentNullException(nameof(testMethod)); }

            // TODO : you may will get error , because GetRelativePath is not in current framework 4.7. so commenting now , will look in future
            // Get the absolute path to the JSON file
            //var path = Path.IsPathRooted(_filePath)
            //    ? _filePath
            //    : Path.GetRelativePath(Directory.GetCurrentDirectory(), _filePath);

            //if (!File.Exists(path))
            //{
            //    throw new ArgumentException($"Could not find file at path: {path}");
            //}

            //// Load the file
            //var fileData = File.ReadAllText(_filePath);


            var rootDirectory = Directory.GetParent( Directory.GetParent( Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName;

          

            var fullfilename = Path.Combine(rootDirectory, _filePath);

            if (!File.Exists(fullfilename))
            {
                throw new ArgumentException($"Could not find file at path: {fullfilename}");
            }
           
            // Load the file
            var fileData = File.ReadAllText(fullfilename);

            if (string.IsNullOrEmpty(_propertyName))
            {
                // Whole file is the data
                var jsonData = JsonConvert.DeserializeObject<List<object[]>>(fileData);
                return CastParamTypes(jsonData, testMethod);
            }
            else
            {
                // Only use the specified property as the data
                var allData = JObject.Parse(fileData);
                var data = allData[_propertyName];
                
                
                   var jsonData = data.ToObject<List<object[]>>();
                    return CastParamTypes(jsonData, testMethod);
                
               
                
            }
        }

        public String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
        private IEnumerable<object[]> CastParamTypes(List<object[]> jsonData, MethodBase testMethod)
        {
            var result = new List<object[]>();

            // Get the parameters of current test method
            var parameters = testMethod.GetParameters();

            // Foreach tuple of parameters in the JSON data
            foreach (var paramsTuple in jsonData)
            {
                var paramValues = new object[parameters.Length];

                // Foreach parameter in the method
                for (int i = 0; i < parameters.Length; i++)
                {
                    // Cast the value in the JSON data to match parameter type
                    paramValues[i] = CastParamValue(paramsTuple[i], parameters[i].ParameterType);
                }

                result.Add(paramValues);
            }

            return result;
        }

        private object CastParamValue(object value, Type type)
        {
            // Cast objects
            if (value is JObject jObjectValue)
            {
                return jObjectValue.ToObject(type);
            }
            // Cast arrays
            else if (value is JArray jArrayValue)
            {
                return jArrayValue.ToObject(type);
            }
            // No cast for value types
            return value;
        }
    }
}
