import { AfterViewInit, Component, OnInit } from '@angular/core';
import * as pseudocode from 'pseudocode';

@Component({
  selector: 'app-gabow',
  templateUrl: './gabow.component.html',
  styleUrls: ['./gabow.component.css']
})
export class GabowComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    if (document.getElementById("pseudocode")) {
      pseudocode.renderElement(document.getElementById("pseudocode"),
        {
          lineNumber: true,
          noEnd: false,
          captionCount: 4,
        }
      );
    }
  }

}
