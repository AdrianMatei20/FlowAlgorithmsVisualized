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

  //getCapacityNetwork(algorithm: string) {
  //  let params = new HttpParams().set("algorithm", algorithm);
  //  return this.http.get(this.baseUrl + 'algorithmsteps/capacityNetwork', { params, responseType: "text" });
  //}

  //getFlowNetwork(algorithm: string) {
  //  let params = new HttpParams().set("algorithm", algorithm);
  //  return this.http.get(this.baseUrl + 'algorithmsteps/flowNetwork', { params, responseType: "text" });
  //}

  //getAlgorithmSteps(algorithm: string) {
  //  let params = new HttpParams().set("algorithm", algorithm);
  //  return this.http.get(this.baseUrl + 'algorithmsteps/steps', { params });
  //}

  getData(algorithm: string) {
    let params = new HttpParams().set("algorithm", algorithm);
    return this.http.get(this.baseUrl + 'algorithmsteps/steps', { params });
  }
}
