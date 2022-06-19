import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NetworkService {

  private baseUrl;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getCapacityNetwork() {
    return this.http.get(this.baseUrl + 'algorithmsteps/capacityNetwork', { responseType: "text" });
  }

  getFlowNetwork() {
    return this.http.get(this.baseUrl + 'algorithmsteps/flowNetwork', { responseType: "text" });
  }
}
