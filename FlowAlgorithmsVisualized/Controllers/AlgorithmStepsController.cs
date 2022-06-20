using Microsoft.AspNetCore.Mvc;
using NetworkData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowAlgorithmsVisualized.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlgorithmStepsController : ControllerBase
    {
        [HttpGet("capacityNetwork")]
        public string GetCapacityNetwork(string algorithm)
        {
            string capacityNetwork = Network.GetCapacityNetwork(algorithm);
            return capacityNetwork;
        }

        [HttpGet("flowNetwork")]
        public string GetFlowNetwork(string algorithm)
        {
            string flowNetwork = Network.GetFlowNetwork(algorithm);
            return flowNetwork;
        }
    }
}
