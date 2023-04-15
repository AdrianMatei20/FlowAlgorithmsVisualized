import { AfterViewInit, Component, OnInit } from '@angular/core';
import * as pseudocode from 'pseudocode';

@Component({
  selector: 'app-ahuja-orlin-capacity-scaling',
  templateUrl: './ahuja-orlin-capacity-scaling.component.html',
  styleUrls: ['./ahuja-orlin-capacity-scaling.component.css']
})
export class AhujaOrlinCapacityScalingComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    if (document.getElementById("pseudocode")) {
      pseudocode.renderElement(document.getElementById("pseudocode"),
        {
          lineNumber: true,
          noEnd: false,
          captionCount: 3,
        }
      );
    }
  }

}
