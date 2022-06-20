import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NetworkService } from '../services/network.service';
// import * as d3 from 'd3';
declare var d3: any;

@Component({
  selector: 'app-algorithm-steps',
  templateUrl: './algorithm-steps.component.html',
  styleUrls: ['./algorithm-steps.component.css']
})
export class AlgorithmStepsComponent implements OnInit {

  private capacityNetwork: string = "";
  private flowNetwork: string = "";

  constructor(private networkService: NetworkService) {
  }

  getData(): void {
    this.networkService.getCapacityNetwork().subscribe((network) => {
      this.capacityNetwork = network;
      console.log("getData() --> CapacityNetwork:\n" + this.capacityNetwork);
      this.renderNetwork(this.capacityNetwork, "#capacity-network");
    });

    this.networkService.getFlowNetwork().subscribe((network) => {
      this.flowNetwork = network;
      console.log("getData() --> FlowNetwork:\n" + this.flowNetwork);
      this.renderNetwork(this.flowNetwork, "#flow-network");
    });
  }

  ngOnInit(): void {
    this.getData();
    this.startAnimation();
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

    var render = function () {
      var dotLines = dots[dotIndex];
      var dot = dotLines.join('');
      graphviz!
        .renderDot(dot)
        .on("end", function () {
          //dotIndex = (dotIndex + 1) % dots.length;
          if (dotIndex + 1 < dots.length) {
            dotIndex = dotIndex + 1;
            render();
          }
        });
    }

    graphviz = d3.select("#residual-network").graphviz()
      .transition(function () {
        return d3.transition("main")
          .ease(d3.easeLinear)
          .delay(100)
          .duration(500);
      })
      .logEvents(false)
      .on("initEnd", render);
  }

}

