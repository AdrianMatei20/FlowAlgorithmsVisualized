import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NetworkService {

  private baseUrl;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getData(algorithm: string) {
    let params = new HttpParams().set("algorithmName", algorithm);
    return this.http.get(this.baseUrl + 'algorithmsteps/steps', { params });
  }
}
