import { AfterViewInit, Component, OnInit } from '@angular/core';
import * as pseudocode from 'pseudocode';

@Component({
  selector: 'app-edmonds-karp',
  templateUrl: './edmonds-karp.component.html',
  styleUrls: ['./edmonds-karp.component.css']
})
export class EdmondsKarpComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    if (document.getElementById("pseudocode")) {
      pseudocode.renderElement(document.getElementById("pseudocode"),
        {
          lineNumber: true,
          noEnd: false,
          captionCount: 2,
        }
      );
    }
  }

}
