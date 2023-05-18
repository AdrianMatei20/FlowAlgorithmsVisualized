using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using FlowAlgorithmsVisualizedBackend.Algorithms;
using FlowAlgorithmsVisualizedBackend.Network;

namespace FlowAlgorithmsVisualized.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlgorithmStepsController : ControllerBase
    {
        private string[] algorithms = { "GenericCuDMF", "FF", "EK", "AOSMC", "Gabow", "AODS", "AORS", "GenericCuPreflux", "PrefluxFIFO", "PrefluxCuECMM", "ScalareExces" };

        [HttpGet("steps")]
        public string GetData(string algorithmName)
        {
            List<List<string>> data = new List<List<string>>()
            {
                new List<string>(),
                new List<string>(),
                new List<string>()
            };

            if (algorithms.Contains(algorithmName))
            {
                IAlgorithmFactory algorithmFactory = new AlgorithmFactory();
                IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);
                data = algorithm.GetAlgorithmSteps();
            }

            return JsonSerializer.Serialize(data);
        }
    }
}
