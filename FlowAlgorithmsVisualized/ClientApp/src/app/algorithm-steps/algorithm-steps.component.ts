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
  private flowNetwork: string = "";
  private residualNetworks: string[] = [];
  private flowNetworks: string[] = [];

  constructor(private networkService: NetworkService, private router: Router, private route: ActivatedRoute) {
    router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.algorithm = this.route.snapshot.paramMap.get('algorithm');
        console.log("Algorithm: " + this.algorithm);
        d3.select("svg").remove();
        this.reset();
        this.getData(this.algorithm);
      }
    });
  }

  ngOnInit(): void {
  }

  getData(algorithm: string): void {
    this.networkService.getCapacityNetwork(algorithm).subscribe((network) => {
      this.capacityNetwork = network;
      // console.log("getData() --> CapacityNetwork:\n" + this.capacityNetwork);
      this.renderNetwork(this.capacityNetwork, "#capacity-network");
      // this.renderNetwork(this.capacityNetwork, "#residual-network");
    });

    //this.networkService.getFlowNetwork(algorithm).subscribe((network) => {
    //  this.flowNetwork = network;
    //  // console.log("getData() --> FlowNetwork:\n" + this.flowNetwork);
    //  this.renderNetwork(this.flowNetwork, "#flow-network");
    //});

    this.networkService.getAlgorithmSteps(algorithm).subscribe((steps) => {
      this.residualNetworks = steps[0] as string[];
      this.flowNetworks = steps[1] as string[];

      // console.log(this.residualNetworks.length);
      // this.residualNetworks.forEach((step) => console.log(step));
      // this.flowNetworks.forEach((step) => console.log(step));

      this.renderNetwork(this.flowNetworks[0], "#flow-network");
      this.renderNetwork(this.residualNetworks[0], "#residual-network");

      //this.startAnimation(this.residualNetworks, "#residual-network");
      //this.startAnimation(this.flowNetworks, "#flow-network");
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
    var dotIndex = 0;

    var graphviz = null;
    var t1 = d3.transition("main")
      .ease(d3.easeLinear)
      .delay(500)
      .duration(500);

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
      .transition(t1)
      .logEvents(false)
      .on("initEnd", render);
    graphviz.engine(this.layoutEngine);
  }

  startAnimationOnClick() {
    this.startAnimation(this.residualNetworks, "#residual-network");
    this.startAnimation(this.flowNetworks, "#flow-network");
  }

  reset() {
    this.startAnimation([""], "#residual-network");
    this.startAnimation([""], "#flow-network");
    this.renderNetwork("", "#flow-network");
    this.renderNetwork("", "#residual-network");
  }

}
