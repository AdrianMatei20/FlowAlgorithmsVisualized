import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {

  private jsonFilePath = 'assets/algorithms.json';
  private subproblems = [];
  private chapterColors = ["#1565C0", "#C62828"];
  private subchapterColors = ["#1976D2", "#D32F2F"];
  private subsubchapterColors = ["#2196F3", "#F44336"];

  private chapterColors2 = ["#2196F3", "#F44336"];
  private subchapterColors2 = ["#1976D2", "#D32F2F"];
  private subsubchapterColors2 = ["#1565C0", "#C62828"];

  constructor(private http: HttpClient) {
    this.getJSON().subscribe(data => {
      console.log(data);
      this.subproblems = data;
    });
  }

  ngOnInit() {
  }

  public getJSON(): Observable<any> {
    return this.http.get(this.jsonFilePath);
  }

}
