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

  private algorithm: string = "";
  private capacityNetwork: string = "";
  private flowNetwork: string = "";
  private algorithmSteps: string[] = [];

  constructor(private networkService: NetworkService, private router: Router, private route: ActivatedRoute) {
    router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.algorithm = this.route.snapshot.paramMap.get('algorithm');
        console.log("Algorithm: " + this.algorithm);
        this.getData(this.algorithm);
      }
    });
  }

  ngOnInit(): void {
    // this.startAnimation();
  }

  getData(algorithm: string): void {
    this.networkService.getCapacityNetwork(algorithm).subscribe((network) => {
      this.capacityNetwork = network;
      console.log("getData() --> CapacityNetwork:\n" + this.capacityNetwork);
      this.renderNetwork(this.capacityNetwork, "#capacity-network");
    });

    this.networkService.getFlowNetwork(algorithm).subscribe((network) => {
      this.flowNetwork = network;
      console.log("getData() --> FlowNetwork:\n" + this.flowNetwork);
      this.renderNetwork(this.flowNetwork, "#flow-network");
    });

    this.networkService.getAlgorithmSteps(algorithm).subscribe((steps) => {
      this.algorithmSteps = steps as string[];
      console.log(this.algorithmSteps.length);
      this.algorithmSteps.forEach((step) => console.log(step));
      this.startAnimation();
    });
  }

  renderNetwork(network: string, selector: string): void {
    var graphviz = null;
    graphviz = d3.select(selector).graphviz();
    graphviz.engine("fdp");
    graphviz!.renderDot(network)
  }

  startAnimation(): void {
    var dotIndex = 0;
    var graphviz = null;
    var algorithmSteps = this.algorithmSteps;

    var render = function () {
      var dot = algorithmSteps[dotIndex];
      graphviz!
        .renderDot(dot)
        .on("end", function () {
          //dotIndex = (dotIndex + 1) % dots.length;
          if (dotIndex + 1 < algorithmSteps.length) {
            dotIndex = dotIndex + 1;
            render();
          }
        });
    }

    graphviz = d3.select("#residual-network").graphviz()
      .transition(function () {
        return d3.transition("main")
          .ease(d3.easeLinear)
          .delay(500)
          .duration(1000);
      })
      .logEvents(false)
      .on("initEnd", render);

    graphviz.engine("fdp");
  }

}
