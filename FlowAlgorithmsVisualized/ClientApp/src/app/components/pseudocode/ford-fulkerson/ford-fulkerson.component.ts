import { AfterViewInit, Component, OnInit } from '@angular/core';
import * as pseudocode from 'pseudocode';

@Component({
  selector: 'app-ford-fulkerson',
  templateUrl: './ford-fulkerson.component.html',
  styleUrls: ['./ford-fulkerson.component.css']
})
export class FordFulkersonComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    if (document.getElementById("pseudocode")) {
      pseudocode.renderElement(document.getElementById("pseudocode"),
        {
          lineNumber: true,
          noEnd: false,
          captionCount: 1,
        }
      );
    }
  }

}
