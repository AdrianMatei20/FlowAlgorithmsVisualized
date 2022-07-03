import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { NetworkService } from '../services/network.service';
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
    router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.algorithm = this.route.snapshot.paramMap.get('algorithm');
        console.log("Algorithm: " + this.algorithm);
        this.reset();
        this.getData(this.algorithm);
      }
    });
  }

  ngOnInit(): void {
  }

  getData(algorithm: string): void {
    this.networkService.getData(algorithm).subscribe((response) => {
      let data = response as string[][];
      if (data.length > 0) {
        this.capacityNetwork = data[0][0] as string;
        this.residualNetworks = data[1] as string[];
        this.flowNetworks = data[2] as string[];

        this.renderNetwork(this.capacityNetwork, "#capacity-network");
        this.renderNetwork(this.flowNetworks[0], "#flow-network");
        this.renderNetwork(this.residualNetworks[0], "#residual-network");
      }
      else {
        this.reset();
      }
    });
  }

  renderNetwork(network: string, selector: string): void {
    var graphviz = null;
    graphviz = d3.select(selector).graphviz();
    graphviz.engine(this.layoutEngine);
    graphviz!.renderDot(network)
  }

  startAnimation(network: string[], netwokDiv: string): void {
    var algorithmSteps = network;
    var dotIndex = 1;
    var graphviz = null;
    var bigDelay = 2000;
    var smallDelay = 100;

    var render = function () {
      var dot = algorithmSteps[dotIndex];
      graphviz!
        .renderDot(dot)
        .on("end", function () {
          if (dotIndex + 1 < algorithmSteps.length) {
            dotIndex = dotIndex + 1;
            render();
          }
        });
    }

    graphviz = d3.select(netwokDiv).graphviz()
      .transition(function () {
        return d3.transition("main")
          .ease(d3.easeLinear)
          .delay(smallDelay)
          .duration(250);
      })
      .logEvents(false)
      .on("initEnd", render);
    graphviz.engine(this.layoutEngine);
  }

  startAnimationOnClick() {
    this.startAnimation(this.residualNetworks, "#residual-network");
    this.startAnimation(this.flowNetworks, "#flow-network");
  }

  reset() {
    this.startAnimation([], "#residual-network");
    this.startAnimation([], "#flow-network");

    this.renderNetwork(null, "#capacity-network");
    this.renderNetwork(null, "#flow-network");
    this.renderNetwork(null, "#residual-network");
  }

}
