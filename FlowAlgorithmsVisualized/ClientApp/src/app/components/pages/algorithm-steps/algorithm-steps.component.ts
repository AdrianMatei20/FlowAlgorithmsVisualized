import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NetworkService } from '../../../services/network.service';
// import * as d3 from 'd3';
declare var d3: any;

@Component({
  selector: 'app-algorithm-steps',
  templateUrl: './algorithm-steps.component.html',
  styleUrls: ['./algorithm-steps.component.css']
})
export class AlgorithmStepsComponent implements OnInit {

  private layoutEngines = ["dot", "neato", "fdp", "sfdp", "circo", "twopi", "osage", "patchwork"];
  private layoutEngine = this.layoutEngines[1];

  private algorithm: string = "";
  private capacityNetwork: string = "";
  private residualNetworks: string[] = [];
  private flowNetworks: string[] = [];

  constructor(private networkService: NetworkService, private router: Router, private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      this.algorithm = params.get('algorithm');
      console.log('algorithm: ' + this.algorithm);
      this.reset();
      this.getData(this.algorithm);
    });
  }

  ngOnInit(): void {
    this.reset();
    this.getData(this.algorithm);
  }

  getData(algorithm: string): void {
    if (["GenericCuDMF", "FF", "EK", "AOSMC", "Gabow", "AODS", "AORS", "GenericCuPreflux", "PrefluxFIFO", "PrefluxCuECMM", "ScalareExces"].includes(algorithm)) {
      this.networkService.getData(algorithm).subscribe((response) => {
        let data = response as string[][];
        if (data.length > 0) {
          this.capacityNetwork = null;
          this.residualNetworks = null;
          this.flowNetworks = null;

          this.capacityNetwork = data[0][0] as string;
          this.flowNetworks = data[1] as string[];
          this.residualNetworks = data[2] as string[];

          this.renderNetwork(this.capacityNetwork, "#capacity-network");
          this.renderNetwork(this.flowNetworks[0], "#flow-network");
          this.renderNetwork(this.residualNetworks[0], "#residual-network");
        }
        else {
          this.reset();
        }
      });
    }
    else {
      this.renderNetwork("digraph {}", "#capacity-network");
      this.renderNetwork("digraph {}", "#flow-network");
      this.renderNetwork("digraph {}", "#residual-network");
    }
  }

  renderNetwork(network: string, selector: string): void {
    var graphviz = null;

    if (d3.select(selector) != null) {
      graphviz = d3.select(selector).graphviz();
      graphviz.engine(this.layoutEngine);
      graphviz!.renderDot(network);
      graphviz!.zoom(false);
    }
  }

  startAnimation(network: string[], netwokDiv: string): void {
    var algorithmSteps = network;
    var dotIndex = 1;
    var graphviz = null;

    var render = function () {
      var dot = algorithmSteps[dotIndex];
      graphviz!
        .renderDot(dot)
        .on("end", function () {
          if (dotIndex + 1 < algorithmSteps.length) {
            dotIndex = dotIndex + 1;
            render();
            if (this.animationPaused == true) {
              dotIndex = algorithmSteps.length;
            }
          }
        });
    }

    if (d3.select(netwokDiv) != null) {
      graphviz = d3.select(netwokDiv).graphviz()
        .transition(function () {
          return d3.transition("main")
            .ease(d3.easeLinear)
            .delay(1000)
            .duration(200);
        })
        .logEvents(false)
        .on("initEnd", render);

      graphviz.engine(this.layoutEngine);
    }
  }

  startAnimations() {
    this.startAnimation(this.residualNetworks, "#residual-network");
    this.startAnimation(this.flowNetworks, "#flow-network");
  }

  async reset() {
    d3.select("#capacity-network").innerHTML = "";
    d3.select("#flow-network").innerHTML = "";
    d3.select("#residual-network").innerHTML = "";
    this.renderNetwork("digraph {}", "#capacity-network");
    this.renderNetwork("digraph {}", "#flow-network");
    this.renderNetwork("digraph {}", "#residual-network");
    this.getData(this.algorithm);
    //this.startAnimation(this.residualNetworks.slice(0, 1), "#residual-network");
    //this.startAnimation(this.flowNetworks.slice(0, 1), "#flow-network");
  }

}