var dots = [
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="10"]
        1 -> 3 [label="12"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7"]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="15"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="10" penwidth=3]
        1 -> 3 [label="12"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7"]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="15"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="10" penwidth=3]
        1 -> 3 [label="12"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" penwidth=3]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="15"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="10" penwidth=3]
        1 -> 3 [label="12"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" penwidth=3]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="15" penwidth=3]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="10" penwidth=3 color="blue"]
        1 -> 3 [label="12"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" penwidth=3 color="blue"]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="15" penwidth=3 color="blue"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3" penwidth=3 color="red" fontcolor="red"]
        1 -> 3 [label="12"]
        2 -> 1 [label="7" penwidth=3 color="red" fontcolor="red"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" penwidth=3 color="blue"]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="15" penwidth=3 color="blue"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3" penwidth=3 color="red" fontcolor="red"]
        1 -> 3 [label="12"]
        2 -> 1 [label="7" penwidth=3 color="red" fontcolor="red"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" penwidth=3 color="red" fontcolor="red" dir=back]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="15" penwidth=3 color="blue"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3" penwidth=3 color="red" fontcolor="red"]
        1 -> 3 [label="12"]
        2 -> 1 [label="7" penwidth=3 color="red" fontcolor="red"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" penwidth=3 color="red" fontcolor="red" dir=back]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="8" penwidth=3 color="red" fontcolor="red"]
        6 -> 5 [label="7" penwidth=3 color="red" fontcolor="red"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3" penwidth=3]
        1 -> 3 [label="12"]
        2 -> 1 [label="7" penwidth=3]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" penwidth=3 dir=back]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="8" penwidth=3]
        6 -> 5 [label="7" penwidth=3]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3"]
        1 -> 3 [label="12"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="8"]
        6 -> 5 [label="7"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3"]
        1 -> 3 [label="12" penwidth=3]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 5 [label="5"]
        4 -> 6 [label="5"]
        5 -> 6 [label="8"]
        6 -> 5 [label="7"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3"]
        1 -> 3 [label="12" penwidth=3]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 5 [label="5" penwidth=3]
        4 -> 6 [label="5"]
        5 -> 6 [label="8"]
        6 -> 5 [label="7"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3"]
        1 -> 3 [label="12" penwidth=3]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 5 [label="5" penwidth=3]
        4 -> 6 [label="5"]
        5 -> 6 [label="8" penwidth=3]
        6 -> 5 [label="7"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3"]
        1 -> 3 [label="12" penwidth=3 color="blue"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 5 [label="5" penwidth=3 color="blue"]
        4 -> 6 [label="5"]
        5 -> 6 [label="8" penwidth=3 color="blue"]
        6 -> 5 [label="7"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3"]
        1 -> 3 [label="7" penwidth=3 color="red" fontcolor="red"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5" penwidth=3 color="red" fontcolor="red"]
        3 -> 5 [label="5" penwidth=3 color="blue"]
        4 -> 6 [label="5"]
        5 -> 6 [label="8" penwidth=3 color="blue"]
        6 -> 5 [label="7"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3"]
        1 -> 3 [label="7" penwidth=3 color="red" fontcolor="red"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5" penwidth=3 color="red" fontcolor="red"]
        3 -> 5 [label="5" penwidth=3 color="red" fontcolor="red" dir=back]
        4 -> 6 [label="5"]
        5 -> 6 [label="8" penwidth=3 color="blue"]
        6 -> 5 [label="7"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3"]
        1 -> 3 [label="7" penwidth=3 color="red" fontcolor="red"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5" penwidth=3 color="red" fontcolor="red"]
        3 -> 5 [label="5" penwidth=3 color="red" fontcolor="red" dir=back]
        4 -> 6 [label="5"]
        5 -> 6 [label="3" penwidth=3 color="red" fontcolor="red"]
        6 -> 5 [label="12" penwidth=3 color="red" fontcolor="red"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3"]
        1 -> 3 [label="7"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5"]
        3 -> 5 [label="5" dir=back]
        4 -> 6 [label="5"]
        5 -> 6 [label="3"]
        6 -> 5 [label="12"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3" penwidth=3]
        1 -> 3 [label="7"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5"]
        3 -> 5 [label="5" dir=back]
        4 -> 6 [label="5"]
        5 -> 6 [label="3"]
        6 -> 5 [label="12"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3" penwidth=3]
        1 -> 3 [label="7"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3" penwidth=3]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5"]
        3 -> 5 [label="5" dir=back]
        4 -> 6 [label="5"]
        5 -> 6 [label="3"]
        6 -> 5 [label="12"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3" penwidth=3]
        1 -> 3 [label="7"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3" penwidth=3]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5"]
        3 -> 5 [label="5" dir=back]
        4 -> 6 [label="5" penwidth=3]
        5 -> 6 [label="3"]
        6 -> 5 [label="12"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 2 [label="3" penwidth=3 color="blue"]
        1 -> 3 [label="7"]
        2 -> 1 [label="7"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3" penwidth=3 color="blue"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5"]
        3 -> 5 [label="5" dir=back]
        4 -> 6 [label="5" penwidth=3 color="blue"]
        5 -> 6 [label="3"]
        6 -> 5 [label="12"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 3 [label="7"]
        2 -> 1 [label="10" penwidth=3 color="red" fontcolor="red"]
        2 -> 3 [label="10"]
        2 -> 4 [label="3" penwidth=3 color="blue"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5"]
        3 -> 5 [label="5" dir=back]
        4 -> 6 [label="5" penwidth=3 color="blue"]
        5 -> 6 [label="3"]
        6 -> 5 [label="12"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 3 [label="7"]
        2 -> 1 [label="10" penwidth=3 color="red" fontcolor="red"]
        2 -> 3 [label="10"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5"]
        3 -> 5 [label="5" dir=back]
        4 -> 2 [label="3" penwidth=3 color="red" fontcolor="red"]
        4 -> 6 [label="5" penwidth=3 color="blue"]
        5 -> 6 [label="3"]
        6 -> 5 [label="12"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 3 [label="7"]
        2 -> 1 [label="10" penwidth=3 color="red" fontcolor="red"]
        2 -> 3 [label="10"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5"]
        3 -> 5 [label="5" dir=back]
        4 -> 2 [label="3" penwidth=3 color="red" fontcolor="red"]
        4 -> 6 [label="2" penwidth=3 color="red" fontcolor="red"]
        5 -> 6 [label="3"]
        6 -> 4 [label="3" penwidth=3 color="red" fontcolor="red"]
        6 -> 5 [label="12"]
      }`],
  [`digraph  {
        rankdir=LR
        ranksep = 2
        nodesep = 1
        node [shape="circle"]
        1 [style="filled", fillcolor="pink"]
        2
        3
        4
        5
        6 [style="filled", fillcolor="lightblue"]
        {rank = same; 2; 3;}
        {rank = same; 4; 5;}
        2, 4 [group=1]
        3, 5 [group=2]
        1 -> 3 [label="7"]
        2 -> 1 [label="10"]
        2 -> 3 [label="10"]
        2 -> 5 [label="7" dir=back]
        3 -> 1 [label="5"]
        3 -> 5 [label="5" dir=back]
        4 -> 2 [label="3"]
        4 -> 6 [label="2"]
        5 -> 6 [label="3"]
        6 -> 4 [label="3"]
        6 -> 5 [label="12"]
      }`]
];
