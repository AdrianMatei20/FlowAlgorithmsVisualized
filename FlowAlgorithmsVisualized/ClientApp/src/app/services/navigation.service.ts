import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {

  private jsonFilePath = 'assets/algorithms.json';

  constructor(private http: HttpClient) { }

  public getJSON(): Observable<any> {
    return this.http.get(this.jsonFilePath);
  }
}
