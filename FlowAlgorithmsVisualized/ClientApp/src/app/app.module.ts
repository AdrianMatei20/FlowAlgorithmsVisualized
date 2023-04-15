import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { AlgorithmStepsComponent } from './components/algorithm-steps/algorithm-steps.component';
import { GenericWithAugmentingPathComponent } from './components/pseudocode/generic-with-augmenting-path/generic-with-augmenting-path.component';
import { FordFulkersonComponent } from './components/pseudocode/ford-fulkerson/ford-fulkerson.component';
import { EdmondsKarpComponent } from './components/pseudocode/edmonds-karp/edmonds-karp.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule, MatDividerModule, MatProgressSpinnerModule, MatToolbarModule } from '@angular/material';
import { AhujaOrlinCapacityScalingComponent } from './components/pseudocode/ahuja-orlin-capacity-scaling/ahuja-orlin-capacity-scaling.component';
import { GabowComponent } from './components/pseudocode/gabow/gabow.component';
import { AhujaOrlinShortestPathComponent } from './components/pseudocode/ahuja-orlin-shortest-path/ahuja-orlin-shortest-path.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    AlgorithmStepsComponent,
    GenericWithAugmentingPathComponent,
    FordFulkersonComponent,
    EdmondsKarpComponent,
    AhujaOrlinCapacityScalingComponent,
    GabowComponent,
    AhujaOrlinShortestPathComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'algorithm-steps/:algorithm', component: AlgorithmStepsComponent },
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: '**', redirectTo:'' },
    ]),
    BrowserAnimationsModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatDividerModule,
    MatToolbarModule,
    MatProgressSpinnerModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
