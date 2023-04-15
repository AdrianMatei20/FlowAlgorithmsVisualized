import { AfterViewInit, Component, OnInit } from '@angular/core';
import * as pseudocode from 'pseudocode';

@Component({
  selector: 'app-ahuja-orlin-shortest-path',
  templateUrl: './ahuja-orlin-shortest-path.component.html',
  styleUrls: ['./ahuja-orlin-shortest-path.component.css']
})
export class AhujaOrlinShortestPathComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    if (document.getElementById("pseudocode")) {
      pseudocode.renderElement(document.getElementById("pseudocode"),
        {
          lineNumber: true,
          noEnd: false,
          captionCount: 5,
        }
      );
    }
  }

}
