import { AfterViewInit, Component, OnInit } from '@angular/core';
import * as pseudocode from 'pseudocode';

@Component({
  selector: 'app-generic-with-augmenting-path',
  templateUrl: './generic-with-augmenting-path.component.html',
  styleUrls: ['./generic-with-augmenting-path.component.css']
})
export class GenericWithAugmentingPathComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    if (document.getElementById("pseudocode")) {
      pseudocode.renderElement(document.getElementById("pseudocode"),
        {
          lineNumber: true,
          noEnd: false,
          captionCount: 0,
        }
      );
    }
  }

}
