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
        public string GetCapacityNetwork()
        {
            string capacityNetwork = Network.GetCapacityNetwork();
            return capacityNetwork;
        }

        [HttpGet("flowNetwork")]
        public string GetFlowNetwork()
        {
            string flowNetwork = Network.GetFlowNetwork();
            return flowNetwork;
        }
    }
}
